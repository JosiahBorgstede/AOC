using System.Xml.XPath;

public class Day1 {
    public static int RunDay1(string path) {
        string input = File.ReadAllText(path);
        return input.Count(c => c == '(') - input.Count(c => c == ')');
    }

    public static int RunDay1Part2(string path) {
        string input = File.ReadAllText(path);
        int floor = 0;
        int val = 0;
        for(int i = 0; i < input.Length; i++) {
            val++;
            if(input[i] == '(') {
                floor++;
            }
            else{
                floor--;
            }
            if(floor < 0) {
                return val;
            }
        }
        return 0;
    }
}