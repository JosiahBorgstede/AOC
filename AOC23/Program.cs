namespace AOC23;

public record Game {
    int gameNum {get; set;}


}
public class MainClass {
    public static void Main(string[] args){
        Console.WriteLine(Day3.SumOfEngineNumbers("../Inputs/Day3.txt"));
    }

    //     List<string> lines = File.ReadAllLines("./aocDay2.txt").ToList();
    //     Regex gameNum= new Regex("Game (?<gameNum>\\d+):");
    //     Regex gamePulls = new Regex(@"(?:(?<amount>\d+)\s(?<color>red|green|blue),?(?:\s|;)){1,3}");
    //     Regex redVal = new Regex(@"(?<amount>\d+)\sred");
    //     Regex greenVal = new Regex(@"(?<amount>\d+)\sgreen");
    //     Regex blueVal = new Regex(@"(?<amount>\d+)\sblue");
    //     int sum = 0;
    //     foreach(string line in lines){
    //         int gameNumber = int.Parse(gameNum.Match(line).Groups["gameNum"].Value);
    //         var redMatches = redVal.Matches(line).MaxBy(match => int.Parse(match.Groups["amount"].Value));
    //         var greenMatches = greenVal.Matches(line).MaxBy(match => int.Parse(match.Groups["amount"].Value));
    //         var blueMatches = blueVal.Matches(line).MaxBy(match => int.Parse(match.Groups["amount"].Value));
    //         sum += int.Parse(redMatches.Groups["amount"].Value) * int.Parse(greenMatches.Groups["amount"].Value) * int.Parse(blueMatches.Groups["amount"].Value);
    //     }
    //     Console.WriteLine(sum);
    //     //lines.ConvertAll(line => Regex.Match(line, "Game (?<gameNum>\\d+)")).ForEach(match => Console.WriteLine(match.Result("${gameNum}")));
    //     // Regex regex = new Regex("one|two|three|four|five|six|seven|eight|nine|\\d");
    //     // Regex reverse = new Regex("one|two|three|four|five|six|seven|eight|nine|\\d", RegexOptions.RightToLeft);
    //     // var firstmatches = lines.ConvertAll(regex.Match);
    //     // var lastmatches = lines.ConvertAll(reverse.Match);
    //     // firstmatches.Zip(lastmatches).ToList().ForEach(i => Console.WriteLine(i.First.ToString() + i.Second.ToString()));
    //     // int result = lines.ConvertAll(line => (regex.Match(line), reverse.Match(line)))
    //     //                   .ConvertAll(col => int.Parse(ConvertToIntString(col.Item1.ToString()) + ConvertToIntString(col.Item2.ToString())))
    //     //                   .Aggregate((a, b) => a + b);

    //     // Console.WriteLine(result);
    // }

    // public static string ConvertToIntString(string val) => val switch
    // {
    //         "one" => "1",
    //         "two" => "2",
    //         "three" => "3",
    //         "four" => "4",
    //         "five" => "5",
    //         "six" => "6",
    //         "seven" => "7",
    //         "eight" => "8",
    //         "nine" => "9",
    //         "zero" => "0",
    //         _ => val,
    // };
}



