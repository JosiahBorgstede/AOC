using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

public record Registers(long A, long B, long C);
public class Day17 : IDay
{
    public int DayNum => 17;

    public string GetExpectedResult(int part)
    {
        if(part == 1) {
            return "4,1,5,3,1,5,3,5,7";
        }
        return "164542125272765";
    }

    public string Part1(string path)
    {
        List<string> lines = File.ReadLines(path).ToList();
        int[] regs = new int[3];
        for(int i = 0; i < 3; i++) {
            string[] vals = lines[i].Split(": ");
            regs[i] = int.Parse(vals[1]);
        }
        Registers registers = new(regs[0], regs[1], regs[2]);
        List<long> program = lines[4].Split(": ")[1].Split(",").Select(long.Parse).ToList();
        List<long> outputs = RunProgram(program, registers);
        StringBuilder res = new();
        foreach(var output in outputs) {
            res.Append(output);
            res.Append(',');
        }
        return res.ToString().TrimEnd(',');
    }

    public List<long> RunProgram(List<long> program, Registers registers, bool JmpEneabled = true) {
        List<long> outputs = [];
        Registers curRegs = registers;
        for(int i = 0; i < program.Count; i +=2) {
            long opcode = program[i];
            long operand = program[i+1];
            switch (opcode) {
                case 0:
                    int denom = (int) Math.Pow(2, GetComboOperand(operand, curRegs));
                    long num = curRegs.A;
                    curRegs = curRegs with {A = num/denom};
                    break;
                case 1:
                    long curB = curRegs.B;
                    curRegs = curRegs with {B = curB ^ operand};
                    break;
                case 2:
                    long val = GetComboOperand(operand, curRegs);
                    curRegs = curRegs with {B = val % 8};
                    break;
                case 3:
                    if(curRegs.A != 0 && JmpEneabled) {
                        i = (int)operand - 2;
                    }
                    break;
                case 4:
                    long currentB = curRegs.B;
                    long currentC = curRegs.C;
                    curRegs = curRegs with {B = currentB ^ currentC};
                    break;
                case 5:
                    long curVal = GetComboOperand(operand, curRegs);
                    outputs.Add(curVal % 8);
                    break;
                case 6:
                    denom = (int) Math.Pow(2, GetComboOperand(operand, curRegs));
                    num = curRegs.A;
                    curRegs = curRegs with {B = num/denom};
                    break;
                case 7:
                    denom = (int) Math.Pow(2, GetComboOperand(operand, curRegs));
                    num = curRegs.A;
                    curRegs = curRegs with {C = num/denom};
                    break;
            }
        }
        return outputs;
    }

    public long GetComboOperand(long operand, Registers regs) => operand switch
    {
        1 => 1,
        2 => 2,
        3 => 3,
        4 => regs.A,
        5 => regs.B,
        6 => regs.C,
        _ => throw new Exception("unknown operand"),
    };

    public string Part2(string path)
    {
        List<string> lines = File.ReadLines(path).ToList();
        int[] regs = new int[3];
        for(int i = 0; i < 3; i++) {
            string[] vals = lines[i].Split(": ");
            regs[i] = int.Parse(vals[1]);
        }
        Registers registers = new(regs[0], regs[1], regs[2]);
        List<long> program = lines[4].Split(": ")[1].Split(",").Select(long.Parse).ToList();
        registers = registers with {A = (long)Math.Pow(8, 15)};
        List<long> outputs = RunProgram(program, registers);
        writeOutput(outputs);
        for(int j = program.Count - 1; j >= 0; j--) {
            if(j == 2) {
                break;
            }
            while(outputs[j] != program[j]){
                long curA = registers.A;
                registers = registers with {A = curA + (long)Math.Pow(8, j-1)};
                Console.WriteLine("value of A:" + registers.A);
                outputs = RunProgram(program, registers);
                writeOutput(outputs);
            }
        }
        while(!outputs.SequenceEqual(program)) {
            long curA = registers.A;
            registers = registers with {A = curA + 1};
            Console.WriteLine("value of A:" + registers.A);
            outputs = RunProgram(program, registers);
            writeOutput(outputs);
        }

        return registers.A.ToString();
    }

    public void writeOutput(List<long> outputs) {
        StringBuilder res = new();
        foreach(var output in outputs) {
            res.Append(output);
            res.Append(',');
        }
        Console.WriteLine(res.ToString().TrimEnd(','));
    }
}