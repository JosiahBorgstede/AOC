namespace AOC24;

public class Day10 : IDay {
    public int DayNum => 10;

    public string GetExpectedResult(int part) {
        if (part == 1) {
            return "459";
        }
        return "1034";
    }

    public string Part1(string path)
    {
        int[,] map = GetMap(File.ReadLines(path));
        int sum = 0;
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i, j] == 0) {
                    sum += NinesReached(i, j, map, 0).Distinct().Count();
                }

            }
        }
        return sum.ToString();
    }

    public static int[,] GetMap(IEnumerable<string> lines) {
        int[,] map = new int[lines.First().Length,lines.Count()];
        for(int i = 0; i < lines.Count(); i++) {
            for(int j = 0; j<lines.ElementAt(i).Length; j++) {
                map[j,i] = int.Parse([lines.ElementAt(i)[j]]);
            }
        }
        return map;
    }

    public List<(int, int)> NinesReached(int x, int y, int[,] map, int searchVal) {
        if(x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) {
            return [];
        }
        if(map[x,y] != searchVal) {
            return [];
        }
        if(map[x,y] == 9) {
            return [(x, y)];
        }
        return  [..NinesReached(x + 1, y, map, searchVal + 1),
                 ..NinesReached(x - 1, y, map, searchVal + 1),
                 ..NinesReached(x, y + 1, map, searchVal + 1),
                 ..NinesReached(x, y - 1, map, searchVal + 1)];
    }

    public string Part2(string path)
    {
        int[,] map = GetMap(File.ReadLines(path));
        int sum = 0;
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i, j] == 0) {
                    sum += NinesReached(i, j, map, 0).Count();
                }

            }
        }
        return sum.ToString();
    }

    
}