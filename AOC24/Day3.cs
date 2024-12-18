namespace AOC24;

using System.Text.RegularExpressions;

public class Day3 : ADay {
    public override int DayNum => 3;

    public override string Part1(string path) {
        string lines = File.ReadAllText(path);
        Regex regex = new Regex(@"mul\((?<dig1>\d{1,3}),(?<dig2>\d{1,3})\)");
        int sum = regex.Matches(lines)
                       .Select(m => int.Parse(m.Groups["dig1"].Value) * int.Parse(m.Groups["dig2"].Value))
                       .Sum();
        return sum.ToString();
    }

    public override string Part2(string path) {
        string line = File.ReadAllText(path);
        Regex regex = new Regex(@"(mul\((?<dig1>\d{1,3}),(?<dig2>\d{1,3})\))|do(n't)?\(\)");
        int sum = 0;
        bool summing = true;
        var matches = regex.Matches(line);
        foreach(Match match in matches) {
            if(match.Value == "do()") {
                summing = true;
                continue;
            }
            if(match.Value == "don't()") {
                summing = false;
                continue;
            }
            if(summing){
                sum += int.Parse(match.Groups["dig1"].Value) * int.Parse(match.Groups["dig2"].Value);
            }
        }
            //sum += matches.Select(m => int.Parse(m.Groups["dig1"].Value) * int.Parse(m.Groups["dig2"].Value)).Sum();
        return sum.ToString();
    }
}