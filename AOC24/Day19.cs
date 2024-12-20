namespace AOC24;

using AOCUtil;
public sealed class Day19 : ADay
{

    public override int DayNum => 19;

    public override string Part1(string path)
    {
        IEnumerable<string> lines = File.ReadAllLines(path);
        IEnumerable<string> avaTowels = lines.First().Split(", ");
        IEnumerable<string> toMake = lines.Skip(2);
        int sum = 0;
        foreach(var seq in toMake) {
            if(TowelBuildable(seq, avaTowels)) {
                sum++;
            }
        }
        return sum.ToString();
    }

    public bool TowelPossibleRecursive(string toMake, IEnumerable<string> avalaibleTowels) {
        if(avalaibleTowels.Contains(toMake)) {
            return true;
        }
        if(avalaibleTowels.Where(x => x.Length >= toMake.Length).Any() && avalaibleTowels.Where(x => x.Length > toMake.Length).All(x => !x.Contains(toMake))) {
            return false;
        }
        for(int i = 1; i < toMake.Length; i++) {
            if(TowelPossibleRecursive(toMake[0..i], avalaibleTowels) &&
               TowelPossibleRecursive(toMake[i..^0], avalaibleTowels)) {

                return true;
            }
        }
        return false;
    }

    public bool TowelPossibleRecursiveInverse(string toMake, IEnumerable<string> avalaibleTowels) {
        if(avalaibleTowels.Contains(toMake)) {
            return true;
        }
        if(avalaibleTowels.Where(x => x.Length >= toMake.Length).Any() && avalaibleTowels.Where(x => x.Length > toMake.Length).All(x => !x.Contains(toMake))) {
            return false;
        }
        for(int i = 1; i < toMake.Length; i++) {
            if(TowelPossibleRecursive(toMake[0..i], avalaibleTowels) &&
               TowelPossibleRecursive(toMake[i..^0], avalaibleTowels)) {

                return true;
            }
        }
        return false;
    }

    public bool TowelBuildable(string toMake, IEnumerable<string> toUse) {
        return TowelBuildableAux(toMake, toUse, []);
    }

    public bool TowelBuildableAux(string toMake, IEnumerable<string> toUse, Dictionary<string, bool> memo) {
        if(toMake == "") {return true;}
        if(memo.TryGetValue(toMake, out bool res)) {return res;}
        memo[toMake] = false;
        foreach(var word in toUse) {
            if(toMake.StartsWith(word) && TowelBuildableAux(toMake[word.Length..^0], toUse, memo)) {
                memo[toMake] = true;
            }
        }
        return memo[toMake];
    }
    public bool TowelPossible(IEnumerable<string> avalaibleTowels, string toMake) {
        IEnumerable<(int, int)> allRanges = [];
        foreach(var ava in avalaibleTowels) {
            IEnumerable<int> idxs = toMake.AllIndexesOf(ava);
            IEnumerable<(int, int)> ranges = idxs.Select(x => (x, ava.Length));
            allRanges = allRanges.Concat(ranges);
        }

        return FullyCoverRanges(allRanges, toMake.Length);
    }

    public bool FullyCoverRanges(IEnumerable<(int, int)> ranges, int toCover) {
        bool[] covered = new bool[toCover];
        IEnumerable<(int,int)> currentRanges = [];
        foreach((int start, int length) in ranges) {
            if(covered[start..(start + length)].All(x => !x)) {
                for(int i = start; i < length; i++) {
                    covered[i+start] = true;
                }
            }
        }
        return false;
    }

    public override string Part2(string path)
    {
        IEnumerable<string> lines = File.ReadAllLines(path);
        IEnumerable<string> avaTowels = lines.First().Split(", ");
        IEnumerable<string> toMake = lines.Skip(2);
        long sum = 0;
        foreach(var seq in toMake) {
            long cur = TowelBuildableCount(seq, avaTowels);
            sum += cur;
        }
        return sum.ToString();
    }

    public long TowelBuildableCount(string toMake, IEnumerable<string> toUse) {
        return TowelBuildableAuxCount(toMake, toUse, []);
    }

    public long TowelBuildableAuxCount(string toMake, IEnumerable<string> toUse, Dictionary<string, long> memo) {
        if(toMake == "") {return 0;}
        if(memo.TryGetValue(toMake, out long res)) {return res;}
        memo[toMake] = toUse.Contains(toMake) ? 1 : 0;
        foreach(var word in toUse) {
            if(toMake.StartsWith(word)) {
                long laterHalf = TowelBuildableAuxCount(toMake[word.Length..^0], toUse, memo);
                if(laterHalf > 0) {
                    memo[toMake] += laterHalf;
                }
            }
        }
        return memo[toMake];
    }
}