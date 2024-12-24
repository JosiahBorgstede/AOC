namespace AOC24;

public class Day23 : ADay
{
    public override int DayNum => 23;

    public override string Part1(string path)
    {
        Dictionary<string, List<string>> graph = BuildGraph(File.ReadAllLines(path));
        IEnumerable<(string, string, string)> groups = GroupsOfThree(graph);
        return groups.Count(x => DetermineIfGroupContainsT(x)).ToString();
    }

    public bool DetermineIfGroupContainsT((string A, string B, string C) group) {
        return group.A.StartsWith('t') || group.B.StartsWith('t') || group.C.StartsWith('t');
    }

    public static Dictionary<string, List<string>> BuildGraph(IEnumerable<string> connections)
    {
        Dictionary<string, List<string>> graph = [];
        foreach (string connection in connections)
        {
            var sides = connection.Split('-');
            if(graph.TryGetValue(sides[0], out var curList)) {
                curList.Add(sides[1]);
            } else {
                graph.Add(sides[0], [sides[1]]);
            }

            if(graph.TryGetValue(sides[1], out curList)) {
                curList.Add(sides[0]);
            } else {
                graph.Add(sides[1], [sides[0]]);
            }
        }
        return graph;
    }

    public static IEnumerable<(string, string)> BuildEdgesOfGraph(IEnumerable<string> connections) {
        foreach(var conn in connections) {
            var nodes = conn.Split('-');
            yield return (nodes[0], nodes[1]);
        }
    }

    public static IEnumerable<(string, string, string)> GroupsOfThree(Dictionary<string, List<string>> graph) {
        List<(string, string, string)> groups = [];
        List<string> visited = [];
        foreach((var nodeA, var listNodes) in graph) {
            List<string> subVisited = [];
            foreach(var nodeB in listNodes) {
                subVisited.Add(nodeB);
                if(visited.Contains(nodeB)) {
                    continue;
                }
                foreach(var nodeC in graph[nodeB]) {
                    if(visited.Contains(nodeC) || subVisited.Contains(nodeC)) {
                        continue;
                    }
                    if(graph[nodeC].Contains(nodeA)) {
                        yield return (nodeA, nodeB, nodeC);
                    }
                }
            }
            visited.Add(nodeA);
        }
    }

    public override string Part2(string path)
    {
        Dictionary<string, List<string>> graph = BuildGraph(File.ReadAllLines(path));
        IEnumerable<IEnumerable<string>> groups = FullyConnectedGroups(graph);
        IEnumerable<string> best = groups.MaxBy(g => g.Count())!;
        var bestSorted = best.Order();
        //Console.WriteLine(string.Join(",", bestSorted));
        return string.Join(",", bestSorted);
    }

    public IEnumerable<IEnumerable<string>> FullyConnectedGroups(Dictionary<string, List<string>> graph) {
        List<List<string>> fullyConnected = [];
        List<string> visited = [graph.Keys.First()];
        Queue<string> toVisit = [];
        toVisit.Enqueue(graph.Keys.First());
        while(toVisit.Count > 0) {
            var curNode = toVisit.Dequeue();
            fullyConnected.Add([curNode]);
            foreach(var adj in graph[curNode]) {
                if(!visited.Contains(adj)) {
                    visited.Add(adj);
                    toVisit.Enqueue(adj);
                }
            }
            foreach(var list in fullyConnected) {
                if(list.All(x => graph[curNode].Contains(x))) {
                    list.Add(curNode);
                }
            }
        }
        return fullyConnected.Where(x => x.Count > 3);
    }
}