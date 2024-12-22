namespace AOC24;

using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Reflection;

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
        var debugOption = new Option<bool>(name: "--debug", description: "Will enable debug printing (currently not implemented)");
        debugOption.AddAlias("-d");

        var runsOption = new Option<int>(name: "--runs", description: "How many times to run the timing to determine the average", getDefaultValue: () => TimesToRun);
        runsOption.AddAlias("-r");

        var listVersionsCommand = new Command("list", "lists the different versions a day has");
        listVersionsCommand.AddArgument(dayArgument);
        listVersionsCommand.SetHandler(ListVersions, dayArgument);

        var compareSpeedsCommand = new Command("compare", "compares the speed of 2 parts of a day");
        var typeArgument= new Argument<List<string>>("styles", () => [], "the first version to test. Must be configured in the Day constructor");
        var comparePartArgument = new Argument<int>("part", "the part where the types to comapre are, must be specified");
        partArgument.FromAmong("1", "2");
        compareSpeedsCommand.AddArgument(dayArgument);
        compareSpeedsCommand.AddArgument(comparePartArgument);
        compareSpeedsCommand.AddArgument(typeArgument);
        compareSpeedsCommand.SetHandler(CompareParts, dayArgument, comparePartArgument, typeArgument, fileOption, runsOption, debugOption);

        var rootCommand = new RootCommand("Running a day of the Avent of Code challenge of 2024");
        rootCommand.AddArgument(dayArgument);
        rootCommand.AddArgument(partArgument);
        rootCommand.AddGlobalOption(fileOption);
        rootCommand.AddGlobalOption(debugOption);
        rootCommand.AddOption(typeOption);
        rootCommand.AddOption(checkResultOption);
        rootCommand.AddOption(timingOption);
        rootCommand.AddGlobalOption(runsOption);
        rootCommand.AddOption(debugOption);
        rootCommand.AddCommand(listVersionsCommand);
        rootCommand.AddCommand(compareSpeedsCommand);

        rootCommand.SetHandler(RunningTheDay, dayArgument, partArgument, fileOption, typeOption, checkResultOption, timingOption, runsOption, debugOption);
        await rootCommand.InvokeAsync(args);
    }

    private static void CompareParts(int dayArgument, int part, List<string> types, string filePath, int runs, bool debug)
    {
        if(types.Count == 0) {
            RunningTheDay(dayArgument, -part, filePath, checkRes: true, timeRun: true, times: runs, debug: debug);
        } else {
            foreach(var type in types) {
                try{
                    Console.WriteLine($"Version: {type}");
                    RunningTheDay(dayArgument, part, filePath, type, true, true, runs, debug: debug);
                } catch {
                    Console.WriteLine($"Invalid type given: {type}");
                }
            }
        }
    }


    public static IDay getDay(int dayNum) => dayNum switch  {
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
            19 => new Day19(),
            20 => new Day20(),
            21 => new Day21(),
            _ => throw new Exception("Day not added"),
        };

    public static void ListVersions(int dayNum) {
        IDay day = getDay(dayNum);
        Console.WriteLine("All of the different versions for day " + dayNum);
        foreach((var att, var _) in GetAOCChallenges(day)) {
            string description = att.Description != null ? " Description: " + att.Description : "";
            Console.WriteLine($"Part: {att.Part}, Name: {att.Name.ToString().PadRight(10)}" + description);
        }
    }

    public static void RunningTheDay(int dayNum, int part, string filePath, string type = "base", bool checkRes = false, bool timeRun = false, int times = TimesToRun, bool debug = false) {
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
            Func<string> toRun = RunASpecifiedDayPart(day, part, filePath, type);
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
        RunEntireDay(dayNum, part, filePath);
        foreach((var att, var func) in GetAOCChallenges(day)) {
            if(att.Part == part) {
                Console.WriteLine(func(filePath));
            }
        }
    }

    public static void RunEntireDay(int dayNum, int part, string filePath) {
        IDay day = getDay(dayNum);
        foreach((var _, var func) in GetAOCChallenges(day)) {
            Console.WriteLine(func(filePath));
        }
    }

    public static Func<string> RunASpecifiedDayPart(IDay day, int part, string inputPath, string version = "base") =>
    () => GetSpecificAOCChallenges(day, part, version)(inputPath);

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
        Console.WriteLine("Average time was " + TimeSpan.FromTicks((long) runTimes.Average(x => x.Ticks)));
        return res;
    };

    private static TimeSpan TimeSingleRun(Func<string> toRun, out string result) {
        Stopwatch stopwatch = Stopwatch.StartNew();
        result = toRun();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public static IEnumerable<(AOCAttribute,Func<string, string?>)> GetAOCChallenges(IDay day) {
        List<(AOCAttribute,Func<string, string?>)> challenges = [];
        MethodInfo[] MyMemberInfo = day.GetType().GetMethods();
        for (int i = 0; i < MyMemberInfo.Length; i++)
        {
            AOCAttribute? att = (AOCAttribute?) Attribute.GetCustomAttribute(MyMemberInfo[i], typeof (AOCAttribute));
            if (att != null)
            {
                MethodInfo cur = MyMemberInfo[i];
                challenges.Add((att, (s) => (string?) cur.Invoke(day, [s])));
            }
        }
        return challenges;
    }

    public static Func<string, string> GetSpecificAOCChallenges(IDay day, int part, string name = "base") {
        MethodInfo[] MyMemberInfo = day.GetType().GetMethods();
        for (int i = 0; i < MyMemberInfo.Length; i++)
        {
            AOCAttribute? att = (AOCAttribute?) Attribute.GetCustomAttribute(MyMemberInfo[i], typeof (AOCAttribute));
            if (att != null && att.Name == name && att.Part == part)
            {
                MethodInfo cur = MyMemberInfo[i];
                return (s) => (string) cur.Invoke(day, [s])!;
            }
        }
        throw new ArgumentException($"Method named {name} not found");
    }
}
