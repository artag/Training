guess_me = 7
if (guess_me > 7):
    print('too high')
elif (guess_me == 7):
    print('just right')
else:
    print('too low')

print('---')

guess_me = 7
start = 1
while start <= guess_me:
    if (start < guess_me):
        print('too low')
    if (start == guess_me):
        print('found it')
        break
    start += 1
print('Guessed number:', start)

print('---')

for num in range(3, -1, -1):
    print(num)

print('---')

even_numbers = [number for number in range(10) if number % 2 == 0]
print(even_numbers)

print('---')

squares = {number: number * number for number in range(10)}
print(squares)

print('---')

odd_numbers = {number for number in range(10) if number % 2 == 1}
print(odd_numbers)

print('---')

for item in ('Got %s' % number for number in range(10)):
    print(item)

print('---')


def good():
    return ['Harry', 'Ron', 'Hermione']

print(good())
print('---')


get_odds = (number for number in range(10) if number % 2 == 1)
index = 1
for odd in get_odds:
    if (index == 3):
        print('The third number is:', odd)
        break
    index += 1
else:
    print('Not found')

print('---')

# Another answer
def get_odds():
    for number in range(1, 11, 2):
        yield number

for count, number in enumerate(get_odds(), 1):
    if count == 3:
        print('The third number is:', number)
        break

print('---')

# STOPPED ON 10