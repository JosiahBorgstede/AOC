using System.Collections;
using System.Diagnostics;

public class MainClass {

    const int TimesToRun = 20;
    public static void Main(string[] args) {
        if(args.Length < 2) {
            Console.WriteLine("missing args");
            return;
        }
        string pathToInput = args.Length == 2 ? $"./Inputs/Day{args[0]}.txt" : args[2];
        int dayNum = int.Parse(args[0]);
        IDay day =  dayNum switch  {
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
            _ => throw new Exception(""),
        };
        RunDayAndPart(args[1], day, pathToInput);
    }

    public static void RunDayAndPart(string arg2, IDay day, string inputPath) {
        switch (arg2) {
            case "1":
                Console.WriteLine(day.Part1(inputPath));
                break;
            case "2":
                Console.WriteLine(day.Part2(inputPath));
                break;
            case "1T":
                Console.WriteLine($"Timing day {day.DayNum} part 1");
                TimeDay(day, 1, TimesToRun, inputPath);
                break;
            case "2T":
                Console.WriteLine($"Timing day {day.DayNum} part 2");
                TimeDay(day, 2, TimesToRun, inputPath);
                break;
            case "T":
                Console.WriteLine($"Timing day {day.DayNum}");
                Console.WriteLine("Part 1");
                TimeDay(day, 1, TimesToRun, inputPath);
                Console.WriteLine("Part 2");
                TimeDay(day, 2, TimesToRun, inputPath);
                break;
        }
    }

    public static void TimeDay(IDay dayToRun, int part, int times, string path) {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Func<string, string> toRun = part switch {
            1 => dayToRun.Part1,
            2 => dayToRun.Part2,
            _ => dayToRun.Part1
        };
        List<TimeSpan> runTimes = [];
        Console.WriteLine($"Performing {times} runs");
        for (int i = 0; i < times; i++) {
            stopwatch.Restart();
            string result = toRun(path);
            stopwatch.Stop();
            runTimes.Add(stopwatch.Elapsed);
            if(result != dayToRun.GetExpectedResult(part)) {
                Console.WriteLine($"incorrect answer: {result} expected result was {dayToRun.GetExpectedResult(part)}");
                return;
            }
        }

        Console.WriteLine("Average time was " + TimeSpan.FromTicks((long) runTimes.Average(x => x.Ticks)));
    }
}
