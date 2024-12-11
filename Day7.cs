
public class Day7 : IDay {
    public int DayNum => 7;

    public string GetExpectedResult(int part) {
        if (part == 1) {
            return "14711933466277";
        }
        return "286580387663654";
    }

    public string Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<(long, IEnumerable<long>)> values = ExtractValues(lines);
        long sum = 0;
        foreach((var value, var nums) in values) {
            if(CheckNumberPossible(value, nums.Reverse())) {
                sum += value;
            }
        }
        return sum.ToString();
    }

    private static IEnumerable<(long, IEnumerable<long>)> ExtractValues(IEnumerable<string> lines)
    {
        List<(long, IEnumerable<long>)> result = [];
        foreach(string line in lines) {
            string[] vals = line.Split(": ");
            result.Add((long.Parse(vals[0]), vals[1].Split(' ').Select(long.Parse)));
        }
        return result;
    }

    public static bool CheckNumberPossible(decimal result, IEnumerable<long> nums)
    {
        if(nums.Count() == 1) {
            return result == nums.ElementAt(0);
        }
        return CheckNumberPossible(result - nums.ElementAt(0), nums.Skip(1)) ||
               CheckNumberPossible(result / nums.ElementAt(0), nums.Skip(1));
    }
    public string Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<(long, IEnumerable<long>)> values = ExtractValues(lines);
        long sum = 0;
        foreach((var value, var nums) in values) {
            if(CheckNumberPossiblePart2Normal(value, nums)) {
                sum += value;
            }
        }
        return sum.ToString();
    }

    public static bool CheckNumberPossiblePart2(decimal result, IEnumerable<long> nums)
    {
        if(nums.Count() == 1) {
            return result == nums.ElementAt(0);
        }
        if(nums.Count() == 2 && ConcatNumbers(nums.ElementAt(0), nums.ElementAt(1)) == result) {
            return true;
        }
        return CheckNumberPossiblePart2(result - nums.ElementAt(0), nums.Skip(1)) ||
               CheckNumberPossiblePart2(result / nums.ElementAt(0), nums.Skip(1)) ||
               (result.ToString().EndsWith(nums.First().ToString()) && CheckNumberPossiblePart2((result - nums.First()) / (10 ^ nums.First().ToString().Length), nums.Skip(1)));
    }

    public static long ConcatNumbers(long num1, long num2) {
        return Convert.ToInt64(string.Format("{0}{1}", num1, num2));
    }

    public static bool CheckNumberPossiblePart2Normal(decimal result, IEnumerable<long> nums)
    {
        if(nums.Count() == 1) {
            return result == nums.ElementAt(0);
        }
        IEnumerable<long> concat = [ConcatNumbers(nums.ElementAt(0), nums.ElementAt(1)), ..nums.Skip(2)];
        IEnumerable<long> add = [nums.ElementAt(0) + nums.ElementAt(1), ..nums.Skip(2)];
        IEnumerable<long> mult = [nums.ElementAt(0) * nums.ElementAt(1), ..nums.Skip(2)];
        return CheckNumberPossiblePart2Normal(result, add) ||
               CheckNumberPossiblePart2Normal(result, mult) ||
               CheckNumberPossiblePart2Normal(result, concat);
    }
}