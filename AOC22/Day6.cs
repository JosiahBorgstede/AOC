namespace AOC22;
public class Day6 {
    public static void Part1(string path) {
        string lines = File.ReadAllText(path);
        Queue<char> curChars = new Queue<char>();
        int i = 0;
        for (; i < lines.Length; i++) {
            curChars.Enqueue(lines[i]);
            if(curChars.Count == 5) {
                curChars.Dequeue();
                if(curChars.Distinct().Count() == 4) {
                    break;
                }
            }
        }
        Console.WriteLine(i);
    }

    public static void Part2(string path) {
        string lines = File.ReadAllText(path);
        Queue<char> curChars = new Queue<char>();
        int i = 0;
        for (; i < lines.Length; i++) {
            curChars.Enqueue(lines[i]);
            if(curChars.Count == 15) {
                curChars.Dequeue();
                if(curChars.Distinct().Count() == 14) {
                    break;
                }
            }
        }
        Console.WriteLine(i);
    }
}