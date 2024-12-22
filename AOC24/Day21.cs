using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using AOCUtil;

namespace AOC24;

public class Day21 : ADay
{
    public override int DayNum => 21;

    public override string Part1(string path)
    {
        IEnumerable<string> lines = File.ReadAllLines(path);
        IEnumerable<List<char>> inputs = lines.Select(x => x.ToList());
        char[,] startKeypad = new char[,]{{'7', '4', '1', ' '}, {'8', '5', '2', '0'}, {'9', '6', '3', 'A'}};
        char[,] dirKeypad = new char[,]{{' ', '<'}, {'^', 'v'}, {'A', '>'}};
        List<IEnumerable<char>> secondStep = [];
        foreach(var input in inputs) {
            secondStep.Add(GetButtonSequence(input, startKeypad));
        }
        List<IEnumerable<char>> thirdStep = [];
        foreach(var second in secondStep) {
            thirdStep.Add(GetButtonSequence(second, dirKeypad));
        }
        List<IEnumerable<char>> lastStep = [];
        foreach(var third in thirdStep) {
            lastStep.Add(GetButtonSequence(third, dirKeypad));
        }
        int sum = 0;
        for(int i = 0; i < lines.Count(); i++) {
            Console.WriteLine("Input was: " + lines.ElementAt(i));
            Console.WriteLine("first step " + string.Join(" ", secondStep[i]));
            Console.WriteLine("second step " + string.Join(" ", thirdStep[i]));
            Console.WriteLine("last step " + string.Join(" ", lastStep[i]));
            Console.WriteLine("number of moves needed is " + lastStep[i].Count());
            sum += int.Parse(lines.ElementAt(i)[0..^1]) * lastStep[i].Count();
        }
        return sum.ToString();
    }
    public IEnumerable<char> GetButtonSequence(IEnumerable<char> input, char[,] keypad) {
        (int x, int y) curPoint = MapHelper.LocatePoint(keypad, 'A');
        List<char> result = [];
        foreach (char c in input) {
            result.AddRange(BestPath(c, keypad, curPoint.x, curPoint.y));
            curPoint = MapHelper.LocatePoint(keypad, c);
        }

        return result;
    }

    public IEnumerable<char> GetButtonSequenceFirst(IEnumerable<char> input, char[,] keypad) {
        (int x, int y) curPoint = MapHelper.LocatePoint(keypad, 'A');
        List<char> result = [];
        foreach (char c in input) {
            result.AddRange(BestPathFirst2(c, keypad, curPoint.x, curPoint.y));
            curPoint = MapHelper.LocatePoint(keypad, c);
        }
        return result;
    }

    public static IEnumerable<char> BestPath(char nextPoint, char[,] keypad, int curX, int curY) {
        if(keypad[curX, curY] == nextPoint) {
            return ['A'];
        }
        (int nextX, int nextY) = MapHelper.LocatePoint(keypad, nextPoint);
        int horz = nextX - curX;
        int vert = nextY - curY;
        List<char> result;
        if (keypad[curX, nextY] != ' ' && vert > 0)
        {
            result = vertFirst(horz, vert);
        }
        else if(keypad[nextX, curY] != ' ')
        {
            result = horzFirst(horz, vert);
        } else {
            result = vertFirst(horz, vert);
        }
        result.Add('A');
        return result;
    }

    public static List<char> horzFirst(int horz, int vert) {
        List<char> result = [];
        for(int i = 0; i < Math.Abs(horz); i++)
        {
            result.Add(horz < 0 ? '<' :'>');
        }
        for(int i = 0; i < Math.Abs(vert); i++)
        {
            result.Add(vert < 0 ? '^' :'v');
        }
        return result;
    }

