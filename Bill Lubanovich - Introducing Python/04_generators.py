number_thing = (number for number in range(1, 6))
print(type(number_thing))       # <class 'generator'>
print('---')


for number in number_thing:
    print(number)               # 1 2 3 4 5
print('---')


try_again = list(number_thing)
print(try_again)                # []
print('---')


generator = range(1, 101)
print(sum(generator))           # 5050
print('---')


def my_range_generator(first = 0, last = 10, step = 1):
    number = first
    while number < last:
        yield number
        number += step

print(my_range_generator)       # <function my_range_generator at ...>

ranger = my_range_generator(1, 5)
print(ranger)                   # <generator object my_range_generator at ...>

for x in ranger:
    print(x)                    # 1 2 3 4

for x in ranger:
    print(x)                    # Nothing happens
