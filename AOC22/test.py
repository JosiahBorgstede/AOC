infile = open('./Inputs/Day21.txt', 'r', newline='')
lines = [line.rstrip('\n') for line in infile] 
while 'root' not in locals():
    for line in lines: 
        variable, operation = line.split(':') 
        try: 
            locals()[variable] = eval(operation) 
        except NameError: 
            pass; #variable is not defined yet 
print(f'root is: {root}')