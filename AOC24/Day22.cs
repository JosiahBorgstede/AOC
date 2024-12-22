using System.CommandLine.Parsing;

namespace AOC24;

public class Day22 : ADay
{
    public override int DayNum => 22;

    public override string Part1(string path)
    {
        IEnumerable<long> inputs = File.ReadAllLines(path).Select(long.Parse);
        var finals = inputs.Select(x => CalculateNthSecretNumber(x, 2000));
        return finals.Sum().ToString();
    }

    public static long CalculateNthSecretNumber(long initial, int n) {
        for (int i = 0; i < n; i++)
        {
            initial = GenerateNextNumber(initial);
        }
        return initial;
    }

    public static long GenerateNextNumber(long curNumber) {
        curNumber ^= curNumber * 64;   //mix
        curNumber %= 16777216;           //prune
        curNumber ^= curNumber / 32;   //mix
        curNumber %= 16777216;           //prune
        curNumber ^= curNumber * 2048;   //mix
        curNumber %= 16777216;           //prune
        return curNumber;
    }

    public static Dictionary<(int, int, int, int), int> GetPricesAndChanges(long initial, int n) {
        Dictionary<(int, int, int, int), int> values = [];
        Queue<int> curSeq = [];
        int curPrice = (int) initial % 10;
        for (int i = 0; i < n; i++)
        {
            initial = GenerateNextNumber(initial);
            int newPrice = (int) initial % 10;
            int change = newPrice - curPrice;
            if(curSeq.Count == 4) {
                var curSeqKey = (curSeq.ElementAt(0), curSeq.ElementAt(1), curSeq.ElementAt(2), curSeq.ElementAt(3));
                if(curPrice > 0) {
                    values.TryAdd(curSeqKey, curPrice);
                }
                curSeq.Dequeue();
            }
            curSeq.Enqueue(change);
            curPrice = newPrice;
        }
        return values;
    }

    public override string Part2(string path)
    {
        IEnumerable<long> inputs = File.ReadAllLines(path).Select(long.Parse);
        var finals = inputs.Select(x => GetPricesAndChanges(x, 2000));
        var allSeqsAndVals = finals.SelectMany(x => x).GroupBy(x => x.Key);
        int curBest = 0;
        foreach(var group in allSeqsAndVals) {
            int curSum = 0;
            curSum = group.Sum(x => x.Value);
            curBest = curSum > curBest ? curSum : curBest;
        }
        return curBest.ToString();
    }
}