using System.Text.RegularExpressions;
namespace AOC22;

public class Day21 {

    static Dictionary<string, string> monkeys = new Dictionary<string, string>();
    static Dictionary<string, double> monkeysVals = new Dictionary<string, double>();
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Regex monkey = new Regex(@"(?<name>\D{4}): (?<exp>(\d+)|(\D{4} (\*|\+|-|/) \D{4}))");
        foreach(string line in lines) {
            Match match = monkey.Match(line);
            monkeys[match.Groups["name"].Value] = match.Groups["exp"].Value;
            Console.WriteLine(match.Groups["exp"].Value);
        }
        Console.WriteLine(ProcessMonkeys("root"));
    }

    public static double ProcessMonkeys(string start) {
        if(int.TryParse(monkeys[start], out int result)) {
            if(result > 100) {
                Console.WriteLine(start + result);
            }
            return result;
        }
        Regex op = new Regex(@"(?<name1>\D{4}) (?<op>\*|\+|-|/) (?<name2>\D{4})");
        Match match = op.Match(monkeys[start]);
        switch (match.Groups["op"].Value) {
            case "*":
                return ProcessMonkeys(match.Groups["name1"].Value) * ProcessMonkeys(match.Groups["name2"].Value);
            case "-":
                return ProcessMonkeys(match.Groups["name1"].Value) - ProcessMonkeys(match.Groups["name2"].Value);
            case "+":
                return ProcessMonkeys(match.Groups["name1"].Value) + ProcessMonkeys(match.Groups["name2"].Value);
            case "/":
                return ProcessMonkeys(match.Groups["name1"].Value) / ProcessMonkeys(match.Groups["name2"].Value);
            default:
                return 0;
        }
    }

    public static void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Regex monkey = new Regex(@"(?<name>\D{4}): (?<exp>(\d+)|(\D{4} (\*|\+|-|/) \D{4}))");
        foreach(string line in lines) {
            Match match = monkey.Match(line);
            monkeys[match.Groups["name"].Value] = match.Groups["exp"].Value;
            //Console.WriteLine(match.Groups["exp"].Value);
        }
        var vals = monkeys["root"].Split(" ");
        var val = ProcessMonkeys(vals[2]);
        Console.WriteLine(val);
        var cur = 3423279932999;
        var curCal = ProcessMonkeysAndHuman(vals[0], cur);
        while(curCal < val) {
            Console.WriteLine(curCal + " vs " + val);
            Console.WriteLine("diff= " + (val- curCal));
            cur--;
            curCal = ProcessMonkeysAndHuman(vals[0], cur);
        }
        Console.WriteLine(cur);
    }

    public static double ProcessMonkeysAndHuman(string start, double human) {
        if(start == "humn") {return human;}
        if(int.TryParse(monkeys[start], out int result)) {
            if(result > 100) {
                Console.WriteLine(start + result);
            }
            return result;
        }
        Regex op = new Regex(@"(?<name1>\D{4}) (?<op>\*|\+|-|/) (?<name2>\D{4})");
        Match match = op.Match(monkeys[start]);
        switch (match.Groups["op"].Value) {
            case "*":
                return ProcessMonkeysAndHuman(match.Groups["name1"].Value, human) * ProcessMonkeysAndHuman(match.Groups["name2"].Value, human);
            case "-":
                return ProcessMonkeysAndHuman(match.Groups["name1"].Value, human) - ProcessMonkeysAndHuman(match.Groups["name2"].Value, human);
            case "+":
                return ProcessMonkeysAndHuman(match.Groups["name1"].Value, human) + ProcessMonkeysAndHuman(match.Groups["name2"].Value, human);
            case "/":
                return ProcessMonkeysAndHuman(match.Groups["name1"].Value, human) / ProcessMonkeysAndHuman(match.Groups["name2"].Value, human);
            default:
                return 0;
        }
    }
}