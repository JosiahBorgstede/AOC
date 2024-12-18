namespace AOC24;

using System.Diagnostics;
using System.Text.RegularExpressions;

public class Day1 : ADay {
    public override int DayNum => 1;

    public Day1() {
        _part1Versions.Add("testing", new("testing", "seeing if string splitting is faster than regex", Part1Testing));
        _part2Versions.Add("testing", new("testing", "seeing if string splitting is faster than regex", Part2Testing));
    }

    public override string Part1(string path) {
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
        return first.Zip(second).Select(pair => int.Abs(pair.First - pair.Second)).Sum().ToString();
    }

    public override string Part2(string path) {
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
        return val.ToString();
    }

    public static string Part1Testing(string path) {
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
        return first.Zip(second).Select(pair => int.Abs(pair.First - pair.Second)).Sum().ToString();
    }

    public static string Part2Testing(string path) {
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
        return sum.ToString();
    }
}