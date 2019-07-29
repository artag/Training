for x in range(0, 3):
    print(x)            # 0 1 2
print('---')

from_0_to_2 = list(range(0, 3))
print(from_0_to_2)
print('---')

for x in range(2, -1, -1):
    print(x)            # 2 1 0
print('---')

from_2_to_0 = list(range(2, -1, -1))
print(from_2_to_0)
print('---')

print(list(range(0, 11, 2)))    # 0 2 4 6 8 10
