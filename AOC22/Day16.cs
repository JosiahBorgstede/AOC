namespace AOC22;

using System.Text.RegularExpressions;

public class Day16 {

    static Dictionary<string,(int, List<string>)> graph = new Dictionary<string,(int, List<string>)>();
    public static void Part1(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
        Regex getLineInfo = new Regex(@"Valve (?<name>\D{2}) has flow rate=(?<pres>\d*); tunnels? leads? to valves? (?<conn>(\D{2}(, )?)*)");
        foreach (string line in lines) {
            Match match = getLineInfo.Match(line);
            graph[match.Groups["name"].Value] = (int.Parse(match.Groups["pres"].Value), match.Groups["conn"].Value.Split(", ").ToList());
        }
        List<string> nodes = [.. graph.Keys.Order()];
        int[][] betterGraph = transformGraph();

        int curTime = 30;
        string curNode = "AA";
        int sum = 0;
        while(curTime > 0) {
            (string best, int bestDist) = SearchForBestNode(curNode, curTime);
            sum += valOfNode(bestDist, curTime, graph[best].Item1);
            graph[best] = (0, graph[best].Item2);
            curTime = curTime - bestDist+1;
        }
        Console.WriteLine(sum);
        //(string best, int bestDist) = SearchForBestNode("AA", 5);
        //Console.WriteLine("Best node: " + best + " with a distance of " +bestDist);
    }

    public static void Part2(string path) {
        IEnumerable<string> lines = File.ReadLines(path);
    }

    public static int[][] transformGraph(){
        List<string> nodes = [.. graph.Keys.Order()];
        int[][] betterGraph = new int[nodes.Count][];
        for (int i =0; i<betterGraph.Length; i++) {
            betterGraph[i] = new int[nodes.Count];
        }
        foreach(var (name, node) in graph) {
            int idx = nodes.IndexOf(name);
            betterGraph[idx][idx] = node.Item1;
            foreach(var connected in node.Item2) {
                int conIdx = nodes.IndexOf(connected);
                betterGraph[idx][conIdx] = 1;
                betterGraph[conIdx][idx] = 1;
            }
        }
        return betterGraph;
    }

    public static void ReduceGraph(int[][] graph, int start)
    {
        bool[] visited = new bool[graph.GetLength(0)];
        for(int i = 0; i<graph.Length;i++) {
            if(graph[i][i] == 0) {

            }
        }
    }
    public static (string, int) SearchForBestNode(string start, int remainingTime)
    {
        Dictionary<string, int> visited = new Dictionary<string, int>();
        Dictionary<string, int> distances = new Dictionary<string, int>();
        Dictionary<string, string> parents = new Dictionary<string, string>();
        foreach (string key in graph.Keys) {
            visited[key] = -1;
        }
        Queue<string> queue = new Queue<string>();
        distances[start] = 0;
        queue.Enqueue(start);
        while (queue.Any()) {
            string curr = queue.Dequeue();
            foreach (string connected in graph[curr].Item2) {
                if (visited[connected] == -1) {
                    parents[connected] = curr;
                    distances[connected] = distances[curr] + 1;
                    queue.Enqueue(connected);
                    int val = valOfNode(distances[connected], remainingTime, graph[connected].Item1);
                    visited[connected] = val;
                    //Console.WriteLine("Value of valve " + connected + ": " + val);
                }
            }
        }
        string best = visited.MaxBy(x => x.Value).Key;
        return (best, distances[best]);
    }

    public static int valOfNode(int distance, int remainingTime, int pressure)
    {
        return (remainingTime - (distance + 1)) * pressure;
    }
}