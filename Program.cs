using System.Collections;

public class MainClass {
    public static void Main(string[] args) {
        if(args.Length < 2) {
            Console.WriteLine("missing args");
            return;
        }
        string pathToInput = args.Length == 2 ? $"./Inputs/Day{args[0]}.txt" : args[2];
        IDay day = args[0] switch  {
            "1" => new Day1(),
            "2" => new Day2(),
            "3" => new Day3(),
            "4" => new Day4(),
            "5" => new Day5(),
            "6" => new Day6(),
            "7" => new Day7(),
            "8" => new Day8(),
            "9" => new Day9(),
            _ => throw new Exception(""),
        };
        switch (args[1]) {
            case "1":
                day.Part1(pathToInput);
                break;
            case "2":
                day.Part2(pathToInput);
                break;
        }
    }
}
