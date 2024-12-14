using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
public record Robot((int x, int y) startPos, (int dx, int dy) velocity);
public class Day14 : IDay
{
    public int DayNum => 14;

    public string GetExpectedResult(int part)
    {
        if(part == 1) {
            return "228410028";
        }
        return "8258";
    }

    public string Part1(string path)
    {
        List<Robot> robots = GetRobots(File.ReadLines(path));
        List<(int, int)> finalPositions = [];
        foreach(Robot robot in robots) {
            finalPositions.Add(CalculateRobotPosition(robot, 100, 101, 103));
        }
        int topLeftCount = CountQuadrant(finalPositions, 0, 49, 0, 50);
        int topRightCount = CountQuadrant(finalPositions, 51, 100, 0, 50);
        int bottomLeftCount = CountQuadrant(finalPositions, 0, 49, 52, 102);
        int bottomRightCount = CountQuadrant(finalPositions, 51, 100, 52, 102);
        int res = topLeftCount * topRightCount * bottomLeftCount * bottomRightCount;
        return res.ToString();
    }

    public static int CountQuadrant(List<(int, int)> positions, int startX, int endX, int startY, int endY) {
        return positions.Where(x => x.Item1 >= startX && x.Item1 <= endX && x.Item2 >= startY && x.Item2 <= endY).Count();
    }

    public static List<Robot> GetRobots(IEnumerable<string> lines) {
        Regex robotReg = new Regex(@"p=(?<xPos>\d+),(?<yPos>\d+) v=(?<dx>-?\d+),(?<dy>-?\d+)");
        List<Robot> robots = [];
        foreach(var line in lines) {
            var match = robotReg.Match(line);
            (int, int) startPos = (int.Parse(match.Groups["xPos"].Value), int.Parse(match.Groups["yPos"].Value));
            (int, int) vel = (int.Parse(match.Groups["dx"].Value), int.Parse(match.Groups["dy"].Value));
            robots.Add(new Robot(startPos, vel));
        }
        return robots;
    }

    public static (int, int) CalculateRobotPosition(Robot robot, int secs, int xMax, int yMax) {
        int x = (robot.velocity.dx * secs + robot.startPos.x) % xMax;
        x = x < 0 ? xMax + x : x;
        int y = (robot.velocity.dy * secs + robot.startPos.y) % yMax;
        y = y < 0 ? yMax + y : y;
        return (x, y);
    }

    public string Part2(string path)
    {
        List<Robot> robots = GetRobots(File.ReadLines(path));
        int cur = 0;
        while(cur < 10000){
            List<(int, int)> finalPositions = [];
            foreach(Robot robot in robots) {
                finalPositions.Add(CalculateRobotPosition(robot, cur, 101, 103));
            }
            (double x, double y) = GetVariance(finalPositions);
            if(x < 400 && y < 400) {
                //Console.WriteLine($"{cur}:{x},{y}");
                //Console.WriteLine(CurrentFloor(finalPositions, 101, 103));
                break;
            }
            cur++;
        }
        return cur.ToString();
    }

    public static (double, double) GetVariance(List<(int, int)> values) {
        double xAvg = values.Average(x => x.Item1);
        double xVariance = values.Sum(x => (x.Item1 - xAvg)*(x.Item1 - xAvg));
        double yAvg = values.Average(x => x.Item2);
        double yVariance = values.Sum(x => (x.Item2 - yAvg)*(x.Item2 - yAvg));
        return (xVariance/values.Count, yVariance/values.Count);
    }

    public static string CurrentFloor(List<(int, int)> values, int xMax, int yMax) {
        StringBuilder stringBuilder= new();
        for(int i = 0; i < xMax; i++) {
            for(int j = 0; j < yMax; j++) {
                if(values.Contains((i,j))) {
                    stringBuilder.Append('#');
                }
                else {
                    stringBuilder.Append('.');;
                }
            }
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
}