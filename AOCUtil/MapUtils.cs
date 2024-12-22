namespace AOCUtil;

public static class MapHelper  {
    public static void DrawMap<T>(this Map<T> map) {
        DrawMap(map.Data);
    }

    public static void DrawMap<T>(this Map<T> map, Func<T, string> transform) {
        DrawMap(map.Data, transform);
    }

    public static void DrawMap<U>(U[,] map, Func<U, string> transform) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                Console.Write(transform(map[j,i]));
            }
            Console.WriteLine();
        }
    }

    public static void DrawMap<U>(U[,] map, Func<(int, int), string> transform) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                Console.Write(transform((i,j)));
            }
            Console.WriteLine();
        }
    }

    public static void DrawMap<U>(U[,] map) {
        for(int i = 0; i < map.GetLength(1); i++) {
            for(int j = 0; j < map.GetLength(0); j++) {
                Console.Write(map[j,i]?.ToString());
            }
            Console.WriteLine();
        }
    }

    public static bool OutOfBounds(int x, int y, int maxX, int maxY) {
        return x < 0 || y < 0 || x >= maxX || y >= maxY;
    }

    public static char[,] ConvertTextToMap(IEnumerable<string> lines) {
        char[,] map = new char[lines.First().Length, lines.Count()];
        for(int i = 0; i < lines.Count(); i++) {
            for(int j = 0; j < lines.ElementAt(i).Length; j++) {
                map[j,i] = lines.ElementAt(i)[j];
            }
        }
        return map;
    }

    public static (int, int) LocatePoint<T>(T[,] map, T toFind) where T : IEquatable<T> {
        for(int i = 0; i < map.GetLength(0); i++) {
            for(int j = 0; j < map.GetLength(1); j++) {
                if(map[i,j].Equals(toFind)) {
                    return (i, j);
                }
            }
        }
        return (0,0);
    }
}
public class Map<T>  {

    public T[,] Data {
        get => _data;
    }

    private T[,] _data;
    public Map(long maxX, long maxY, T defaultValue) {
        T[,] map = new T[maxX,maxY];
        for(int i = 0; i < maxX; i++) {
            for(int j = 0; j < maxY; j++) {
                map[i,j] = defaultValue;
            }
        }
        _data = map;
    }

    public Map(T[,] map) {
        _data = map;
    }

    public Map(long maxX, long maxY) {
        _data = new T[maxX, maxY];
    }

    public static Map<char> MakeCharMap(IEnumerable<string> lines) {
        char[,] map = new char[lines.First().Length, lines.Count()];
        for(int i = 0; i < lines.Count(); i++) {
            for(int j = 0; j < lines.ElementAt(i).Length; j++) {
                map[j,i] = lines.ElementAt(i)[j];
            }
        }
        return new Map<char>(map);
    }

    public static T[,] MakeSimpleMap(long maxX, long maxY, T defaultValue) {
        T[,] map = new T[maxX,maxY];
        for(int i = 0; i < maxX; i++) {
            for(int j = 0; j < maxY; j++) {
                map[i,j] = defaultValue;
            }
        }
        return map;
    }

    public bool OutOfBounds(int x, int y) {
        return MapHelper.OutOfBounds(x, y, Data.GetLength(0), Data.GetLength(1));
    }
}
