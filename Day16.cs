using System.Net.Mail;
using System.Reflection.Metadata;

public class Day16 : IDay
{
    public int DayNum => 16;

    public (int x, int y) StartPos = (0, 0);
    public (int x, int y) EndPos = (0, 0);

    public int[,]? mapRes;

    public string GetExpectedResult(int part)
    {
        if(part == 1) {
            return "not done";
        }
        return "not done";
    }

    public string Part1(string path)
    {
        char[,] map = MakeMap(File.ReadLines(path));
        mapRes = MakeResultMap(map);
        List<(int, int)> visted = [];
        int res = BestPathCost(map, visted, StartPos.x, StartPos.y, StartPos.x-1, StartPos.y);
        return res.ToString();
    }

    public static void DrawMap(char[,] map) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                Console.Write(map[j,i]);
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

    public int[,] MakeResultMap(char[,] map) {
        int[,] resMap = new int[map.GetLength(0), map.GetLength(1)];
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                resMap[j,i] = int.MaxValue;
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

    public int BestPathCost(char[,] map, IEnumerable<(int, int)> visited, int curX, int curY, int prevX, int prevY)
    {
        if(map[curX, curY] == 'E') {
            return 0;
        }
        List<(int, int)> potentialNextPoints = [(curX + 1, curY),
                                                (curX - 1, curY),
                                                (curX, curY - 1),
                                                (curX, curY + 1)];
        potentialNextPoints.Remove((prevX, prevY));
        potentialNextPoints.RemoveAll(p => map[p.Item1, p.Item2] == '#');
        potentialNextPoints.RemoveAll(visited.Contains);
        if(potentialNextPoints.Count == 0) {
            return int.MaxValue;
        }
        IEnumerable<(int, int)> newvisited = visited.Append((curX, curY));
        (int nextX, int nextY) nextPos = (2*curX - prevX, 2 * curY - prevY);
        List<int> cur = potentialNextPoints.ConvertAll(p => {
                int res = BestPathCost(map, newvisited, p.Item1, p.Item2, curX, curY);
                if(res == int.MaxValue) {
                    return res;
                }
                if(p == nextPos) {
                    return  res + 1;
                }
                return res + 1001;});
        return cur.Min();
    }

    public string Part2(string path)
    {
        throw new NotImplementedException();
    }
}