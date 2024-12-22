namespace AOC24;
using AOCUtil;

public class Day20 : ADay
{
    public override int DayNum => 20;

    public override string Part1(string path)
    {
        char[,] map = MapHelper.ConvertTextToMap(File.ReadAllLines(path));
        int[,] distMap = GetMinDistances(map, 'S');
        int[,] distMapEnd = GetMinDistances(map, 'E');
        //MapHelper.DrawMap(distMap, HowToDraw);
        var end = LocatePoint(map, 'E');
        int[,] distPath = FindPath(distMapEnd, distMap, distMap[end.Item1, end.Item2]);
        //CheatsOverLenght(distMap, 100);
        
        return CheatsOverLenght(distMap, 100).ToString();
    }

    public static string HowToDraw(int val) => val switch {
        int.MaxValue => "#".PadRight(4),
        _ => val.ToString().PadRight(4),
    };

    public int[,] GetMinDistances(char[,] map, char startChar) {
        int xMax = map.GetLength(0);
        int yMax = map.GetLength(1);
        int[,] distMap = Map<int>.MakeSimpleMap(xMax, yMax, int.MaxValue);
        bool[,] visited = Map<bool>.MakeSimpleMap(xMax, yMax, false);
        var start = LocatePoint(map, startChar);
        (int, int) toCheck = start;
        distMap[start.Item1,start.Item2] = 0;
        do{
            (int curX, int curY) = toCheck;
            int curDist = distMap[curX, curY];
            visited[curX, curY] = true;
            List<(int, int)> potentialPoints = [(curX - 1, curY),
                                                (curX + 1, curY),
                                                (curX, curY - 1),
                                                (curX, curY + 1)];
            potentialPoints.RemoveAll(p => MapHelper.OutOfBounds(p.Item1, p.Item2, xMax, yMax));
            potentialPoints.RemoveAll(p => map[p.Item1, p.Item2] == '#');
            foreach((int x, int y) in potentialPoints) {
                distMap[x,y] = Math.Min(curDist + 1, distMap[x,y]);
            }
            toCheck = NextCheck(distMap, visited);
        } while(toCheck != (0,0));
        return distMap;
    }

    public (int, int) LocatePoint(char[,] map, char toFind) {
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i,j] == toFind) {
                    return (i, j);
                }
            }
        }
        return (0,0);
    }

    public int[,] FindPath(int[,] distFromEnd, int[,] distFromStart, int totalDist) {
        int[,] path = new int[distFromEnd.GetLength(0), distFromEnd.GetLength(1)];
        for(int i = 0; i < distFromEnd.GetLength(0); i++) {
            for(int j = 0; j < distFromEnd.GetLength(1); j++) {
                if(distFromEnd[i,j] + distFromStart[i,j] == totalDist) {
                    path[i,j] = distFromStart[i,j];
                }
                else {
                    path[i,j] = int.MaxValue;
                }
            }
        }
        return path;
    }

    public int CheatsOverLenght(int[,] distMap, int requiredTimeSave) {
        int sum = 0;
        for(int i = 0; i < distMap.GetLength(0); i++) {
            for(int j = 0; j < distMap.GetLength(1); j++) {
                if(distMap[i,j] != int.MaxValue) {
                    sum += CountGoodCheats(distMap, i, j, requiredTimeSave);
                }
            }
        }
        return sum;
    }

    public IEnumerable<(int, int, int)> PointsWithinDistance(int x, int y, int maxX, int maxY, int dist) {
        List<(int, int, int)> potentialPoints = [];
        for(int i = 0; i <= dist; i++) {
            for(int j = 0; j <= dist - i; j++) {
                if((i == 0 && j == 1) || (i == 1 && j == 0) || (i == 0 && j == 0)) {
                    continue;
                }
                potentialPoints.Add((x + i, y + j, i+j));
                potentialPoints.Add((x - i, y + j, i+j));
                potentialPoints.Add((x + i, y - j, i+j));
                potentialPoints.Add((x - i, y - j, i+j));
            }
        }
        potentialPoints.RemoveAll(p => MapHelper.OutOfBounds(p.Item1, p.Item2, maxX, maxY));
        return potentialPoints.Distinct();
    }

    public int CountGoodCheats(int[,] distMap, int curX, int curY, int minimumSaved) {
        List<(int, int)> potentialPoints = [(curX - 2, curY),
                                            (curX + 2, curY),
                                            (curX, curY - 2),
                                            (curX, curY + 2)];
        potentialPoints.RemoveAll(p => MapHelper.OutOfBounds(p.Item1, p.Item2, distMap.GetLength(0), distMap.GetLength(1)));
        potentialPoints.RemoveAll(p => distMap[p.Item1, p.Item2] == int.MaxValue);
        return potentialPoints.Select(p => distMap[p.Item1, p.Item2] - distMap[curX, curY]).Where(x => x - 2 >= minimumSaved).Count();
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

    public override string Part2(string path)
    {
        char[,] map = MapHelper.ConvertTextToMap(File.ReadAllLines(path));
        int[,] distMap = GetMinDistances(map, 'S');
        return CheatsOverLenghtFurther(distMap, 100, 20).ToString();;
    }

    public int CheatsOverLenghtFurther(int[,] distMap, int requiredTimeSave, int maxCheat) {
        int sum = 0;
        for(int i = 0; i < distMap.GetLength(0); i++) {
            for(int j = 0; j < distMap.GetLength(1); j++) {
                if(distMap[i,j] != int.MaxValue) {
                    sum += CountGoodCheatsFurther(distMap, i, j, requiredTimeSave, maxCheat);
                }
            }
        }
        return sum;
    }

    public int CountGoodCheatsFurther(int[,] distMap, int curX, int curY, int minimumSaved, int maxDist) {
        List<(int, int, int)> potentialPoints = PointsWithinDistance(curX, curY, distMap.GetLength(0), distMap.GetLength(1), maxDist).ToList();
        potentialPoints.RemoveAll(p => distMap[p.Item1, p.Item2] == int.MaxValue);
        return potentialPoints.Select(p => (distMap[p.Item1, p.Item2] - distMap[curX, curY], p.Item3)).Where(x => x.Item1 - x.Item2 >= minimumSaved).Count();
    }
}