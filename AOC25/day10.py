from dataclasses import dataclass
from collections import deque
import numpy as np
import re

@dataclass
class Machine:
    lights: list[bool]
    buttons: list[list[int]]
    joltage: list[int]

def createLights(lightStr: str) -> list[bool]:
    vals = []
    for i in range(len(lightStr)):
        if lightStr[i] == ".":
            vals.append(False)
        elif lightStr[i] == "#":
            vals.append(True)
    return vals

def createButtons(buttonStrs: list[str]) -> list[list[int]]:
    vals = []
    for btns in buttonStrs:
        vals.append(list(map(int, btns.split(","))))
    return vals

def createJolts(joltStr: str) -> list[int]:
    return list(map(int, joltStr.split(",")))

def createMachines(file: str) -> list[Machine]:
    lines = open(file, "r").readlines()
    machines = []
    lightsRe = re.compile("\\[([.|#]*)\\]")
    buttonsRe = re.compile("\\(([\\d,]*)\\)")
    joltsRe = re.compile("\\{([\\d,]*)\\}")
    for line in lines:
        lights = lightsRe.findall(line)
        buttons = buttonsRe.findall(line)
        jolts = joltsRe.findall(line)
        machines.append(Machine(createLights(lights[0]), createButtons(buttons), createJolts(jolts[0])))
    return machines    


def computePresses(mach: Machine, presses: list[int]) -> list[bool]:
    init = [False for _ in range(len(mach.lights))]
    for press in presses:
        for btn in mach.buttons[press]:
            init[btn] = not init[btn]
    return init

def getShortest(mach: Machine):
    q = deque()
    q.append([])
    visited = [[]]
    prevHit = []
    while len(q) > 0:
        cur = q.popleft()
        curResult = computePresses(mach, cur)
        if curResult == mach.lights:
            return cur
        if prevHit.count(curResult) > 0:
            continue
        prevHit.append(curResult)
        for i in range(len(mach.buttons)):
            if visited.count(cur + [i]) == 0:
                q.append(cur + [i])
                visited.append(cur + [i])
            
def part1(file):
    machines = createMachines(file)
    ans = 0
    for mach in machines:
        result = getShortest(mach)
        print(result)
        ans += len(result)
    print(ans)

def computeJoltage(mach: Machine, presses: list[int]) -> list[int]:
    init = [0 for _ in range(len(mach.joltage))]
    for i in range(len(presses)):
        for btn in mach.buttons[i]:
            init[btn] += presses[i]
    return init

def invalidJoltage(mach: Machine, cur: list[int]):
    minPresses = max(mach.joltage)
    if sum(cur) < minPresses:
        return True
    maxVal = max(mach.joltage)
    for i in range(len(cur)):
        if cur[i] > mach.joltage[i] or cur[i] > maxVal:
            return True
    return False

def createMat(mach: Machine):
    array = []
    for btn in mach.buttons:
        curArray = []
        for i in range(len(mach.joltage)):
            if btn.count(i) > 0:
                curArray.append(1)
            else:
                curArray.append(0)
        array.append(curArray)
    return np.array(array)


def getShortestJoltage(mach: Machine):
    sortedBtns = sorted(mach.buttons, key=lambda item: len(item), reverse=True)
    q = deque()
    q.append([0 for _ in range(len(mach.buttons))])
    visited = [[0 for _ in range(len(mach.buttons))]]
    prevHit = []
    solved = False
    while not solved and len(q) > 0:
        cur = q.popleft()
        curResult = computeJoltage(mach, cur)
        if curResult == mach.joltage:
            print(curResult)
            return cur
        if prevHit.count(curResult) > 0:
            continue
        prevHit.append(curResult)
        if invalidJoltage(mach, curResult):
            for i in range(len(mach.buttons)):
                copyCur = cur[:]
                copyCur[i] -= 1
                if visited.count(copyCur) == 0 and copyCur[i] >= 0:
                    print(copyCur)
                    print(len(q))
                    q.append(copyCur)
                    visited.append(copyCur)
            continue
        for i in range(len(mach.buttons)):
            copyCur = cur[:]
            copyCur[i] += 25
            if visited.count(copyCur) == 0:
                q.append(copyCur)
                visited.append(copyCur)

# def getShortestJoltage(mach: Machine):
#     q = deque()
#     q.append([])
#     visited = [[]]
#     prevHit = []
#     solved = False


#     while not solved and len(q) > 0:
#         cur = q.popleft()
#         curResult = computeJoltage(mach, cur)
#         if curResult == mach.joltage:
#             return cur
#         if prevHit.count(curResult) > 0:
#             continue
#         prevHit.append(curResult)
#         if invalidJoltage(mach, curResult):
#             continue
#         for i in range(len(mach.buttons)):
#             if visited.count(cur + [i]) == 0:
#                 q.append(cur + [i])
#                 visited.append(cur + [i])

def part2(file):
    machines = createMachines(file)
    ans = 0
    for mach in machines:
        result = getShortestJoltage(mach)
        print(result)
        ans += sum(result)
    print(ans)

part1("testinput10.txt")
part2("input10.txt")