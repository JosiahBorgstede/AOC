namespace AOC24;

using System.Text;

public record Registers(long A, long B, long C);
public sealed class Day17 : ADay
{
    public override int DayNum => 17;

    public Day17() : base() {
        _part2Versions.Add("old", new("old", "the older version of part 2 that uses brute force and is based on my personal input, therefore not scalable", Part2Old));
    }

    public override string Part1(string path)
    {
        List<string> lines = File.ReadLines(path).ToList();
        int[] regs = new int[3];
        for(int i = 0; i < 3; i++) {
            string[] vals = lines[i].Split(": ");
            regs[i] = int.Parse(vals[1]);
        }
        Registers registers = new(regs[0], regs[1], regs[2]);
        List<int> program = lines[4].Split(": ")[1].Split(",").Select(int.Parse).ToList();
        List<long> outputs = RunProgram(program, registers);
        StringBuilder res = new();
        foreach(var output in outputs) {
            res.Append(output);
            res.Append(',');
        }
        return res.ToString().TrimEnd(',');
    }

    public List<long> RunProgram(List<int> program, Registers registers) {
        List<long> outputs = [];
        Registers curRegs = registers;
        for(int i = 0; i < program.Count; i +=2) {
            long opcode = program[i];
            int operand = program[i+1];
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
                    if(curRegs.A != 0) {
                        i = operand - 2;
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

    public string Part2Old(string path)
    {
        List<string> lines = File.ReadLines(path).ToList();
        int[] regs = new int[3];
        for(int i = 0; i < 3; i++) {
            string[] vals = lines[i].Split(": ");
            regs[i] = int.Parse(vals[1]);
        }
        Registers registers = new(regs[0], regs[1], regs[2]);
        List<int> program = lines[4].Split(": ")[1].Split(",").Select(int.Parse).ToList();
        registers = registers with {A = (long)Math.Pow(8, 15)};
        List<long> outputs = RunProgram(program, registers);
        //writeOutput(outputs);
        for(int j = program.Count - 1; j >= 0; j--) {
            if(j == 2) {
                break;
            }
            while(outputs[j] != program[j]){
                long curA = registers.A;
                registers = registers with {A = curA + (long)Math.Pow(8, j-1)};
                Console.WriteLine("value of A:" + registers.A);
                outputs = RunProgram(program, registers);
                //writeOutput(outputs);
            }
        }
        while(!outputs.SequenceEqual(program.ConvertAll<long>(x => x))) {
            long curA = registers.A;
            registers = registers with {A = curA + 1};
            Console.WriteLine("value of A:" + registers.A);
            outputs = RunProgram(program, registers);
            writeOutput(outputs);
        }

        return registers.A.ToString();
    }

    public string ExpandProg(List<int> program, Registers registers) {
        StringBuilder outputs = new();
        Registers curRegs = registers;
        for(int i = 0; i < program.Count; i += 2) {
            long opcode = program[i];
            long operand = program[i+1];
            switch (opcode) {
                case 0:

                    int denom = (int) Math.Pow(2, GetComboOperand(operand, curRegs));
                    long num = curRegs.A;
                    curRegs = curRegs with {A = num/denom};
                    outputs.Append($"A = A({num})/{GetComboOperatorRep(operand)}^2({denom}): {curRegs.A}\n");
                    break;
                case 1:
                    long curB = curRegs.B;
                    curRegs = curRegs with {B = curB ^ operand};
                    outputs.Append($"B = B({curB}) XOR {operand}: {curRegs.B}\n");
                    break;
                case 2:
                    long val = GetComboOperand(operand, curRegs);
                    curRegs = curRegs with {B = val % 8};
                    outputs.Append($"B = {GetComboOperatorRep(operand)}({val}) % 8: {curRegs.B}\n");

                    break;
                case 3:
                    if(curRegs.A != 0) {
                        i = (int)operand - 2;
                        outputs.Append($"Jumping to {operand}\n");
                    }
                    break;
                case 4:
                    long currentB = curRegs.B;
                    long currentC = curRegs.C;
                    curRegs = curRegs with {B = currentB ^ currentC};
                    outputs.Append($"B = B({currentB}) XOR C({currentC}): {curRegs.B}\n");
                    break;
                case 5:
                    long curVal = GetComboOperand(operand, curRegs);
                    outputs.Append($"output: {GetComboOperatorRep(operand)}({curVal}) % 8: {curVal % 8}\n");
                    break;
                case 6:
                    denom = (int) Math.Pow(2, GetComboOperand(operand, curRegs));
                    num = curRegs.A;
                    curRegs = curRegs with {B = num/denom};
                    outputs.Append($"B = A({num})/{GetComboOperatorRep(operand)}^2({denom}): {curRegs.B}\n");
                    break;
                case 7:
                    denom = (int) Math.Pow(2, GetComboOperand(operand, curRegs));
                    num = curRegs.A;
                    curRegs = curRegs with {C = num/denom};
                    outputs.Append($"C = A({num})/{GetComboOperatorRep(operand)}^2({denom}): {curRegs.C}\n");
                    break;
            }
        }
        return outputs.ToString();
    }

    public static char GetComboOperatorRep(long operand) => operand switch
    {
        1 => '1',
        2 => '2',
        3 => '3',
        4 => 'A',
        5 => 'B',
        6 => 'C',
        _ => throw new Exception("unknown operand"),
    };

    public override string Part2(string path)
    {
        List<string> lines = File.ReadLines(path).ToList();
        int[] regs = new int[3];
        for(int i = 0; i < 3; i++) {
            string[] vals = lines[i].Split(": ");
            regs[i] = int.Parse(vals[1]);
        }
        Registers registers = new(regs[0], regs[1], regs[2]);
        List<int> program = lines[4].Split(": ")[1].Split(",").Select(int.Parse).ToList();
        string expanded = ExpandProg(program, registers);
        //Console.Write(expanded);
        for(int i = 0; i < 64; i++) {
            long res = FastCalcA(i);
            //Console.Write(res + " ");
        }
        return registers.A.ToString();
    }

    public static long FastCalcA(long A)
    {
        return (((A ^ 1) ^ 5) ^ (A / (long)Math.Pow(2,A ^ 1))) % 8;
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