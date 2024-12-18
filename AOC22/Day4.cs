namespace AOC22;
using System.Text.RegularExpressions;

public class Day4 {
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Regex assignmentGet = new Regex(@"(?<start1>\d+)-(?<end1>\d+),(?<start2>\d+)-(?<end2>\d+)");
        int overlaps = 0;
        foreach (string line in lines) {
            Match match = assignmentGet.Match(line);
            if(int.Parse(match.Groups["start1"].Value) >= int.Parse(match.Groups["start2"].Value) &&
               int.Parse(match.Groups["end1"].Value) <= int.Parse(match.Groups["end2"].Value)) {
                overlaps++;
            }
            else if(int.Parse(match.Groups["start2"].Value) >= int.Parse(match.Groups["start1"].Value) &&
               int.Parse(match.Groups["end2"].Value) <= int.Parse(match.Groups["end1"].Value)) {
                overlaps++;
            }
        }
        Console.WriteLine(overlaps);
    }

    public static void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Regex assignmentGet = new Regex(@"(?<start1>\d+)-(?<end1>\d+),(?<start2>\d+)-(?<end2>\d+)");
        int overlaps = 0;
        foreach (string line in lines) {
            Match match = assignmentGet.Match(line);
            if(int.Parse(match.Groups["start1"].Value) >= int.Parse(match.Groups["start2"].Value) &&
               int.Parse(match.Groups["start1"].Value) <= int.Parse(match.Groups["end2"].Value)) {
                overlaps++;
            }
            else if(int.Parse(match.Groups["end1"].Value) >= int.Parse(match.Groups["start2"].Value) &&
               int.Parse(match.Groups["end1"].Value) <= int.Parse(match.Groups["end2"].Value)) {
                overlaps++;
            }
            else if(int.Parse(match.Groups["end2"].Value) >= int.Parse(match.Groups["start1"].Value) &&
               int.Parse(match.Groups["end2"].Value) <= int.Parse(match.Groups["end1"].Value)) {
                overlaps++;
            }
            else if(int.Parse(match.Groups["start2"].Value) >= int.Parse(match.Groups["start1"].Value) &&
               int.Parse(match.Groups["start2"].Value) <= int.Parse(match.Groups["end1"].Value)) {
                overlaps++;
            }
        }
        Console.WriteLine(overlaps);
    }


}