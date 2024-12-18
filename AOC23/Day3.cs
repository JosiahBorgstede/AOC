namespace AOC23;

using System.Text.RegularExpressions;

public class Day3 {

    public static int SumOfEngineNumbers(string path)
    {
        List<string> lines = File.ReadAllLines(path).ToList();
        Regex regex_numbers= new Regex(@"\d{2,3}");
        string emptyline = new string('.', lines[0].Length);
        lines.Add(emptyline);
        lines.Insert(0, emptyline);
        int sum = 0;
        for(int i = 1; i <= lines.Count - 1; i++) {
            int starIdx = lines[i].IndexOf('*');
            if(starIdx != -1) {
                int.TryParse(lines[i].Substring(starIdx-3, 3), out int val1);
            }
            //regex.Matches(line).ToList().ForEach(match => Console.WriteLine(match.Value));
            var nums = from numbers in regex_numbers.Matches(lines[i])
                       where hasSurroundingStar(numbers, lines[i], lines[i-1], lines[i+1])
                       select numbers;
            if(nums.Count() > 0) {
                sum += nums.Select(number => int.Parse(number.Value)).Aggregate((a, b) => a + b);
            }
        }
        return sum;
    }

    public static bool hasSurrounding(Match val, string cur, string prev, string next) {
        int startIdx = val.Index == 0 ? 0 : val.Index-1;
        int length = val.Index+val.Length >= cur.Length ? val.Length +1 : val.Length + 2;
        char prevChar = val.Index > 0 ? cur[val.Index-1] : '.';
        char nextChar = val.Index+val.Length >= cur.Length ? '.' : cur[val.Index + val.Length];
        //Console.WriteLine(@"Searching at ${cur}, " + prev.Substring(startIdx, length).Any(charc => charc != '.'));
        return prevChar != '.' || nextChar != '.' ||
               prev.Substring(startIdx, length).Any(charc => charc != '.') ||
               next.Substring(startIdx, length).Any(charc => charc != '.');
    }

    public static bool hasSurroundingStar(Match val, string cur, string prev, string next) {
        int startIdx = val.Index == 0 ? 0 : val.Index-1;
        int length = val.Index+val.Length >= cur.Length ? val.Length +1 : val.Length + 2;
        char prevChar = val.Index > 0 ? cur[val.Index-1] : '.';
        char nextChar = val.Index+val.Length >= cur.Length ? '.' : cur[val.Index + val.Length];
        //Console.WriteLine(@"Searching at ${cur}, " + prev.Substring(startIdx, length).Any(charc => charc != '.'));
        return prevChar == '*' || nextChar == '*' ||
               prev.Substring(startIdx, length).Any(charc => charc == '*') ||
               next.Substring(startIdx, length).Any(charc => charc == '*');
    }
}