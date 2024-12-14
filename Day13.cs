using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

public record Machine (
    (int x, int y) AButton,
    (int x, int y) BButton,
    (int x, int y) Prize);
public class Day13 : IDay
{
    public int DayNum => 13;

    public string GetExpectedResult(int part)
    {
        if(part == 1) {
            return "34393";
        }
        return "34393";
    }

    public string Part1(string path)
    {
        List<Machine> machines = GetMachines(File.ReadAllText(path));
        long sum = 0;
        foreach(Machine machine in machines) {
            int cur = TokensToWin(machine);
            sum += cur;
        }

        return sum.ToString();
    }

    public static int TokensToWin(Machine machine) {
        if(machine.AButton.x * 100 + machine.BButton.x * 100 < machine.Prize.x ||
           machine.AButton.y * 100 + machine.BButton.y * 100 < machine.Prize.y) {
            return 0;
        }
        int bestResult = int.MaxValue;
        for(int i = 100; i > 0; i--) {
            for(int j = 100; j > 0; j--) {
                if(machine.BButton.x * i + machine.AButton.x * j == machine.Prize.x &&
                   machine.BButton.y * i + machine.AButton.y * j == machine.Prize.y) {
                    int curResult = i + j*3;
                    if(curResult < bestResult) {
                        bestResult = curResult;
                    }
                }
            }
        }
        return bestResult == int.MaxValue ? 0 : bestResult;
    }

    public static List<Machine> GetMachines(string inputText) {
        Regex aButtons = new Regex(@"^Button A: X\+(?<aX>\d+), Y\+(?<aY>\d+)\r?$", RegexOptions.Multiline);
        Regex bButtons = new Regex(@"^Button B: X\+(?<aX>\d+), Y\+(?<aY>\d+)\r?$", RegexOptions.Multiline);
        Regex prizes = new Regex(@"^Prize: X=(?<aX>\d+), Y=(?<aY>\d+)\r?$", RegexOptions.Multiline);
        var amatches = aButtons.Matches(inputText);
        var bmatches = bButtons.Matches(inputText);
        var pmatches = prizes.Matches(inputText);
        List<Machine> machines = [];
        for(int i = 0; i < amatches.Count; i++) {
            (int Ax, int Ay) curA = (int.Parse(amatches[i].Groups["aX"].Value), int.Parse(amatches[i].Groups["aY"].Value));
            (int Ax, int Ay) curB = (int.Parse(bmatches[i].Groups["aX"].Value), int.Parse(bmatches[i].Groups["aY"].Value));
            (int Ax, int Ay) curC = (int.Parse(pmatches[i].Groups["aX"].Value), int.Parse(pmatches[i].Groups["aY"].Value));
            machines.Add(new Machine(curA, curB, curC));
        }
        return machines;
    }

    public string Part2(string path)
    {
        List<Machine> machines = GetMachines(File.ReadAllText(path));
        long sum = 0;
        foreach(Machine machine in machines) {
            long cur = TokensCramer(machine, 10000000000000);
            sum += cur;
        }

        return sum.ToString();
    }


    public static long TokensCramer(Machine machine, long offset) {
        (long, long) prize = (machine.Prize.x + offset, machine.Prize.y + offset);
        long detMain = (machine.AButton.x * machine.BButton.y) - (machine.BButton.x * machine.AButton.y);
        long detA = (prize.Item1 * machine.BButton.y) - ((long)machine.BButton.x * prize.Item2);
        long detB = (prize.Item2 * machine.AButton.x) - ((long)machine.AButton.y * prize.Item1);

        long x = detA/detMain;
        long y = detB/detMain;
        if(x * machine.AButton.x + y * machine.BButton.x == prize.Item1 &&
           x * machine.AButton.y + y * machine.BButton.y == prize.Item2) {
            return 3*x +  y;
        }
        //Console.WriteLine($"Found result: A: {x} B: {y}");
        return 0;
    }
}