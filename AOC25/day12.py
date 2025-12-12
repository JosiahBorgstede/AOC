from dataclasses import dataclass



@dataclass
class Tree:
    xDim: int
    yDim: int
    presents: list[int]

@dataclass
class Present:
    shape: list[list[bool]]

def createPresent(present):
    shape = []
    for line in present[1:]:
        curLine = []
        for val in line:
            if val == ".":
                curLine.append(False)
            else:
                curLine.append(True)
        shape.append(curLine)
    return Present(shape)
                
def createPresentsAndTrees(file):
    mode = 0
    lines = open(file, "r").readlines()
    trees = []
    presents = []
    curPres = []
    for line in lines:
        if line.count("x") > 0:
            mode = 1
        if mode == 0 and len(line) > 1:
            curPres.append(line.strip())
        elif mode == 0 and len(line) <= 1:
            presents.append(createPresent(curPres))
            curPres = []
        if mode == 1:
            vals = line.strip().split(" ")
            dims = vals[0].strip(":").split("x")
            tree = Tree(int(dims[0]), int(dims[1]), list(map(int, vals[1:])))
            trees.append(tree)
    return (trees, presents)

def part1(file):
    print(createPresentsAndTrees(file))

part1("testinput12.txt")
    