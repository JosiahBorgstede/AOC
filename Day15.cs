using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

public class Day15 : IDay
{
    public int DayNum => 15;

    public string GetExpectedResult(int part)
    {
        if (part == 1) {
            return "not done";
        }
        return "not done";
    }

    public string Part1(string path)
    {
        IEnumerable<string> lines = File.ReadLines(path);
        char[,] map = MakeMap(lines.TakeWhile(x => x.Length > 0));
        Queue<char> movements = GetMovements(lines.SkipWhile(x => x.Length > 0).Skip(1));
        DrawMap(map);
        while(movements.Count > 0) {
            (int botX, int botY) = DetermineBotPos(map);
            map = MakeMovement(botX, botY, map, movements.Dequeue());
        }
        DrawMap(map);
        return ValueOfMap(map).ToString();
    }

    public static void DrawMap(char[,] map) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                Console.Write(map[j,i]);
            }
            Console.WriteLine();
        }
    }
    public static (int, int) DetermineBotPos(char[,] map) {
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i,j] == '@') {
                    return (i,j);
                }
            }
        }
        return (0,0);
    }

    public static int ValueOfMap(char[,] map) {
        int sum = 0;
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i,j] == 'O') {
                    sum += i + 100 *j;
                }
            }
        }
        return sum;
    }

    public static int ValueOfMapWide(char[,] map) {
        int sum = 0;
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i,j] == '[') {
                    sum += i + 100 * j;
                }
            }
        }
        return sum;
    }

    public static char[,] MakeMap(IEnumerable<string> lines) {
        char[,] map = new char[lines.First().Length,lines.Count()];
        for(int i = 0; i < lines.Count(); i++) {
            for(int j = 0; j < lines.ElementAt(i).Length; j++) {
                map[j,i] = lines.ElementAt(i)[j];
            }
        }
        return map;
    }

    public static char[,] MakeMapWide(IEnumerable<string> lines) {
        char[,] map = new char[lines.First().Length*2,lines.Count()];
        for(int i = 0; i < lines.Count(); i++) {
            for(int j = 0; j < lines.ElementAt(i).Length; j++) {
                switch (lines.ElementAt(i)[j]) {
                    case 'O':
                        map[2*j, i] = '[';
                        map[2*j+1, i] = ']';
                        break;
                    case '.':
                        map[2*j, i] = '.';
                        map[2*j+1, i] = '.';
                        break;
                    case '@':
                        map[2*j, i] = '@';
                        map[2*j+1, i] = '.';
                        break;
                    case '#':
                        map[2*j, i] = '#';
                        map[2*j+1, i] = '#';
                        break;
                }
            }
        }
        return map;
    }

    public static Queue<char> GetMovements(IEnumerable<string> lines)
    {
        Queue<char> movements = new();
        foreach(string line in lines) {
            foreach(char ch in line) {
                movements.Enqueue(ch);
            }
        }
        return movements;
    }

    public static char[,] MakeMovement(int botX, int botY, char[,] map, char dir) {
        var move = GetDirection(dir);
        (int x, int y) curPos = (botX, botY);
        while(map[curPos.x, curPos.y] != '.' &&
              map[curPos.x, curPos.y] != '#') {
                curPos = move(curPos);
        }
        if(map[curPos.x, curPos.y] == '#')
        {
            return map;
        }
        var newBot = move((botX, botY));
        map[newBot.Item1, newBot.Item2] = '@';
        if(curPos != newBot) {
            map[curPos.x, curPos.y] = 'O';
        }
        map[botX, botY] = '.';
        return map;
    }

    public static bool VerticalMovePossible(List<int> curCols, int curY, char[,] map, char dir) {
        var move = GetDirection(dir);
        (int nextX, int nextY) = move((curCols[0], curY));
        List<int> nextCols = [];
        if(nextY < 0 || nextY > map.GetLength(1)) {
            return false;
        }
        foreach(int col in curCols) {
            if(map[col, nextY] == '#') {
                return false;
            }
            if(map[col, nextY] == '[') {
                nextCols.Add(col + 1);
                nextCols.Add(col);
            }
            if(map[col, curY] == ']') {
                nextCols.Add(col - 1);
                nextCols.Add(col);
            }
        }
        if(nextCols.Count == 0) {
            return true;
        }
        return VerticalMovePossible(nextCols, nextY, map, dir);
    }

    public static char[,] MakeMovementVertical(int botX, int botY, char[,] map, char dir) {
        var move = GetDirection(dir);
        Queue<(int, int)> toCheck = [];
        toCheck.Enqueue((botX, botY));
        List<(int, int)> toMove = [];
        while(toCheck.Count > 0) {
            var Cur = toCheck.Dequeue();
            var nextPos = move(Cur);
            if(map[nextPos.Item1, nextPos.Item2] == '[')
            {
                toCheck.Enqueue(nextPos);
                toCheck.Enqueue((nextPos.Item1 + 1, nextPos.Item2));
            }
            else if(map[nextPos.Item1, nextPos.Item2] == ']')
            {
                toCheck.Enqueue(nextPos);
                toCheck.Enqueue((nextPos.Item1 - 1, nextPos.Item2));
            }
            else if(map[nextPos.Item1, nextPos.Item2] == '#') {
                return map;
            }
            toMove.Add(Cur);
        }
        char[,] mapCopy = new char[map.GetLength(0), map.GetLength(1)];
        Array.Copy(map, mapCopy, map.Length);
        List<(int, int)> movedInto = [];
        foreach(var cur in toMove) {
            var newPos = move(cur);
            mapCopy[newPos.Item1, newPos.Item2] = map[cur.Item1, cur.Item2];
            movedInto.Add(newPos);
            if(!movedInto.Contains(cur)) {
                mapCopy[cur.Item1, cur.Item2] = '.';
            }
        }
        return mapCopy;
    }

    public static char[,] MakeMovementHorizontal(int botX, int botY, char[,] map, char dir) {
        var move = GetDirection(dir);
        (int x, int y) curPos = (botX, botY);
        Queue<char> passed = [];
        while(map[curPos.x, curPos.y] != '.' &&
              map[curPos.x, curPos.y] != '#') {
                passed.Enqueue(map[curPos.x, curPos.y]);
                curPos = move(curPos);
        }
        if(map[curPos.x, curPos.y] == '#'){
            return map;
        }
        curPos = (botX, botY);
        while(passed.Count > 0) {
            curPos = move(curPos);
            map[curPos.x, curPos.y] = passed.Dequeue();
        }
        map[botX, botY] = '.';
        return map;
    }

    public static Func<(int, int), (int, int)> GetDirection(char dr) => dr switch {
        '<' => ((int x, int y) p) => {return (p.x-1, p.y);},
        '>' => ((int x, int y) p) => {return (p.x+1, p.y);},
        '^' => ((int x, int y) p) => {return (p.x, p.y-1);},
        'v' => ((int x, int y) p) => {return (p.x, p.y+1);},
        _ => throw new NotImplementedException(),
    };

    public string Part2(string path)
    {
        IEnumerable<string> lines = File.ReadLines(path);
        char[,] map = MakeMapWide(lines.TakeWhile(x => x.Length > 0));
        Queue<char> movements = GetMovements(lines.SkipWhile(x => x.Length > 0).Skip(1));
        DrawMap(map);
        while(movements.Count > 0) {
            (int botX, int botY) = DetermineBotPos(map);
            char next = movements.Dequeue();
            if(next == '<' || next == '>') {
                map = MakeMovementHorizontal(botX, botY, map, next);
            } else {
                map = MakeMovementVertical(botX, botY, map, next);
            }
        }
        DrawMap(map);
        return ValueOfMapWide(map).ToString();
    }
}