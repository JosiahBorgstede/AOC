namespace AOC24;

public sealed class Day12 : ADay {
    public override int DayNum => 12;


    public override string Part1(string path)
    {
        char[,] map = GetMap(File.ReadAllLines(path));
        List<List<(int, int)>> regions = GetRegions(map);
        int sum = 0;
        foreach (var region in regions) {
            sum += GetPerimeter(region) * GetArea(region);
        }
        return sum.ToString();
    }

    public static char[,] GetMap(IEnumerable<string> lines) {
        char[,] map = new char[lines.First().Length,lines.Count()];
        for(int i = 0; i < lines.Count(); i++) {
            for(int j = 0; j < lines.ElementAt(i).Length; j++) {
                map[j,i] = lines.ElementAt(i)[j];
            }
        }
        return map;
    }

    public static List<List<(int, int)>> GetRegions(char[,] map) {
        List<List<(int, int)>> regions = [];
        bool[][] searched = new bool[map.GetLength(0)][];
        for(int i = 0; i < searched.Length; i++) {
            searched[i] = new bool[map.GetLength(1)];
        }
        Queue<(int, int)> currentAdjs = new Queue<(int, int)>();
        while(searched.Any(s => s.Any(x => !x))) {
            currentAdjs.Enqueue(FirstFalse(searched));
            List<(int, int)> curRegion = [];
            while(currentAdjs.Any()) {
                (int curX, int curY) curPoint= currentAdjs.Dequeue();
                searched[curPoint.curX][curPoint.curY] = true;
                curRegion.Add(curPoint);
                GetAdjacents(map, curPoint).Where(((int x, int y) val) => !searched[val.x][val.y] && !currentAdjs.Contains(val))
                                           .ToList()
                                           .ForEach(val => currentAdjs.Enqueue(val));
            }
            regions.Add(curRegion);
        }
        return regions;
    }

    public static (int, int) FirstFalse(bool[][] bools) {
        for(int i = 0; i < bools.Length; i++) {
            for(int j = 0; j < bools[i].Length; j++) {
                if(!bools[i][j]) {
                    return (i, j);
                }
            }
        }
        return (-1, -1);
    }

    public static int CountNonAdjecents(IEnumerable<(int, int)> region, (int x, int y) p1) {
        return  (region.Contains((p1.x - 1, p1.y)) ? 0 : 1) +
                (region.Contains((p1.x + 1, p1.y)) ? 0 : 1) +
                (region.Contains((p1.x, p1.y - 1)) ? 0 : 1) +
                (region.Contains((p1.x, p1.y + 1)) ? 0 : 1);
    }

    public static int GetPerimeter(List<(int, int)> region) {
        return region.Select(x => CountNonAdjecents(region, x)).Sum();

    }

    public static List<(int, int)> GetAdjacents(char[,] map, (int x, int y) p) {
        List<(int, int)> adjs = [(p.x-1, p.y), (p.x+1, p.y), (p.x, p.y-1), (p.x, p.y+1)];
        return adjs.Where(val => InBounds(map, val.Item1, val.Item2) && map[val.Item1, val.Item2] == map[p.x,p.y]).ToList();
    }

    public static bool InBounds(char[,] map, int x, int y) {
        return x >= 0 && y >= 0 && x < map.GetLength(0) && y < map.GetLength(1);
    }

    public static int GetArea(List<(int, int)> region) {
        return region.Count;
    }

    public static List<(int, int)> GetRegionAdjs(IEnumerable<(int, int)> region, (int x, int y)p) {
        return [..region.Where(x => x == (p.x - 1, p.y)),
                ..region.Where(x => x == (p.x + 1, p.y)),
                ..region.Where(x => x == (p.x, p.y - 1)),
                ..region.Where(x => x == (p.x, p.y + 1))];
    }

    public static List<(int, int)> GetRegionDiags(IEnumerable<(int, int)> region, (int x, int y)p) {
        return [..region.Where(x => x == (p.x - 1, p.y - 1)),
                ..region.Where(x => x == (p.x + 1, p.y - 1)),
                ..region.Where(x => x == (p.x - 1, p.y + 1)),
                ..region.Where(x => x == (p.x + 1, p.y + 1))];
    }

    public static int GetSides(List<(int, int)> region) {
        int sum = 0;
        foreach(var p in region) {
            var adjs = GetRegionAdjs(region, p);
            if(adjs.Count == 0) {
                sum += 4;
            } else if(adjs.Count == 1) {
                sum += 2;
            } else if (adjs.Count == 2) {
                if(adjs[0].Item1 == adjs[1].Item1 || adjs[0].Item2 == adjs[1].Item2) {
                    sum += 0;
                } else {
                    int xDiff = adjs[0].Item1 - adjs[1].Item1;
                    int yDiff = adjs[0].Item2 - adjs[1].Item2;
                    var corner = GetRegionDiags(region, p).Intersect([(adjs[0].Item1 - xDiff, adjs[0].Item2), (adjs[0].Item1, adjs[0].Item2 -yDiff)]);
                    if(corner.Count() == 0) {
                        sum += 1;
                    }
                    sum += 1;
                }
            } else if (adjs.Count == 3) {
                if(adjs.Select(x => x.Item1).Distinct().Count() == 3) {
                    int diffY = adjs.Select(x => x.Item2).Where(x => x != p.Item2).First();
                    sum += 2 - GetRegionDiags(region, p).Where(x => x.Item2 == diffY).Count();
                } else {
                    int diffX = adjs.Select(x => x.Item1).Where(x => x != p.Item1).First();
                    sum += 2 - GetRegionDiags(region, p).Where(x => x.Item1 == diffX).Count();
                }
            } else if (adjs.Count == 4) {
                sum += 4 - GetRegionDiags(region, p).Count;
            }
        }
        return sum;
    }


    public override string Part2(string path)
    {
        char[,] map = GetMap(File.ReadAllLines(path));
        List<List<(int, int)>> regions = GetRegions(map);
        int sum = 0;
        foreach (var region in regions) {
            sum += GetSides(region) * GetArea(region);
        }
        return sum.ToString();
    }
}