public class Day11 : IDay {
    public int DayNum => 11;

    public string GetExpectedResult(int part) {
        if (part == 1) {
            return "194782";
        }
        return "233007586663131";
    }

    public string Part1(string path)
    {
        string line = File.ReadAllText(path);
        Dictionary<long, long> startStones = line.Split(" ").ToDictionary(x => long.Parse(x), x =>(long) 1);
        for(int i = 0; i < 25; i++) {
            startStones = processStonesBatch(startStones);
        }

        return startStones.Select(x => x.Value).Sum().ToString();
    }

    public string Part2(string path)
    {
        string line = File.ReadAllText(path);
        Dictionary<long, long> startStones = line.Split(" ").ToDictionary(x => long.Parse(x), x =>(long) 1);
        for(int i = 0; i < 75; i++) {
            startStones = processStonesBatch(startStones);
        }

        return startStones.Select(x => x.Value).Sum().ToString();
    }

    public static List<long> BlinkOnce(List<long> stones) {
        List<long> newStones = [];
        foreach(long val in stones) {
            newStones.AddRange(processStone(val));
        }
        return newStones;
    }
    public static List<long> processStone(long stone) {
        if (stone == 0) {
            return [1];
        }
        string stoneString = stone.ToString();
        if(stoneString.Length % 2 == 0) {
            long halfLength = (long) Math.Pow(10, stoneString.Length / 2);
            long bottomHalf = stone % halfLength;
            return [stone / halfLength,
                    bottomHalf];
        }
        return [stone * 2024];
    }

    public static Dictionary<long, long> processStonesBatch(Dictionary<long, long> stones)
    {
        Dictionary<long, long> newStones = new();
        foreach((long stoneVal, long stoneCount) in stones) {
            List<long> newVals = processStone(stoneVal);
            foreach(long val in newVals) {
                if(newStones.TryGetValue(val, out long curVal)) {
                    newStones[val] = curVal + stoneCount;
                } else {
                    newStones.Add(val, stoneCount);
                }
            }
        }
        return newStones;
    }
}