using System.Diagnostics;

public class MainClass {
    const int TimesToRun = 50;
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
        RunDayAndPart(args[1], day, pathToInput)();
    }

    public static Action RunDayAndPart(string arg2, IDay day, string inputPath) => arg2 switch {
            "1" => () => Console.WriteLine(day.Part1(inputPath)),
            "2" => () => Console.WriteLine(day.Part2(inputPath)),
            "1T" => () => TimeDay(day.GetExpectedResult(1), TimesToRun, () => day.Part1(inputPath)),
            "2T" => () => TimeDay(day.GetExpectedResult(2), TimesToRun, () => day.Part2(inputPath)),
            "T" => () => {TimeDay(day.GetExpectedResult(1), TimesToRun, () => day.Part1(inputPath));
                          TimeDay(day.GetExpectedResult(2), TimesToRun, () => day.Part2(inputPath));},
            "TS" => () => {TimeDay(null, TimesToRun, () => day.Part1(inputPath));
                          TimeDay(null, TimesToRun, () => day.Part2(inputPath));},
            _ => throw new Exception("incorrect value for what to run"),
    };

    public static void TimeDay(string? expectedResult, int times, Func<string> toRun) {
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<TimeSpan> runTimes = [];
        Console.WriteLine($"Performing {times} runs");
        for (int i = 0; i < times; i++) {
            stopwatch.Restart();
            string result = toRun();
            stopwatch.Stop();
            runTimes.Add(stopwatch.Elapsed);
            if(expectedResult != null && result != expectedResult) {
                Console.WriteLine($"incorrect answer: {result} expected result was {expectedResult}");
                return;
            }
        }
        Console.WriteLine("Average time was " +
                          TimeSpan.FromTicks((long) runTimes.Average(x => x.Ticks)));
    }
}
