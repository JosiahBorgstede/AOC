f = open("input1.txt", "r")

curPos = 50
zeros = 0
for line in f:
    clicks = int(line[1:])
    dir = 1 if line[0] == "R" else -1
    for i in range(clicks):
        curPos += dir
        curPos = curPos % 100
        if curPos == 0:
            zeros += 1
        

print(zeros)

