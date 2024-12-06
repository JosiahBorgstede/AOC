public class Day6 {

    public static void Run(string part, string path) {
        if(part == "1") {
            Part1(path);
        } else {
            Part2(path);
        }
    }

    public static void Part1(string path) {
        string[] lines = File.ReadAllLines(path);
        int[,] result = mapRoute(lines);
        int sum = 0;
        for(int i = 0; i < result.GetLength(0); i++) {
            for(int j = 0; j < result.GetLength(1); j++) {
                if(result[i, j] > 0) {
                    sum++;
                }
            }
        }
        Console.WriteLine(sum);
    }

    public static (int, int) getStartPos(string[] lines) {
        for(int i = 0; i < lines.Length; i++) {
            for( int j = 0; j < lines[i].Length; j++) {
                if(lines[i][j] == '^')
                {
                    Console.WriteLine($"Start Position is ({i},{j})");
                    return (j, i);
                }
            }
        }
        return (0, 0);
    }

    public static int[,] mapRoute (string[] lines) {
        int mx = 0;
        int my = -1;
        (int curX, int curY) = getStartPos(lines);
        int[,] visited = new int[lines[0].Length,lines.Length];
        while(true) {
            visited[curX, curY] = DirectionFlag(visited[curX, curY], mx, my);
            (int nextX, int nextY) = (curX + mx, curY + my);
            if(!InBounds(lines, nextX, nextY)) {
                Console.WriteLine($"Out of bounds at {nextX} {nextY}");
                break;
            }
            if(visited[curX,curY] == 15) {
                Console.WriteLine("looped");
                break;
            }
            if(lines[nextY][nextX] == '#') {
                (mx, my) = rotate(mx, my);
            } else {
                curX = nextX;
                curY = nextY;
            }
        }
        return visited;
    }

    public static int DirectionFlag(int cur, int mx, int my) {
        if(my == -1) {
            return cur | 1;
        } else if (my == 1) {
            return cur | 2;
        } else if (mx == -1) {
            return cur | 4;
        } else if (mx == 1) {
            return cur | 8;
        }
        return cur;
    }

    public static (int, int) rotate(int x, int y) {
        if(y == -1) {
            return (1, 0);
        } else if (x == 1) {
            return (0, 1);
        } else if (y == 1) {
            return (-1, 0);
        } else if (x == -1) {
            return (0, -1);
        }
        return (1, 0);
    }
    public static bool InBounds(string[] lines, int x, int y) {
        return x > -1 && y > -1 && x < lines[0].Length && y < lines.Length;
    }

    public static bool checkLoop (string[] lines, int xAdd, int yAdd) {
        int mx = 0;
        int my = -1;
        (int curX, int curY) = getStartPos(lines);
        int[,] visited = new int[lines[0].Length,lines.Length];
        while(true) {
            (int nextX, int nextY) = (curX + mx, curY + my);
            if(!InBounds(lines, nextX, nextY)) {
                return false;
            }
            if(visited[curX,curY] > 5) {
                return true;
            }
            if(lines[nextY][nextX] == '#' || (nextX,nextY) == (xAdd, yAdd)) {
                (mx, my) = rotate(mx, my);
            } else {
                visited[curX, curY]++;
                curX = nextX;
                curY = nextY;
            }
        }
    }

    public static void Part2(string path) {
        string[] lines = File.ReadAllLines(path);
        int sum = 0;
        for(int i = 0; i < lines.Length; i++) {
            for(int j = 0; j < lines[0].Length; j++) {
                if(checkLoop(lines, i, j)) {
                    sum++;
                }
            }
        }
        Console.WriteLine(sum);
    }
}