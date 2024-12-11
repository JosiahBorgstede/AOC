public interface IDay {

    int DayNum {get;}
    public string Part1(string path);
    public string Part2(string path);

    public string GetExpectedResult(int part);
}