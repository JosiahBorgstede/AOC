using System.Collections.Immutable;
using AOC24;
public record FunctionVersion(string name, string description, Func<string, string> runnable);
public abstract class ADay : IDay
{
    public abstract int DayNum {get;}

    protected Dictionary<string, FunctionVersion> _part1Versions = [];
    public ImmutableList<FunctionVersion> Part1Versions {get {return _part1Versions.Values.ToImmutableList();}}
    protected Dictionary<string, FunctionVersion> _part2Versions = [];
    public ImmutableList<FunctionVersion> Part2Versions {get {return _part2Versions.Values.ToImmutableList();}}

    public ADay() {
        _part1Versions.Add("base", new("base", "Standard version for part 1", Part1));
        _part2Versions.Add("base", new("base", "Standard version for part 2", Part2));
    }

    public Func<string,string> this[int part] {
        get {
            if(part == 1) {
                return Part1;
            } else if(part == 2) {
                return Part2;
            }
            throw new ArgumentException("invalid part");
        }
    }

    public Func<string,string> this[int part, string type] {
        get {
            if(part == 1) {
                return _part1Versions[type].runnable;
            } else if(part == 2) {
                return _part2Versions[type].runnable;
            }
            throw new ArgumentException("invalid part");
        }
    }

    public virtual string GetExpectedResult(int part, string path) {
        if(part == 1) {
            return _part1Versions["base"].runnable(path);
        }
        else if(part == 2) {
            return _part2Versions["base"].runnable(path);
        }
        throw new ArgumentException($"not a valid part: {part}");
    }

    public abstract string Part1(string path);

    public abstract string Part2(string path);
}