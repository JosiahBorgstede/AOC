
public class Day5 : IDay {

    public void Run(string part, string path) {
        if(part == "1") {
            Part1(path);
        } else {
            Part2(path);
        }
    }

    public void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<string> rules = lines.TakeWhile(x => x.Length != 0);
        IEnumerable<string> seqs = lines.SkipWhile(x => x.Length != 0).Skip(1);
        Dictionary<int, List<int>> trueRules = GetRules(rules);
        int sum = 0;
        foreach (var seq in seqs) {
            var sequence = seq.Split(',').Select(int.Parse);
            if(CheckSingleSequence(sequence, trueRules)) {
                sum += sequence.ElementAt(sequence.Count()/2);
            }
        }
        Console.WriteLine(sum);
    }

    public void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<string> rules = lines.TakeWhile(x => x.Length != 0);
        IEnumerable<string> seqs = lines.SkipWhile(x => x.Length != 0).Skip(1);
        Dictionary<int, List<int>> trueRules = GetRules(rules);
        int sum = 0;
        foreach (var seq in seqs) {
            var sequence = seq.Split(',').Select(int.Parse);
            if(CheckSingleSequence(sequence, trueRules)) {
                //sum += sequence.ElementAt(sequence.Count()/2);
            } else {
                var newSeq = reorderedSequence(sequence, trueRules);
                sum += newSeq.ElementAt(sequence.Count()/2);
            }
        }
        Console.WriteLine(sum);
    }

    public static Dictionary<int, List<int>> GetRules(IEnumerable<string> rules)
    {
        Dictionary<int, List<int>> resultRules = new Dictionary<int, List<int>>();
        foreach (string line in rules) {
            var ints = line.Split('|').Select(int.Parse);
            int num1 = ints.ElementAt(0);
            int num2 = ints.ElementAt(1);
            if(resultRules.TryGetValue(num2, out List<int>? result)) {
                resultRules[num2].Add(num1);
            } else {
                resultRules[num2] = [num1];
            }
        }
        return resultRules;
    }

    public static bool CheckSingleSequence(IEnumerable<int> seq, Dictionary<int, List<int>> rules) {
        Dictionary<int, bool> visited = rules.ToDictionary(x => x.Key, x => false);
        foreach (int cur in seq) {
            if(rules[cur].Any(x =>  seq.Contains(x) && visited[x] == false)) {
                return false;
            }
            visited[cur] = true;
        }
        return true;
    }

    public static IEnumerable<int> reorderedSequence(IEnumerable<int> seq, Dictionary<int, List<int>> rules) {
        return seq.Order(Comparer<int>.Create((x, y) => {
            if(rules[x].Contains(y)) {
                return 1;
            } else if(rules[y].Contains(x)) {
                return -1;
            } else {
                return 0;
            }
            }));
    }

}