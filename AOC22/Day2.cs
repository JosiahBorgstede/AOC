namespace AOC22;
public class Day2 {

    const int winValue = 6;
    const int loseValue = 0;
    const int tieValue = 3;
    const int rockValue = 1;
    const int paperValue = 2;
    const int sisValue = 3;
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        int score = 0;
        foreach (string line in lines) {
            string[] parts = line.Split(" ");
            score += RPSResult(parts[0], parts[1]);
        }
        Console.WriteLine(score);
    }

    public static void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        int score = 0;
        foreach (string line in lines) {
            string[] parts = line.Split(" ");
            score += RPSResult2(parts[0], parts[1]);
        }
        Console.WriteLine(score);
    }

    public static int RPSResult(string opp, string play) {
        switch (opp) {
            case "A":
                switch (play) {
                    case "X":
                        return tieValue + rockValue;
                    case "Y":
                        return winValue + paperValue;
                    case "Z":
                        return loseValue + sisValue;
                    default:
                        return 0;
                }
            case "B":
                switch (play) {
                    case "X":
                        return loseValue + rockValue;
                    case "Y":
                        return tieValue + paperValue;
                    case "Z":
                        return winValue + sisValue;
                    default:
                        return 0;
                }
            case "C":
                switch (play) {
                    case "X":
                        return winValue + rockValue;
                    case "Y":
                        return loseValue + paperValue;
                    case "Z":
                        return tieValue + sisValue;
                    default:
                        return 0;
                }
            default:
                return 0;
        }
    }

    public static int RPSResult2(string opp, string play) {
        switch (opp) {
            case "A":
                switch (play) {
                    case "X":
                        return loseValue + sisValue;
                    case "Y":
                        return tieValue + rockValue;
                    case "Z":
                        return winValue + paperValue;
                    default:
                        return 0;
                }
            case "B":
                switch (play) {
                    case "X":
                        return loseValue + rockValue;
                    case "Y":
                        return tieValue + paperValue;
                    case "Z":
                        return winValue + sisValue;
                    default:
                        return 0;
                }
            case "C":
                switch (play) {
                    case "X":
                        return loseValue + paperValue;
                    case "Y":
                        return tieValue + sisValue;
                    case "Z":
                        return winValue + rockValue;
                    default:
                        return 0;
                }
            default:
                return 0;
        }
    }
}