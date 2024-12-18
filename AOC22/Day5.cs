namespace AOC22;
using System.Text.RegularExpressions;

public class Day5 {
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<string> crates = lines.TakeWhile(l => l.Length > 0).SkipLast(1);
        List<Stack<char>> boxes = MakeBoxStacks(crates);
        IEnumerable<string> movements = lines.SkipWhile(l => l.Length > 1).Skip(1);
        foreach (var line in movements) {
            Console.WriteLine(line);
            Match match = Regex.Match(line, @"move (?<x>\d+) from (?<y>\d+) to (?<z>\d+)");
            int x = int.Parse(match.Groups["x"].Value);
            int y = int.Parse(match.Groups["y"].Value);
            int z = int.Parse(match.Groups["z"].Value);
            if (match.Success) {
                for (int i = 0; i < x; i++) {
                    boxes[z].Push(boxes[y].Pop());
                }
            }
        }
        foreach (var line in boxes) {
            if(line.TryPeek(out char result)) {
                Console.Write(result);
            }
        }
    }

    public static List<Stack<char>> MakeBoxStacks(IEnumerable<string> crates) {
        IEnumerable<string> revCrates = crates.Reverse();
        List<Stack<char>> boxes = new List<Stack<char>>();
        for(int i = 0; i < 10; i ++) {
            boxes.Add(new Stack<char>());
        }
        foreach(var line in revCrates) {
            IEnumerable<char[]> chunks = line.Chunk(4);
            for(int i = 0; i< chunks.Count(); i++) {
                if(chunks.ElementAt(i)[0] == '[') {
                    boxes[i+1].Push(chunks.ElementAt(i)[1]);
                }
            }
        }
        return boxes;
    }

    public static void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<string> crates = lines.TakeWhile(l => l.Length > 0).SkipLast(1);
        List<Stack<char>> boxes = MakeBoxStacks(crates);
        IEnumerable<string> movements = lines.SkipWhile(l => l.Length > 1).Skip(1);
        foreach (var line in movements) {
            Console.WriteLine(line);
            Match match = Regex.Match(line, @"move (?<x>\d+) from (?<y>\d+) to (?<z>\d+)");
            int x = int.Parse(match.Groups["x"].Value);
            int y = int.Parse(match.Groups["y"].Value);
            int z = int.Parse(match.Groups["z"].Value);
            if (match.Success) {
                char[] moved = new char[x];
                for (int i = 0; i < x; i++) {
                    moved[i] = boxes[y].Pop();
                }
                var rev = moved.Reverse();
                foreach(char reved in rev) {
                    boxes[z].Push(reved);
                }
            }
        }
        foreach (var line in boxes) {
            if(line.TryPeek(out char result)) {
                Console.Write(result);
            }
        }
    }
}