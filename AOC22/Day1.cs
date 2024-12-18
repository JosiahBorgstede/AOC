namespace AOC22;

public class Day1 {
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        List<int> elvesCals = new List<int>();
        int curSum = 0;
        foreach (string line in lines) {
            if(int.TryParse(line, out int cur)) {
                curSum += cur;
            } else {
                elvesCals.Add(curSum);
                curSum = 0;
            }
        }
        //elvesCals.Sort();
        Console.WriteLine(elvesCals.Max());
    }

    public static void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        List<int> elvesCals = new List<int>();
        int curSum = 0;
        foreach (string line in lines) {
            if(int.TryParse(line, out int cur)) {
                curSum += cur;
            } else {
                elvesCals.Add(curSum);
                curSum = 0;
            }
        }
        var res = elvesCals.OrderDescending();
        Console.WriteLine(res.ElementAt(0) + res.ElementAt(1) + res.ElementAt(2));
    }
}