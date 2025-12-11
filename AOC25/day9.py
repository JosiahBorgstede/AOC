type Position = tuple[int, int]

def getPositions(file):
    lines = open(file, "r").readlines()
    positions = []
    for line in lines:
        clean = line.strip()
        vals = clean.split(",")
        positions.append((int(vals[0]), int(vals[1])))
    return positions

def GetRectSize(p1: Position, p2: Position):
    return (abs(p1[0] - p2[0]) + 1) * (abs(p1[1] - p2[1])+1)

def part1(file):
    positions = getPositions(file)
    curbest = 0
    for pos in positions:
        for pos2 in positions:
            if pos == pos2:
                continue
            if GetRectSize(pos, pos2) > curbest:
                curbest = GetRectSize(pos, pos2)
    print(curbest)

def createRectangles(positions: list[Position]):
    rectangles = {}
    for pos1 in positions:
        for pos2 in positions:
            if pos1 == pos2:
                continue
            rectangles[(pos1, pos2)] = GetRectSize(pos1, pos2)
    return dict(sorted(rectangles.items(), key=lambda item: item[1], reverse=True))

def isValidRectangle(rect : tuple[Position, Position], positions: list[Position]):
    rectXMax = max(rect[0][0], rect[1][0])
    rectXMin = min(rect[0][0], rect[1][0])
    rectYMax = max(rect[0][1], rect[1][1])
    rectYMin = min(rect[0][1], rect[1][1])
    prevPos = positions[-1]
    for pos in positions:
        if pos[0] > rectXMin and pos[0] < rectXMax and pos[1] > rectYMin and pos[1] < rectYMax:
            return True
        if prevPos[0] == pos[0] and pos[0] > rectXMin and pos[0] < rectXMax: #vertical line, and X is within rect X range
            lineYMax = max(pos[1], prevPos[1])
            lineYMin = min(pos[1], prevPos[1])
            if lineYMax >= rectYMax and lineYMin <= rectYMin: #if the line starts and ends around the rect, must intersect
                return True
        if prevPos[1] == pos[1] and pos[1] > rectYMin and pos[1] < rectYMax: # same, but horizontal lines
            lineXMax = max(pos[0], prevPos[0])
            lineXMin = min(pos[0], prevPos[0])
            if lineXMax >= rectXMax and lineXMin <= rectXMin:
                return True
        prevPos = pos
    return False
    

def part2(file):
    positions = getPositions(file)
    rects = createRectangles(positions)
    for rect in rects:
        if isValidRectangle(rect, positions):
            continue
        print(rect)
        print(rects[rect])
        break
        
part1("input9.txt")
part2("input9.txt")

