namespace AOC24;

public class Day4 : IDay {
    public int DayNum => 4;

    public string GetExpectedResult(int part) {
        if (part == 1) {
            return "2633";
        }
        return "1936";
    }

    public string Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        int LineLength = lines.First().Length;
        char[,] input = new char[lines.First().Length+6, lines.Count()+6];
        for (int i = 0; i < lines.Count(); i++) {
            for (int j = 0; j < lines.ElementAt(i).Length; j++) {
                input[i+3, j+3] = lines.ElementAt(i)[j];
            }
        }
        int sum = 0;
        for (int i = 0; i < input.GetLength(0); i++) {
            for(int j = 0; j < input.GetLength(1); j++) {
                sum += SearchAround(input, i, j);
            }
        }
        return sum.ToString();
    }

    public static int SearchAround (char[,] input, int x, int y) {
        if (input[x,y] != 'X') {
            return 0;
        }
        int found = 0;
        if(input[x-1,y] == 'M' && input[x-2,y] == 'A' && input[x-3,y] == 'S') {
            found++;
        }
        if(input[x+1,y] == 'M' && input[x+2,y] == 'A' && input[x+3,y] == 'S') {
            found++;
        }
        if(input[x+1,y+1] == 'M' && input[x+2,y+2] == 'A' && input[x+3,y+3] == 'S') {
            found++;
        }
        if(input[x-1,y+1] == 'M' && input[x-2,y+2] == 'A' && input[x-3,y+3] == 'S') {
            found++;
        }
        if(input[x-1,y-1] == 'M' && input[x-2,y-2] == 'A' && input[x-3,y-3] == 'S') {
            found++;
        }
        if(input[x+1,y-1] == 'M' && input[x+2,y-2] == 'A' && input[x+3,y-3] == 'S') {
            found++;
        }
        if(input[x,y-1] == 'M' && input[x,y-2] == 'A' && input[x,y-3] == 'S') {
            found++;
        }
        if(input[x,y+1] == 'M' && input[x,y+2] == 'A' && input[x,y+3] == 'S') {
            found++;
        }
        return found;
    }

    public string Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        int LineLength = lines.First().Length;
        char[,] input = new char[lines.First().Length+6, lines.Count()+6];
        for (int i = 0; i < lines.Count(); i++) {
            for (int j = 0; j < lines.ElementAt(i).Length; j++) {
                input[i+3, j+3] = lines.ElementAt(i)[j];
            }
        }
        int sum = 0;
        for (int i = 0; i < input.GetLength(0); i++) {
            for(int j = 0; j < input.GetLength(1); j++) {
                sum += xSearchAround(input, i, j);
            }
        }
        return sum.ToString();
    }

    public static int xSearchAround(char[,] input, int x, int y) {
        if(input[x,y] != 'A')
        {
            return 0;
        }
        int masCount = 0;
        if(input[x-1,y-1] == 'M' && input[x+1,y+1] == 'S') {
            masCount++;
        }
        if(input[x-1,y+1] == 'M' && input[x+1,y-1] == 'S') {
            masCount++;
        }
        if(input[x+1,y+1] == 'M' && input[x-1,y-1] == 'S') {
            masCount++;
        }
        if(input[x+1,y-1] == 'M' && input[x-1,y+1] == 'S') {
            masCount++;
        }
        if(masCount == 2) {
            return 1;
        }
        return 0;
    }
}