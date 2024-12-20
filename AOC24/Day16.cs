namespace AOC24;

public sealed record MazePosition(int vertScore, int horzScore);
public sealed class Day16 : ADay
{
    public override int DayNum => 16;

    public (int x, int y) StartPos = (0, 0);
    public (int x, int y) EndPos = (0, 0);


    public override string Part1(string path)
    {
        char[,] map = MakeMap(File.ReadLines(path));
        var values = MakeResultMap(map);
        bool changed = true;
        while(changed) {
            values = updateMap(map, values, out changed);
        }
        var res = values[EndPos.x, EndPos.y];
        return Math.Min(res.vertScore, res.horzScore).ToString();
    }


    public static bool MapFilled(char[,] map, MazePosition[,] vals) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                if(map[j, i] != '#' && vals[j,i].horzScore == int.MaxValue && vals[j,i].vertScore == int.MaxValue) {
                    return false;
                }
            }
        }
        return true;
    }

    public static void DrawMap(char[,] map) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                Console.Write(map[j,i]);
            }
            Console.WriteLine();
        }
    }

    public static void DrawMapResults(MazePosition[,] map, char[,] mapChar) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                if(mapChar[j,i] == '#') {
                    Console.Write("#".PadRight(6));
                } else {
                    Console.Write(map[j,i].vertScore.ToString().PadRight(6));
                }
            }
            Console.WriteLine();
        }
    }

    public char[,] MakeMap(IEnumerable<string> lines) {
        char[,] map = new char[lines.First().Length,lines.Count()];
        for(int i = 0; i < lines.Count(); i++) {
            for(int j = 0; j < lines.ElementAt(i).Length; j++) {
                map[j,i] = lines.ElementAt(i)[j];
                if(map[j,i] == 'S') {
                    StartPos = (j, i);
                }
                if(map[j,i] == 'E') {
                    EndPos = (j, i);
                }
            }
        }
        return map;
    }

    public MazePosition[,] MakeResultMap(char[,] map) {
        MazePosition[,] resMap = new MazePosition[map.GetLength(0), map.GetLength(1)];
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                resMap[j,i] = new(int.MaxValue, int.MaxValue);
            }
        }
        return resMap;
    }

    public bool[,] MakeVisitedMap(char[,] map) {
        bool[,] resMap = new bool[map.GetLength(0), map.GetLength(1)];
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                resMap[j,i] = false;
            }
        }
        return resMap;
    }

    public MazePosition[,] updateMap(char[,] map, MazePosition[,] values, out bool changed)
    {
        changed = false;
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                var cur = values[i,j];
                values[i,j] = CheckPoint(map, values, i, j);
                if(cur != values[i,j]) {
                    changed = true;
                }
            }
        }
        return values;
    }

    public MazePosition CheckPoint(char[,] map, MazePosition[,] vals, int curX, int curY) {
        if(map[curX, curY] == 'S') {
            return new MazePosition(1000, 0);
        }
        if(map[curX, curY] == '#') {
            return new MazePosition(int.MaxValue, int.MaxValue);
        }
        List<int> potentialHorzVals = [vals[curX, curY].horzScore];
        List<int> potentialVertVals = [vals[curX, curY].vertScore];
        if(vals[curX - 1, curY].horzScore != int.MaxValue) {
            potentialHorzVals.Add(vals[curX - 1, curY].horzScore + 1);
        }
        if(vals[curX - 1, curY].vertScore != int.MaxValue) {
            potentialHorzVals.Add(vals[curX - 1, curY].vertScore + 1001);
        }
        if(vals[curX + 1, curY].horzScore != int.MaxValue) {
            potentialHorzVals.Add(vals[curX + 1, curY].horzScore + 1);
        }
        if(vals[curX + 1, curY].vertScore != int.MaxValue) {
            potentialHorzVals.Add(vals[curX + 1, curY].vertScore + 1001);
        }
        if(vals[curX, curY - 1].horzScore != int.MaxValue) {
            potentialVertVals.Add(vals[curX, curY - 1].horzScore + 1001);
        }
        if(vals[curX, curY - 1].vertScore != int.MaxValue) {
            potentialVertVals.Add(vals[curX, curY - 1].vertScore + 1);
        }
        if(vals[curX, curY + 1].horzScore != int.MaxValue) {
            potentialVertVals.Add(vals[curX, curY + 1].horzScore + 1001);
        }
        if(vals[curX, curY + 1].vertScore != int.MaxValue) {
            potentialVertVals.Add(vals[curX, curY + 1].vertScore + 1);
        }
        int minHorz = potentialHorzVals.Min();
        int minVert = potentialVertVals.Min();
        return new MazePosition(minVert, minHorz);
    }

    public override string Part2(string path)
    {
        char[,] map = MakeMap(File.ReadLines(path));
        var values = MakeResultMap(map);
        bool changed = true;
        while(changed) {
            values = updateMap(map, values, out changed);
        }
        IEnumerable<(int, int)> bestSpots = DetermineBestSpots(values, map, EndPos.x, EndPos.y).Distinct();
        //DrawMapBestPath(values, map, bestSpots);
        //DrawMapResults(values, map);
        //DrawMapBestPath(values, map, bestSpots);
        return bestSpots.Count().ToString();
    }

    public static List<(int, int)> DetermineBestSpots(MazePosition[,] values, char[,] map, int curX, int curY) {
        Queue<(int, int)> toCheck = [];
        toCheck.Enqueue((curX, curY));
        List<(int, int)> inPath = [];
        while(toCheck.Count > 0) {
            var cur = toCheck.Dequeue();
            var curNeigh = getBestNeighbors(values, map, cur.Item1, cur.Item2);
                inPath.Add(cur);
            foreach(var neigh in curNeigh) {
                if(!inPath.Contains(neigh)) {
                    toCheck.Enqueue(neigh);
                }
            }
        }
        return inPath;
    }

    public static IEnumerable<(int, int)> getBestNeighbors(MazePosition[,] values, char[,] map, int curX, int curY) {
        if(map[curX, curY] == 'S') {
            return [];
        }
        List<(int, int)> potentialPoints = [(curX - 1, curY),
                                            (curX + 1, curY),
                                            (curX, curY - 1),
                                            (curX, curY + 1)];
        int curBestScore = Math.Min(values[curX,curY].vertScore, values[curX,curY].horzScore);
        potentialPoints.RemoveAll(p => map[p.Item1, p.Item2] == '#');
        if(potentialPoints.Count <= 2) {
            return [potentialPoints.MinBy(p => Math.Min(values[p.Item1, p.Item2].horzScore, values[p.Item1, p.Item2].vertScore))];
        }
        var sameX = potentialPoints.Where(p => p.Item1 == curX);
        var sameY = potentialPoints.Where(p => p.Item2 == curY);
        List<(int, int)> toAdd = [];
        if(sameX.Count() == 2) {
            var max = sameX.Max(p => values[p.Item1, p.Item2].vertScore);
            var min = sameX.Min(p => values[p.Item1, p.Item2].vertScore);
            if(max - min == 2) {
                toAdd.Add(sameX.MinBy(p => values[p.Item1, p.Item2].vertScore));
            }
        }
        if(sameX.Count() == 1) {
            var p = sameX.First();
            if(values[p.Item1, p.Item2].vertScore == values[curX, curY].vertScore - 1) {
                toAdd.Add(p);
            }
        }
        if(sameY.Count() == 2) {
            var max = sameY.Max(p => values[p.Item1, p.Item2].horzScore);
            var min = sameY.Min(p => values[p.Item1, p.Item2].horzScore);
            if(max - min == 2) {
                toAdd.Add(sameY.MinBy(p => values[p.Item1, p.Item2].horzScore));
            }
        }
        if(sameY.Count() == 1) {
            var p = sameY.First();
            if(values[p.Item1, p.Item2].horzScore == values[curX, curY].horzScore - 1) {
                toAdd.Add(p);
            }
        }
        return toAdd;
    }

    public static void DrawMapBestPath(MazePosition[,] map, char[,] mapChar, IEnumerable<(int, int)> bests) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                if(mapChar[j,i] == '#') {
                    Console.Write("#");
                } else if(bests.Contains((j,i))){
                    Console.Write("1");
                } else {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }
}