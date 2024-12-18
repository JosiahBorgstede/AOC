using System.Collections.Immutable;

namespace AOC24;

public interface IDay {

    int DayNum {get;}

    Func<string,string> this[int part] {get;}

    Func<string,string> this[int part, string type] {get;}

    public ImmutableList<FunctionVersion> Part1Versions {get;}

    public ImmutableList<FunctionVersion> Part2Versions {get;}

    public string Part1(string path);
    public string Part2(string path);
    public string GetExpectedResult(int part, string path);
}