
public class Day9 : IDay {

    public void Run(string part, string path) {
        if(part == "1") {
            Part1(path);
        } else {
            Part2(path);
        }
    }

    public string Part1(string path) {
        List<int> lines = File.ReadAllText(path).ToList().ConvertAll(x => int.Parse([x]));
        List<(int, int)> linesWithId = new List<(int, int)> ();
        List<int> fileBlocks = new List<int>();
        for(int i = 0; i < lines.Count; i++) {
            linesWithId.Add((GetFileId(i), lines[i]));
        }
        linesWithId.Reverse();
        for(int i = 0; i < lines.Count; i++) {
            if(i % 2 == 0) {
                if (fileBlocks.Count > 0 && fileBlocks.Last() == GetFileId(i))
                {
                    break;
                }
                for(int j = 0; j < lines[i]; j++) {
                    fileBlocks.Add(GetFileId(i));
                }
            } else {
                for(int j = 0; j < lines[i]; j++) {
                    var valIdx = linesWithId.FindIndex(x => x.Item2 > 0 && x.Item1 != -1);
                    var val = linesWithId[valIdx];
                    fileBlocks.Add(val.Item1);
                    linesWithId[valIdx] = (linesWithId[valIdx].Item1, linesWithId[valIdx].Item2 - 1);
                }
            }
        }
        var finalVal = linesWithId.Find(x => x.Item2 > 0 && x.Item1 != -1);
        if(fileBlocks.Last() == finalVal.Item1) {
            for(int i = 0; i < finalVal.Item2; i++) {
                fileBlocks.Add(finalVal.Item1);
            }
        }
        double sum = 0;
        for(int i = 0; i < fileBlocks.Count; i++) {
            sum += fileBlocks[i] * i;
        }
        return sum.ToString();
    }

    public static int GetFileId(int index) {
        return index % 2 == 1 ? -1 : index/2;
    }

    public static List<int> buildFileBlocks(List<(int, int)> idAndCount) {
        List<int> fileBlocks = [];
        foreach(var block in idAndCount) {
            int valToAdd = block.Item1 == -1 ? 0 : block.Item1;
            for(int i = 0; i < block.Item2; i++) {
                fileBlocks.Add(valToAdd);
            }
        }
        return fileBlocks;
    }

    public string Part2(string path) {
        List<int> lines = File.ReadAllText(path).ToList().ConvertAll(x => int.Parse([x]));
        List<(int, int)> linesWithId = [];
        for(int i = 0; i < lines.Count; i++) {
            linesWithId.Add((GetFileId(i), lines[i]));
        }
        List<(int, int)> revLinesWithId = linesWithId.Reverse<(int, int)>().Where(x => x.Item1 >= 0).ToList();
        for(int i = 0; i < revLinesWithId.Count; i++) {
            int fitIdx = linesWithId.FindIndex(x => x.Item1 == -1 && x.Item2 >= revLinesWithId[i].Item2);
            if(fitIdx < 0 || fitIdx > linesWithId.IndexOf(revLinesWithId[i])) {
                continue;
            }
            var gap = linesWithId[fitIdx];
            linesWithId[linesWithId.IndexOf(revLinesWithId[i])] = (-1, revLinesWithId[i].Item2);
            linesWithId.Insert(fitIdx, revLinesWithId[i]);
            linesWithId.RemoveAt(fitIdx + 1);
            if(revLinesWithId[i].Item2 != gap.Item2) {
                if(linesWithId.Count - 1 < fitIdx + 1) {
                    linesWithId.Add((gap.Item1, gap.Item2 - revLinesWithId[i].Item2));
                } else {
                    linesWithId.Insert(fitIdx+1, (gap.Item1, gap.Item2 - revLinesWithId[i].Item2));
                }
            }
        }
        List<int> fileBlocks = [];
        foreach(var block in linesWithId) {
            int valToAdd = block.Item1 == -1 ? 0 : block.Item1;
            for(int i = 0; i < block.Item2; i++) {
                fileBlocks.Add(valToAdd);
            }
        }
        double sum = 0;
        for(int i = 0; i < fileBlocks.Count; i++) {
            sum += fileBlocks[i] * i;
        }
        return sum.ToString();
    }
}