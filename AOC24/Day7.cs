namespace AOC24;

public class Day7 : ADay {
    public override int DayNum => 7;

    public Day7() : base() {
        _part1Versions.Add("fold", new("fold", "part 1 using folding, which is probably what most people did", Part1NormalFold));
        _part2Versions.Add("unfold", new("unfold", "part 2 starting from the result, which shoul be faster", Part2Faster));
    }


    public override string Part1(string path) {
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

    public static string Part1NormalFold(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<(long, IEnumerable<long>)> values = ExtractValues(lines);
        long sum = 0;
        foreach((var value, var nums) in values) {
            if(CheckNumberFold(value, nums)) {
                sum += value;
            }
        }
        return sum.ToString();
    }

    public static bool CheckNumberFold(decimal result, IEnumerable<long> nums)
    {
        if(nums.Count() == 1) {
            return result == nums.ElementAt(0);
        }
        return CheckNumberFold(result, [nums.ElementAt(0) + nums.ElementAt(1), ..nums.Skip(2)]) ||
               CheckNumberFold(result, [nums.ElementAt(0) * nums.ElementAt(1), ..nums.Skip(2)]);
    }

    public static bool CheckNumberPossible(decimal result, IEnumerable<long> nums)
    {
        if(nums.Count() == 1) {
            return result == nums.ElementAt(0);
        }
        return (result - nums.ElementAt(0) > 0 && CheckNumberPossible(result - nums.ElementAt(0), nums.Skip(1))) ||
               (result % nums.ElementAt(0) == 0 && CheckNumberPossible(result / nums.ElementAt(0), nums.Skip(1)));
    }
    public override string Part2(string path) {
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

    public string Part2Faster(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        IEnumerable<(long, IEnumerable<long>)> values = ExtractValues(lines);
        long sum = 0;
        foreach((var value, var nums) in values) {
            if(CheckNumberPossiblePart2(value, nums.Reverse())) {
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
        return (result - nums.ElementAt(0) > 0 && CheckNumberPossiblePart2(result - nums.ElementAt(0), nums.Skip(1))) ||
               (result % nums.ElementAt(0) == 0 && CheckNumberPossiblePart2(result / nums.ElementAt(0), nums.Skip(1))) ||
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
        return CheckNumberPossiblePart2Normal(result, [nums.ElementAt(0) + nums.ElementAt(1), ..nums.Skip(2)]) ||
               CheckNumberPossiblePart2Normal(result, [nums.ElementAt(0) * nums.ElementAt(1), ..nums.Skip(2)]) ||
               CheckNumberPossiblePart2Normal(result, [ConcatNumbers(nums.ElementAt(0), nums.ElementAt(1)), ..nums.Skip(2)]);
    }
}