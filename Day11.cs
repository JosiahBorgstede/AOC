public class Day11 : IDay
{
    public string Part1(string path)
    {
        string line = File.ReadAllText(path);
        List<long> startStones = line.Split(" ").Select(long.Parse).ToList();
        for(int i = 0; i < 25; i++) {
            startStones = BlinkOnce(startStones);
        }

        return startStones.Count.ToString();
    }

    public string Part2(string path)
    {
        string line = File.ReadAllText(path);
        List<long> startStones = line.Split(" ").Select(long.Parse).ToList();
        Dictionary<long, long> stonesDict = new();
        foreach(var stone in startStones) {
            stonesDict[stone] = 1;
        }
        for(int i = 0; i < 75; i++) {
            stonesDict = processStonesBatch(stonesDict);
        }

        return stonesDict.Select(x => x.Value).Sum().ToString();
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