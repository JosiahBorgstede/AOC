from typing import List

type Position = tuple[int, int]

def part1(file):
    lines = open(file, "r").readlines()
    startPos = getStartPos(lines)
    grid = createLaserGrid(lines)
    filledGrid = fillLazerGridPart1(grid, startPos)
    prettyPrintGird(filledGrid)
    
def getStartPos(lines):
    for i in range(len(lines[0])):
        if lines[0][i] == 'S':
            return (0, i)

def createLaserGrid(lines):
    grid = []
    for line in lines:
        gridLine = []
        for char in line:
            if char == '^':
                gridLine.append(1)
            else:
                gridLine.append(0)
        grid.append(gridLine)
    return grid

def fillLazerGridPart1(grid: List[List[int]], startPos: Position):
    grid[startPos[0]][startPos[1]] = 2
    ans = 0
    for i in range(1, len(grid)):
        for j in range(len(grid[i])):
            if grid[i][j] == 0 and grid[i-1][j] == 2:
                grid[i][j] = 2
            if grid[i][j] == 1 and grid[i-1][j] == 2:
                grid[i][j-1] = 2
                grid[i][j+1] = 2
                ans += 1
    print(ans)
    return grid

def fillLazerGridPart2(grid: List[List[int]], startPos: Position):
    timeGrid = [[0 for _ in range(len(grid[0]))] for _ in range(len(grid))]
    grid[startPos[0]][startPos[1]] = 2
    timeGrid[startPos[0]][startPos[1]] = 1
    for i in range(1, len(grid)):
        for j in range(len(grid[i])):
            if (grid[i][j] == 0 or grid[i][j] == 2) and grid[i-1][j] == 2:
                grid[i][j] = 2
                timeGrid[i][j] += timeGrid[i-1][j]
            elif grid[i][j] == 1 and grid[i-1][j] == 2:
                grid[i][j-1] = 2
                grid[i][j+1] = 2
                timeGrid[i][j-1] = timeGrid[i][j-1] + timeGrid[i-1][j]
                timeGrid[i][j+1] = timeGrid[i][j+1] + timeGrid[i-1][j]


    return timeGrid

def prettyPrintGird(grid: List[List[int]]):
    for line in grid:
        print("".join(map(str,line)))
        

def part2(file):
    lines = open(file, "r").readlines()
    startPos = getStartPos(lines)
    grid = createLaserGrid(lines)
    print(startPos)
    print(grid)
    filledGrid = fillLazerGridPart2(grid, startPos)
    #prettyPrintGird(filledGrid)
    print(sum(filledGrid[-1]))

part1("testinput7.txt")
part2("input7.txt")