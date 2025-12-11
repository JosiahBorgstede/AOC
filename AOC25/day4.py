
def createGrid(file):
    f = open(file, "r")
    grid = []
    for line in f:
        gridLine = []
        for char in line:
            if char == '@':
                gridLine.append(1)
            if char == '.':
                gridLine.append(0)
        grid.append(gridLine)
    f.close()
    return grid

def inBounds(x, y, grid):
    return x >= 0 and y >= 0 and x < len(grid) and y < len(grid[x])


# part 1
def part1(filename):
    grid = createGrid(filename)
    ans = 0
    for i in range(len(grid)):
        for j in range(len(grid[i])):
            if grid[i][j] == 0:
                continue
            surround = 0
            if inBounds(i, j+1, grid) and grid[i][j+1] == 1:
                surround +=1
            if inBounds(i, j-1, grid) and grid[i][j-1] == 1:
                surround +=1
            if inBounds(i+1, j+1, grid) and grid[i+1][j+1] == 1:
                surround +=1
            if inBounds(i+1, j-1, grid) and grid[i+1][j-1] == 1:
                surround +=1
            if inBounds(i+1, j, grid) and grid[i+1][j] == 1:
                surround +=1
            if inBounds(i-1, j+1, grid) and grid[i-1][j+1] == 1:
                surround +=1
            if inBounds(i-1, j-1, grid) and grid[i-1][j-1] == 1:
                surround +=1
            if inBounds(i-1, j, grid) and grid[i-1][j] == 1:
                surround +=1
            if surround < 4:
                ans += 1
    return ans

def part2(filename):
    grid = createGrid(filename)
    ans = 0
    removedThisRun = 1
    while removedThisRun > 0:
        removedThisRun = 0
        for i in range(len(grid)):
            for j in range(len(grid[i])):
                if grid[i][j] == 0:
                    continue
                surround = 0
                if inBounds(i, j+1, grid) and grid[i][j+1] == 1:
                    surround +=1
                if inBounds(i, j-1, grid) and grid[i][j-1] == 1:
                    surround +=1
                if inBounds(i+1, j+1, grid) and grid[i+1][j+1] == 1:
                    surround +=1
                if inBounds(i+1, j-1, grid) and grid[i+1][j-1] == 1:
                    surround +=1
                if inBounds(i+1, j, grid) and grid[i+1][j] == 1:
                    surround +=1
                if inBounds(i-1, j+1, grid) and grid[i-1][j+1] == 1:
                    surround +=1
                if inBounds(i-1, j-1, grid) and grid[i-1][j-1] == 1:
                    surround +=1
                if inBounds(i-1, j, grid) and grid[i-1][j] == 1:
                    surround +=1
                if surround < 4:
                    ans += 1
                    grid[i][j] = 0
                    removedThisRun += 1
    print(ans)

part2("input4.txt")