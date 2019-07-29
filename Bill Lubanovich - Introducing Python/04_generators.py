number_thing = (number for number in range(1, 6))
print(type(number_thing))       # <class 'generator'>
print('---')

for number in number_thing:
    print(number)               # 1 2 3 4 5
print('---')

try_again = list(number_thing)
print(try_again)                # []
print('---')
