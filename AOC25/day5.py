from typing import List

def part1(file):
    f = open(file, "r")
    lines = f.readlines()
    ans = 0
    ranges = []
    values = []
    for line in lines:
        if line.count("-") == 1:
            ranges.append(list(map(int, line.split('-'))))
        elif len(line) > 1:
            values.append(int(line))
    for val in values:
        for range in ranges:
            if val >= range[0] and val <= range[1]:
                ans += 1
                break 
    print(ans)

def part2(file):
    f = open(file, "r")
    lines = f.readlines()
    ans = 0
    ranges = []
    for line in lines:
        if line.count("-") == 1:
            ranges.append(list(map(int, line.split('-'))))
    curLength = 0
    prevLength = len(ranges)
    while curLength < prevLength:
        prevLength = len(ranges)
        ranges = combineRangesOld(ranges)
        curLength = len(ranges)
        print("shrank")
    ranges = combineRangesOld(ranges)
    ranges = combineRangesOld(ranges)
    print(len(ranges))
    print(ranges)
    for range in ranges:
        ans += (range[1] - range[0]) + 1
    print(ans)

def combineRangesOld(ranges : List[List[int]]):
    for curRange in ranges:
        for testRange in ranges:
            if testRange == curRange:
                if ranges.count(curRange) > 1:
                    ranges.remove(curRange)
                break
            if isOverlap(curRange, testRange):
                ranges.remove(curRange)
                ranges.remove(testRange)
                ranges.append(combineRanges(curRange, testRange))
                break
    return ranges

def isOverlap(rangeOne : List[int], rangeTwo : List[int]) -> bool:
    return (rangeOne[0] >= rangeTwo[0] and rangeOne[0] <= rangeTwo[1]) or (rangeOne[1] >= rangeTwo[0] and rangeOne[1] <= rangeTwo[1])

def combineRanges(rangeOne : List[int], rangeTwo : List[int]) -> List[int]:
    return [min(rangeOne[0], rangeTwo[0]), max(rangeOne[1], rangeTwo[1])]
def part2New(file):
    f = open(file, "r")
    lines = f.readlines()
    flips = []
    for line in lines:
        if line.count("-") == 1:
            [start, end] = (list(map(int, line.split('-'))))
            flips.append([start, 1])
            flips.append([end + 1, -1])
    flips = sorted(flips, key=lambda a: a[0])
    print(flips)
    start = 0 # When did we move from no ranges open to at least one range open
    currFlip = 0 # More like a state: how many ranges are currently open
    ans = 0

    for i in range(len(flips)):
        if currFlip == 0 and flips[i][1] == 1: #First range opens
            start = i
        elif currFlip == 1 and flips[i][1] == -1: # Last current range closes
            ans += flips[i][0] - flips[start][0]
        currFlip += flips[i][1]
    print(ans)

        
#part2("testinput5.txt")
part2New("input5.txt")