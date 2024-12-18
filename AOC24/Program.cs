namespace AOC24;

using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

public static class MainClass {
    const int TimesToRun = 50;
    public static async Task Main(string[] args) {

        var dayArgument = new Argument<int>("day", "the day to run");
        var partArgument = new Argument<int>(name: "part", description: "the part to run", getDefaultValue: () => 0);
        partArgument.FromAmong("0", "1", "2", "-1", "-2");
        var typeOption = new Option<string>("--style", ()=>"base", "the version to test. Must be configured in the Day constructor");
        var checkResultOption = new Option<bool>(name: "--check", description: "Will check the result of the base version to ensure all versions are correct");
        var fileOption = new Option<string>(name: "--file", () => "", description: "the file to run") {IsRequired = true};
        var timingOption = new Option<bool>(name: "--time", description: "Will time how long a part takes to run");
        timingOption.AddAlias("-t");
        var debugOption = new Option<bool>(name: "--debug", description: "Will time how long a part takes to run");
        debugOption.AddAlias("-d");

        var runsOption = new Option<int>(name: "--runs", description: "How many times to run the timing to determine the average", getDefaultValue: () => TimesToRun);

        var listVersionsCommand = new Command("list", "lists the different versions a day has");
        listVersionsCommand.AddArgument(dayArgument);
        listVersionsCommand.SetHandler(ListVersions, dayArgument);

        var rootCommand = new RootCommand("Running a day of the Avent of Code challenge of 2024");
        rootCommand.AddArgument(dayArgument);
        rootCommand.AddArgument(partArgument);
        rootCommand.AddOption(fileOption);
        rootCommand.AddOption(typeOption);
        rootCommand.AddOption(checkResultOption);
        rootCommand.AddOption(timingOption);
        rootCommand.AddOption(runsOption);
        rootCommand.AddOption(debugOption);
        rootCommand.AddCommand(listVersionsCommand);

        rootCommand.SetHandler(RunningTheDay, dayArgument, partArgument, fileOption, typeOption, checkResultOption, timingOption, runsOption);
        await rootCommand.InvokeAsync(args);
    }

    public static IDay getDay(int dayNum) =>  dayNum switch  {
            1 => new Day1(),
            2 => new Day2(),
            3 => new Day3(),
            4 => new Day4(),
            5 => new Day5(),
            6 => new Day6(),
            7 => new Day7(),
            8 => new Day8(),
            9 => new Day9(),
            10 => new Day10(),
            11 => new Day11(),
            12 => new Day12(),
            13 => new Day13(),
            14 => new Day14(),
            15 => new Day15(),
            16 => new Day16(),
            17 => new Day17(),
            18 => new Day18(),
            _ => throw new Exception("Day not added"),
        };

    public static void ListVersions(int dayNum) {
        IDay day = getDay(dayNum);
        Console.WriteLine("All of the different versions for day " + dayNum);
        Console.WriteLine("Part 1:");
        foreach(var version in day.Part1Versions) {
            Console.WriteLine($"{version.name}: {version.description}");
        }
        Console.WriteLine("Part 2:");
        foreach(var version in day.Part2Versions) {
            Console.WriteLine($"{version.name}: {version.description}");
        }
    }
    public static void RunningTheDay(int dayNum, int part, string filePath, string type = "base", bool checkRes = false, bool timeRun = false, int times = TimesToRun) {
        IDay day = getDay(dayNum);
        if(filePath == "") {
            filePath = $"./Inputs/Day{dayNum}.txt";
        }
        switch (part) {
            case 0:
                RunSinglePart(day, 1, filePath, type, checkRes, timeRun, times);
                RunSinglePart(day, 2, filePath, type, checkRes, timeRun, times);
                break;
            case 1:
            case 2:
                RunSinglePart(day, part, filePath, type, checkRes, timeRun, times);
                break;
            case -1:
            case -2:
                RunAllVersionsOfPart(dayNum, Math.Abs(part), filePath, checkRes, timeRun, times);
                break;
        }
    }

    public static void RunSinglePart(IDay day, int part, string filePath, string type = "base", bool checkRes = false, bool timeRun = false, int times = TimesToRun) {
        Console.WriteLine($"Day: {day.DayNum} Part: {part} ");
        Func<string> toRun = () => day[part, type](filePath);
        if(timeRun) {
            toRun = toRun.TimeFunction(times);
        }
        if(checkRes) {
            toRun = toRun.CheckFunc(day.GetExpectedResult(part, filePath));
        }
        Console.WriteLine("Result: " + toRun());
    }

    public static void RunAllVersionsOfPart(int dayNum, int part, string filePath, bool checkRes = false, bool timeRun = false, int times = TimesToRun) {
        IDay day = getDay(dayNum);
        foreach(var version in day.Part1Versions) {
            Console.WriteLine($"Version: {version.name}");
            RunSinglePart(day, part, filePath, version.name, checkRes, timeRun, times);
        }
    }

    public static Func<string> RunASpecifiedDayPart(IDay day, int part, string inputPath, string version = "base") => () => {
        return day[part, version](inputPath);
    };

    public static Func<string> CheckFunc(this Func<string> toRun, string expectedResult) => () => {
        string res = toRun();
        if(res != expectedResult) {
            Console.WriteLine($"Incorrect Result: {res}, expected result was: {expectedResult}");
        } else {
            Console.WriteLine("result was correct");
        }
        return res;
    };

    public static Func<string> TimeFunction(this Func<string> toRun, int times) => () => {
        List<TimeSpan> runTimes = [];
        Console.WriteLine($"Performing {times} runs");
        string res = "";
        for (int i = 0; i < times; i++) {
            runTimes.Add(TimeSingleRun(toRun, out res));
        }
        Console.WriteLine("Average time was " +
                          TimeSpan.FromTicks((long) runTimes.Average(x => x.Ticks)));
        return res;
    };
    public static void TimeFunctionChecked(int times, Func<string> toRun, string expectedResult) {
        List<TimeSpan> runTimes = [];
        Console.WriteLine($"Performing {times} runs");
        for (int i = 0; i < times; i++) {
            runTimes.Add(TimeSingleRun(toRun, out string result));
            if(result != expectedResult) {
                Console.WriteLine($"incorrect answer: {result} expected result was {expectedResult}");
                return;
            }
        }
        Console.WriteLine("Average time was " +
                          TimeSpan.FromTicks((long) runTimes.Average(x => x.Ticks)));
    }

    private static TimeSpan TimeSingleRun(Func<string> toRun, out string result) {
        Stopwatch stopwatch = Stopwatch.StartNew();
        result = toRun();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}
