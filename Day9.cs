public class Day9 : IDay {

    public void Run(string part, string path) {
        if(part == "1") {
            Part1(path);
        } else {
            Part2(path);
        }
    }
    public void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
    }

    public void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
    }
}