f = open("input2.txt", "r")
ranges = f.read().split(",")
ans = 0
# #part 1 

# for curRange in ranges:
#     vals = curRange.split("-")
#     lower = int(vals[0])
#     upper = int(vals[1])
#     for val in range(lower, upper+1):
#         stringVal = str(val)
#         length = len(stringVal)
#         if length % 2 != 0:
#             continue
#         if stringVal[:int(length/2)] == stringVal[int(length/2):]:
#             print(stringVal)
#             ans += val

# part 2
for curRange in ranges:
    vals = curRange.split("-")
    lower = int(vals[0])
    upper = int(vals[1])
    for val in range(lower, upper+1):
        stringVal = str(val)
        length = len(stringVal)
        for i in range(1, length):
            if (stringVal[:i] * int(length/i)) == stringVal:
                print(val)
                print(i)
                print(stringVal[:i] * int(length/i))
                ans += val
                break

                
print(ans)