    public static List<char> vertFirst(int horz, int vert) {
        List<char> result = [];
        for(int i = 0; i < Math.Abs(vert); i++)
        {
            result.Add(vert < 0 ? '^' :'v');
        }
        for(int i = 0; i < Math.Abs(horz); i++)
        {
            result.Add(horz < 0 ? '<' :'>');
        }
        return result;
    }
    public static IEnumerable<char> BestPathFirst(char nextPoint, char[,] keypad, int curX, int curY) {
        if(keypad[curX, curY] == nextPoint) {
            return ['A'];
        }
        List<char> result = [];
        (int nextX, int nextY) = MapHelper.LocatePoint(keypad, nextPoint);
        //Console.WriteLine($"trying to reach {nextPoint}, it is at{nextX}, {nextY}");
        if(curX > nextX) {
            for(int i = 0; i < (curX - nextX); i++)
            {
                result.Add('<');
            }
        }
        if(curX < nextX) {
            for(int i = 0; i < (nextX - curX); i++)
            {
                result.Add('>');
            }
        }
        if(curY > nextY) {
            for(int i = 0; i < (curY - nextY); i++)
            {
                result.Add('^');
            }
        }
        result.Add('A');
        return result;

    }

        public static IEnumerable<char> BestPathFirst2(char nextPoint, char[,] keypad, int curX, int curY) {
        if(keypad[curX, curY] == nextPoint) {
            return ['A'];
        }
        List<char> result = [];
        (int nextX, int nextY) = MapHelper.LocatePoint(keypad, nextPoint);
        //Console.WriteLine($"trying to reach {nextPoint}, it is at{nextX}, {nextY}");
        if(curX < nextX) {
            for(int i = 0; i < (nextX - curX); i++)
            {
                result.Add('>');
            }
        }
        if(curY > nextY) {
            for(int i = 0; i < (curY - nextY); i++)
            {
                result.Add('^');
            }
        }
        if(curX > nextX) {
            for(int i = 0; i < (curX - nextX); i++)
            {
                result.Add('<');
            }
        }
        if(curY < nextY) {
            for(int i = 0; i < (nextY - curY); i++)
            {
                result.Add('v');
            }
        }
        result.Add('A');
        return result;

    }

    //TODO: currently giving too high results past 2
    public override string Part2(string path)
    {
        IEnumerable<string> lines = File.ReadAllLines(path);
        IEnumerable<IEnumerable<char>> inputs = lines.Select(x => x.ToList());
        char[,] startKeypad = new char[,]{{'7', '4', '1', ' '}, {'8', '5', '2', '0'}, {'9', '6', '3', 'A'}};
        char[,] dirKeypad = new char[,]{{' ', '<'}, {'^', 'v'}, {'A', '>'}};
        List<Dictionary<string, long>> inital = lines.Select(x => new Dictionary<string, long>(){{x, 1}}).ToList();
        List<Dictionary<string, long>> prevStep = [];
        Dictionary<string, string> vals = [];
        foreach(var line in inital) {
            prevStep.Add(GetButtonSequenceP2(line, startKeypad));
        }
        vals = [];
        for(int i = 0; i < 25; i++) {
            List<Dictionary<string, long>> curStep = [];
            foreach(var input in prevStep) {
                var cur = GetButtonSequenceP2(input, dirKeypad);
                curStep.Add(cur);
            }
            prevStep = curStep;
        }
        long sum = 0;
        for(int i = 0; i < lines.Count(); i++) {
            Console.WriteLine("Input was: " + lines.ElementAt(i));
            Console.WriteLine("final step " + string.Join(" ", prevStep[i]));
            Console.WriteLine("number of moves needed is " + prevStep[i].Sum(x => x.Key.Length * x.Value));
            sum += long.Parse(lines.ElementAt(i)[0..^1]) * prevStep[i].Sum(x => x.Key.Length * x.Value);
        }
        return sum.ToString();
    }

    public Dictionary<string, long> GetButtonSequenceP2(Dictionary<string, long> input, char[,] keypad) {
        (int x, int y) curPoint = MapHelper.LocatePoint(keypad, 'A');
        Dictionary<string, long> result = new();
        foreach ((string s, long count) in input) {
            IEnumerable<string> breaks = s.Split('A').SkipLast(1);
            foreach(string st in breaks) {
                string toUse = st + 'A';
                if(result.TryGetValue(toUse, out var cur)) {
                    result[toUse] = cur + count;
                } else {
                    string current = "";
                    foreach(char c in toUse) {
                        current += string.Join("", BestPath(c, keypad, curPoint.x, curPoint.y));
                        curPoint = MapHelper.LocatePoint(keypad, c);
                    }
                    if(result.TryGetValue(current, out cur)) {
                        result[current] = cur + count;
                    } else {
                        result.Add(current, count);
                    }
                }
            }
        }
        return result;
    }
}