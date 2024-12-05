using System.Diagnostics;
using System.Text.RegularExpressions;

public class Day1 {
    public static void Run(string part, string path) {
        if(part == "1") {
            Part1(path);
        } else {
            Part2(path);
        }
    }

    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Regex getNumbers = new Regex(@"(?<num1>\d*)   (?<num2>\d*)");
        IEnumerable<int> first = lines.Select(line => {
            Match val = getNumbers.Match(line);
            return int.Parse(val.Groups["num1"].ToString());
        }).Order();
        IEnumerable<int> second = lines.Select(line => {
            Match val = getNumbers.Match(line);
            return int.Parse(val.Groups["num2"].ToString());
        }).Order();
        Console.WriteLine(first.Zip(second).Select(pair => int.Abs(pair.First - pair.Second)).Sum());
    }

    public static int Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Regex getNumbers = new Regex(@"(?<num1>\d*)   (?<num2>\d*)");
        IEnumerable<int> first = lines.Select(line => {
            Match val = getNumbers.Match(line);
            return int.Parse(val.Groups["num1"].ToString());
        }).Order();
        IEnumerable<int> second = lines.Select(line => {
            Match val = getNumbers.Match(line);
            return int.Parse(val.Groups["num2"].ToString());
        }).Order();
        int val = first.Select(inVal => inVal * second.Count(twoVal => twoVal == inVal)).Sum();
        Console.WriteLine(val);
        return val;
    }

    public static void Part1Testing(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        List<int> first = new List<int>();
        List<int> second = new List<int>();
        foreach(var line in lines) {
            IEnumerable<int> numbers = line.Split("   ").Select(int.Parse);
            first.Add(numbers.First());
            second.Add(numbers.Last());
        }
        first.Sort();
        second.Sort();
        Console.WriteLine(first.Zip(second).Select(pair => int.Abs(pair.First - pair.Second)).Sum());
    }

    public static int Part2Testing(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        List<int> first = new List<int>();
        Dictionary<int, int> second = new();
        foreach(var line in lines) {
            IEnumerable<int> numbers = line.Split("   ").Select(int.Parse);
            first.Add(numbers.First());
            second[numbers.Last()] = second.TryGetValue(numbers.Last(), out int val) ? val + 1 : 1;
        }
        int sum = 0;
        foreach(var num in first) {
            sum += num * (second.TryGetValue(num, out int val) ? val : 0);
        }
        //first.ForEach(inVal => sum += inVal * (second.TryGetValue(inVal, out int val) ? val : 0));
        return sum;
    }

    public static void Part2WithTimings(string path) {
        //var lines = File.ReadLines(path).ToList();
        var watch = Stopwatch.StartNew();
        int result = Part2Testing(path);
        watch.Stop();
        Console.WriteLine(result);
        Console.WriteLine(watch.Elapsed.ToString());
        watch.Restart();
        result = Part2Testing(path);
        watch.Stop();
        Console.WriteLine(result);
        Console.WriteLine(watch.Elapsed.ToString());
        watch.Restart();
        result = Part2Testing(path);
        watch.Stop();
        Console.WriteLine(result);
        Console.WriteLine(watch.Elapsed.ToString());
    }
}