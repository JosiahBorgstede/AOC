public class MainClass {
    public static void Main(string[] args) {
        string pathToInput = args.Length <= 2 ? $"./Inputs/Day{args[0]}.txt" : args[2];
        switch (args[0]) {
            case "1":
                Day1.Run(args[1], pathToInput);
                break;
            case "2":
                Day2.Run(args[1], pathToInput);
                break;
            case "3":
                Day3.Run(args[1], pathToInput);
                break;
            case "4":
                Day4.Run(args[1], pathToInput);
                break;
            case "5":
                Day5.Run(args[1], pathToInput);
                break;
            case "6":
                Day6.Run(args[1], pathToInput);
                break;
        }
    }
}
