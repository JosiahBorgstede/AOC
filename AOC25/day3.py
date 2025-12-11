f = open("input3.txt", "r")
ans = 0
# # part 1
# for line in f:
#     #print(line)
#     firstMax = 0
#     secondMax = 0
#     for i in range(len(line) - 1):
#         if int(line[i]) > firstMax and i < len(line) - 2:
#             firstMax = int(line[i])
#             secondMax = 0
            
#             continue
#         elif int(line[i]) > secondMax:
#             secondMax = int(line[i])
#     print(line)
#     print(f"{firstMax}{secondMax}")
#     ans += (firstMax * 10) + secondMax

# part 2
for line in f:
    digits = []
    curStart = 0
    curNum = 0
    for i in range(12):
        curMax = 0
        for j in range(curStart, len(line) - (12 - i)):
            if int(line[j]) > curMax:
                curMax = int(line[j])
                curStart = j+1
        digits.append(curMax)
        curNum += curMax * 10**(11-i)
    print(curNum)
    ans += curNum
print(ans)