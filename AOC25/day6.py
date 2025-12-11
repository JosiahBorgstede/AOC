from typing import List
import functools
import operator

def readFileToProblems(file) -> List[List[str]]:
    finalVals= []    
    lines = open(file, "r").readlines()
    numStart = 0
    numEnd = 0
    for i in range(len(lines[0])):
        if checkIfColIsSpace(lines, i):
            finalVals.append(extraColOfVals(lines, numStart, numEnd))
            numStart = i+1
        numEnd = i
    finalVals.append(extraColOfVals(lines, numStart, numEnd))
    return finalVals

def checkIfColIsSpace(values : List[str], col : int) -> bool:
    for line in values:
        if line[col] != " " and line[col] != "\n":
            return False
    return True

def extraColOfVals(values : List[str], start : int, end : int) -> List[str]:
    finalVals = []
    for lines in values:
        finalVals.append(lines[start:end+1])
    return finalVals

def part1(file):
    ans = 0
    problems = readFileToProblems(file)
    for problem in problems:
        if problem[-1].count("*") == 1:
            ans += functools.reduce(operator.mul, map(int, problem[:-1]))
        if problem[-1].count("+") == 1:
            ans += functools.reduce(operator.add, map(int, problem[:-1]))
    print(ans)

def readFileToProblemsPart2(file) -> List[List[str]]:
    problems = []    
    lines = open(file, "r").readlines()
    curProblem = []
    for i in range(len(lines[0])):
        if checkIfColIsSpace(lines, i):
            problems.append(curProblem)
            curProblem = []
        else:
            if lines[-1][i] != " ":
                curProblem.append(lines[-1][i])
            curProblem.append(readSingleCol(lines[:-1], i))
        
    return problems

def readSingleCol(values : List[str], col : int):
    val = ""
    for line in values:
        val += line[col]
    return val

def part2(file):
    ans = 0
    problems = readFileToProblemsPart2(file)
    for problem in problems:
        if problem[0] == '*':
            ans += functools.reduce(operator.mul, map(int, problem[1:]))
        if problem[0] == '+':
            ans += functools.reduce(operator.add, map(int, problem[1:]))
    print(ans)

    
part2("testinput6.txt")
