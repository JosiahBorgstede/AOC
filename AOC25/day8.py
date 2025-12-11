from typing import List
from math import sqrt

type Position = tuple[int, int, int]
def readPositions(file):
    positions = []
    lines = open(file, "r").readlines()
    for line in lines:
        cleanLine = line.strip()
        values = cleanLine.split(",")
        positions.append((int(values[0]), int(values[1]), int(values[2])))
    return positions

def getDistance(p1: Position, p2: Position):
    return sqrt((p1[0] - p2[0])**2 + (p1[1] - p2[1])**2 + (p1[2] - p2[2])**2)

def getNextClosestPositions(positions: List[Position], circuits: List[List[Position]], startDist: int):
    curDist = getDistance(positions[0], positions[1])
    pos1, pos2 = positions[0], positions[1]
    for i in range(len(positions)):
        startPos = positions[i]
        for endPos in positions[i:]:
            dist = getDistance(startPos, endPos)
            if dist < startDist:
                continue
            if dist > startDist and dist < curDist:
                curDist = getDistance(startPos, endPos)
                pos1 = startPos
                pos2 = endPos
    return (pos1, pos2)

def positionsConnected(circuits: List[List[Position]], pos1: Position, pos2: Position) -> bool:
    for circuit in circuits:
        if pos1 in circuit and pos2 in circuit:
            return True
    return False

def addConnectionToCircuits(circuits: List[List[Position]], pos1: Position, pos2: Position):
    newCircuits = circuits[:]
    pos1Circuit, pos2Circuit = [], []
    for circuit in circuits:
        if pos1 in circuit:
            pos1Circuit = circuit
        if pos2 in circuit:
            pos2Circuit = circuit
    if pos1Circuit == pos2Circuit:
        return newCircuits
    newCircuits.remove(pos1Circuit)
    newCircuits.remove(pos2Circuit)
    newCircuits.append(pos1Circuit + pos2Circuit)
    return newCircuits

def part1(file):
    positions = readPositions(file)
    circuits = [[pos] for pos in positions]
    startDist = 0
    for _ in range(1000):
        (pos1, pos2) = getNextClosestPositions(positions, circuits, startDist)
        startDist = getDistance(pos1, pos2)
        circuits = addConnectionToCircuits(circuits, pos1, pos2)
    sortedCir = sorted(circuits, key=len, reverse=True)
    print(list(map(len, sortedCir)))
    ans = len(sortedCir[0]) * len(sortedCir[1]) * len(sortedCir[2])
    print(ans)

def getNextClosestUnconnectedPositions(positions: List[Position], circuits: List[List[Position]], distGrid: List[List[int]]):
    curDist = getDistance(positions[0], positions[1])
    pos1, pos2 = positions[0], positions[1]
    for i in range(len(positions)):
        startPos = positions[i]
        for j in range(i, len(positions)):
            if i == j:
                continue
            dist = distGrid[i][j]
            if dist > curDist:
                continue
            endPos = positions[j]
            if positionsConnected(circuits, startPos, endPos):
                continue
            curDist = dist
            pos1 = startPos
            pos2 = endPos
    return (pos1, pos2)

def buildDistanceGrid(positions: List[Position]):
    distances = [[0 for _ in range(len(positions))] for _ in range(len(positions))]
    for i in range(len(positions)):
        for j in range(len(positions)):
            distances[i][j] = getDistance(positions[i], positions[j])
    return distances

def part2(file):
    positions = readPositions(file)
    circuits = [[pos] for pos in positions]
    pos1 = positions[0]
    pos2 = positions[1]
    dists = buildDistanceGrid(positions)
    while len(circuits) > 1:
        (pos1, pos2) = getNextClosestUnconnectedPositions(positions, circuits, dists)
        circuits = addConnectionToCircuits(circuits, pos1, pos2)   
    ans = pos1[0] * pos2[0]
    print(ans)
   
part1("input8.txt")

#part2("input8.txt")      