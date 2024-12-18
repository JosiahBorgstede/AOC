namespace AOC22;
//TODO:this day isn't done
public class Day7 {
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
    }

    public static void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
    }
}

public interface MemoryObj {
    int GetSize();
}

public class Dir : MemoryObj{
    private List<MemoryObj> subFiles;
    public Dir(List<MemoryObj> inside) {
        this.subFiles = inside;
    }

    public int GetSize()
    {
        return subFiles.Sum(x => x.GetSize());
    }
}

public class FileObj : MemoryObj
{
    private int size;
    public FileObj(int size) {
        this.size = size;
    }
    public int GetSize()
    {
        return size;
    }
}