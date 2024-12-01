
using System.Text.RegularExpressions;

public class Day1 {
    public static void Part1(string path) {
        List<string> lines = File.ReadAllLines(path).ToList();
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

    public static void Part2(string path) {
        List<string> lines = File.ReadAllLines(path).ToList();
        Regex getNumbers = new Regex(@"(?<num1>\d*)   (?<num2>\d*)");
        IEnumerable<int> first = lines.Select(line => {
            Match val = getNumbers.Match(line);
            return int.Parse(val.Groups["num1"].ToString());
        }).Order();
        IEnumerable<int> second = lines.Select(line => {
            Match val = getNumbers.Match(line);
            return int.Parse(val.Groups["num2"].ToString());
        }).Order();
        Console.WriteLine(first.Select(inVal => inVal * second.Count(twoVal => twoVal == inVal)).Sum());
    }
}