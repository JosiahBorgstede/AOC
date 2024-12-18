namespace AOC22;
public class Day3 {
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        int sum = 0;
        foreach (string line in lines) {
            string part1 = line[..(line.Length / 2)];
            string part2 = line[(line.Length / 2)..];
            char same = ' ';
            foreach(char ch in part1) {
                if(part2.Contains(ch)) {
                    same = ch;
                    break;
                }
            }
            if((same - 'a') < 0) {
                sum += same - 'A' + 27;
                Console.WriteLine(same + " value was " + (same - 'A' + 27));
            } else {
                sum += same - 'a' + 1;
                Console.WriteLine(same + " value was " + (same - 'a' + 1));
            }
            //sum += 'a' - same + 1;
        }
        Console.WriteLine(sum);
    }

    public static void Part2(string path) {
        List<string> lines = File.ReadLines(path).ToList();
        int sum = 0;
        for(int i = 0; i < lines.Count(); i+=3) {
            string part1 = lines[i];
            string part2 = lines[i+1];
            string part3 = lines[i+2];
            char same = ' ';
            foreach (char ch in part1) {
                if(part2.Contains(ch) && part3.Contains(ch)) {
                    same = ch;break;
                }
            }
            sum += valueOfChar(same);
        }
        Console.WriteLine(sum);
    }

    public static int valueOfChar(char ch) {
        if((ch - 'a') < 0) {
            return ch - 'A' + 27;
            
        } else {
            return ch - 'a' + 1;
        }
    }
}