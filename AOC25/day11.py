def createGraph(file) -> dict[str, list[str]]:
    adjList = {}
    for line in open(file, "r").readlines():
        vals = line.strip().split(" ")
        adjList[vals[0].strip(":")] = vals[1:]
    return adjList

def countPaths(graph: dict[str, list[str]], cur: str, end: str, visited: dict[str, int]):
    if cur == end: return 1
    if cur not in graph: return 0
    visited[cur] = 0
    for neighbor in graph[cur]:
        if neighbor in visited:
            visited[cur] += visited[neighbor]
        else:
            visited[cur] += countPaths(graph, neighbor, end, visited)
    return visited[cur]
    
def part1(file):
    print(countPaths(createGraph(file), "you", "out", {}))

def part2(file):
    graph = createGraph(file)
    fromSvrToDac = countPaths(graph, "svr", "dac", {})
    fromSvrToFft = countPaths(graph, "svr", "fft", {})
    fromFftToDac = countPaths(graph, "fft", "dac", {})
    fromDacToFft = countPaths(graph, "dac", "fft", {})
    fromDacToOut = countPaths(graph, "dac", "out", {})
    fromFftToOut = countPaths(graph, "fft", "out", {})
    print((fromSvrToDac * fromDacToFft * fromFftToOut) + (fromSvrToFft * fromFftToDac * fromDacToOut))

part1("input11.txt")
part2("input11.txt")