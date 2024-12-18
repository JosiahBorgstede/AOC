namespace AOC24;

using AOCUtil;
public class Day18 : IDay
{
    public int DayNum => 18;

    public string GetExpectedResult(int part)
    {
        if(part == 1) {
            return "304";
        }
        return "50,28";
    }

    public string Part1(string path)
    {
        IEnumerable<string> lines = File.ReadAllLines(path);
        IEnumerable<string> firstKilo = lines.Take(1024);
        IEnumerable<(int, int)> points = GetPoints(firstKilo);
        int[,] minDist = GetMinDistances(points, 71, 71);
        MapHelper.DrawMap<int>(minDist, HowToDraw);
        return minDist[70, 70].ToString();
    }

    public static string HowToDraw(int val) => val switch {
        int.MaxValue => "#".PadRight(4),
        _ => val.ToString().PadRight(4),
    };

    public int[,] GetMinDistances(IEnumerable<(int, int)> corrupted, int xMax, int yMax) {
        int[,] distMap = Map<int>.MakeSimpleMap(xMax, yMax, int.MaxValue);
        bool[,] visited = Map<bool>.MakeSimpleMap(xMax, yMax, false);
        (int, int) toCheck = (0,0);
        distMap[0,0] = 0;
        do{
            (int curX, int curY) = toCheck;
            int curDist = distMap[curX, curY];
            visited[curX, curY] = true;
            List<(int, int)> potentialPoints = [(curX - 1, curY),
                                                (curX + 1, curY),
                                                (curX, curY - 1),
                                                (curX, curY + 1)];
            potentialPoints.RemoveAll(p => corrupted.Contains(p));
            potentialPoints.RemoveAll(p => OutOfBounds(p.Item1, p.Item2, xMax, yMax));
            foreach((int x, int y) in potentialPoints) {
                distMap[x,y] = Math.Min(curDist + 1, distMap[x,y]);
            }
            toCheck = NextCheck(distMap, visited);
        } while(toCheck != (0,0));
        return distMap;
    }

    public (int, int) NextCheck(int[,] dists, bool[,] visited) {
        (int x, int y) bestVal = (0,0);
        int bestDist = int.MaxValue;
        for(int i = 0; i < dists.GetLength(0); i++) {
            for(int j = 0; j < dists.GetLength(1); j++) {
                if(visited[i,j]) {
                    continue;
                }
                if(dists[i,j] < bestDist) {
                    bestVal = (i,j);
                    bestDist = dists[i,j];
                }
            }
        }
        return bestVal;
    }

    public bool OutOfBounds(int x, int y, int maxX, int maxY) {
        return x < 0 || y < 0 || x >= maxX || y >= maxY;
    }

    public IEnumerable<(int, int)> GetPoints(IEnumerable<string> lines) {
        return lines.Select(s => s.Split(','))
                    .Select(arr => (int.Parse(arr[0]), int.Parse(arr[1])));
    }

    public string Part2(string path)
    {
        IEnumerable<string> lines = File.ReadAllLines(path);
        IEnumerable<string> firstKilo = lines.Take(1024);
        IEnumerable<(int, int)> points = GetPoints(firstKilo);
        int toAdd = 1024;
        while(StartConnectedToEnd(points, 71, 71)) {
            toAdd++;
            firstKilo = lines.Take(toAdd);
            points = GetPoints(firstKilo);
            Console.WriteLine(toAdd);
        }
        string badPoint = lines.ElementAt(toAdd - 1);
        return badPoint;
    }

    public bool StartConnectedToEnd(IEnumerable<(int, int)> corrupted, int maxX, int maxY) {
        bool[,] visited = Map<bool>.MakeSimpleMap(maxX, maxY, false);
        Queue<(int, int)> toCheck = [];
        toCheck.Enqueue((0,0));
        visited[0,0] = true;
        while(toCheck.Count > 0){
            (int curX, int curY) = toCheck.Dequeue();
            List<(int, int)> potentialPoints = [(curX - 1, curY),
                                                (curX + 1, curY),
                                                (curX, curY - 1),
                                                (curX, curY + 1)];
            potentialPoints.RemoveAll(p => corrupted.Contains(p));
            potentialPoints.RemoveAll(p => OutOfBounds(p.Item1, p.Item2, maxX, maxY));
            foreach((int x, int y) in potentialPoints) {
                if(x == maxX - 1 && y == maxY - 1) {
                    return true;
                }
                if(!visited[x,y]) {
                    visited[x,y] = true;
                    toCheck.Enqueue((x,y));
                }
            }
        }
        return visited[maxX -1, maxY - 1];
    }
}