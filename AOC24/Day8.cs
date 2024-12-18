namespace AOC24;

public class Day8 : ADay {
    public override int DayNum => 8;

    public override string Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Dictionary<char, List<(int, int)>> antennas = GetAntennas(lines);
        int maxX = lines.ElementAt(0).Length -1;
        int maxY = lines.Count() - 1;
        return antennas.SelectMany(x => GetAllAntinodes(x.Value, maxX, maxY))
                                  .Distinct()
                                  .Count()
                                  .ToString();
    }

    public override string Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Dictionary<char, List<(int, int)>> antennas = GetAntennas(lines);
        int maxX = lines.ElementAt(0).Length -1;
        int maxY = lines.Count() - 1;
        return antennas.SelectMany(x => GetAllAntinodesMax(x.Value, maxX, maxY))
                                  .Distinct()
                                  .Count()
                                  .ToString();

    }
    public static List<(int, int)> GetAllAntinodes(List<(int,int)> ants, int maxX, int maxY) {
        List<(int, int)> result = [];
        for(int i = 0; i < ants.Count; i++) {
            for(int j = 0; j < ants.Count; j++) {
                result.AddRange(GetAntinodes(ants[i], ants[j], maxX, maxY));
            }
        }
        return result;
    }

    public static List<(int, int)> GetAntinodes((int x, int y) p1, (int x, int y) p2, int maxX, int maxY) {
        if(p1.x == p2.x && p1.y == p2.y) {
            return [];
        }
        int xDiff = p1.x - p2.x;
        int yDiff = p1.y - p2.y;
        List<(int, int)> results =  [(p1.x + xDiff, p1.y + yDiff), (p2.x - xDiff, p2.y - yDiff)];
        return results.Where((x) => x.Item1 <= maxX && x.Item2 <= maxY && x.Item1 >= 0 && x.Item2 >= 0).ToList();
    }

    public static Dictionary<char, List<(int, int)>> GetAntennas(IEnumerable<string> lines) {
        Dictionary<char, List<(int, int)>> result = [];
        for(int i = 0; i  < lines.Count(); i++) {
            for(int j = 0; j < lines.ElementAt(i).Length; j++) {
                if(lines.ElementAt(i)[j] == '.') {
                    continue;
                }
                if(result.TryGetValue(lines.ElementAt(i)[j], out var list)) {
                    list.Add((j, i));
                } else {
                    result[lines.ElementAt(i)[j]] = [(j, i)];
                }
            }
        }
        return result;
    }

    public static List<(int, int)> GetAllAntinodesMax(List<(int,int)> ants, int maxX, int maxY) {
        List<(int, int)> result = [];
        for(int i = 0; i < ants.Count; i++) {
            for(int j = 0; j < ants.Count; j++) {
                result.AddRange(GetAntinodesMax(ants[i], ants[j], maxX, maxY));
            }
        }
        return result;
    }

    public static List<(int, int)> GetAntinodesMax((int x, int y) p1, (int x, int y) p2, int maxX, int maxY) {
        if(p1.x == p2.x && p1.y == p2.y) {
            return [];
        }
        int xDiff = p1.x - p2.x;
        int yDiff = p1.y - p2.y;
        (int dx, int dy) = MinStep(xDiff, yDiff);
        int step = 0;
        List<(int, int)> results =  [];
        while(InBounds(p1.x + (dx*step), p1.y + (dy*step), maxX, maxY)) {
            results.Add((p1.x + (dx*step), p1.y + (dy*step)));
            step++;
        }
        step = 0;
        while(InBounds(p2.x - (dx*step), p2.y - (dy*step), maxX, maxY)) {
            results.Add((p2.x - (dx*step), p2.y - (dy*step)));
            step++;
        }
        return results;
    }

    public static bool InBounds(int x, int y, int maxX, int maxY) {
        return x <= maxX && y <= maxY && x >= 0 && y >= 0;
    }

    public static (int, int) MinStep(int x, int y) {
        int max = Math.Max(Math.Abs(x), Math.Abs(y));
        for(int i = max; i > 1; i--) {
            if(x % i == 0 && y % i == 0) {
                return (x/i, y/i);
            }
        }
        return (x, y);
    }
}