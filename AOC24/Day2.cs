namespace AOC24;

public class Day2 : ADay {
    public override int DayNum => 2;

    public override string Part1(string path) {
        List<string> lines = File.ReadAllLines(path).ToList();
        IEnumerable<IEnumerable<int>> levels = lines.Select(x => x.Split(' ').Select(int.Parse));
        int safeLines = 0;
        foreach (var level in levels) {
            bool isSafe = true;
            bool increasing = level.First() < level.ElementAt(1);
            int initial = increasing ? level.First() - 1 : level.First() + 1;
            foreach (var val in level) {
                if (increasing && val > initial && val - initial <= 3) {
                    initial = val;
                } else if(!increasing && val < initial && initial - val <= 3) {
                    initial = val;
                }
                else {
                    isSafe = false;
                    break;
                }
            }
            if (isSafe) {safeLines++;}
        }
        return levels.Count(safeLevel).ToString();
    }

    public override string Part2(string path) {
        List<string> lines = File.ReadAllLines(path).ToList();
        IEnumerable<IEnumerable<int>> levels = lines.Select(x => x.Split(' ').Select(int.Parse));
        var unsafeLevels = levels.Where(level => !safeLevel(level));
        int safeLevels = levels.Count() - unsafeLevels.Count();
        foreach (var level in unsafeLevels) {
            if(canBeSafeLevel(level)) {
                safeLevels++;
            }
        }
        return safeLevels.ToString();
    }

    public static bool safeStep(int prev, int cur, bool increasing) {
        if (increasing && cur > prev && cur - prev <= 3) {
            return true;
        } else if(!increasing && cur < prev && prev - cur <= 3) {
            return true;
        }
        return false;
    }

    public static bool safeLevel(IEnumerable<int> level) {
        bool increasing = level.First() < level.ElementAt(1);
        int initial = increasing ? level.First() - 1 : level.First() + 1;
        foreach (var val in level) {
            if(!safeStep(initial, val, increasing)) {
                return false;
            } else {
                initial = val;
            }
        }
        return true;
    }

    public static bool canBeSafeLevel(IEnumerable<int> level) {
        for (int i = 0; i < level.Count(); i++) {
                List<int> levelCopy = level.ToList();
                levelCopy.RemoveAt(i);
                if(safeLevel(levelCopy)) {
                    return true;
                }
            }
        return false;
    }
}