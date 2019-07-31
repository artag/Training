# 1. Присвойте значение 7 переменной guess_me . Далее напишите условные провер-
# ки ( if , else и elif ), чтобы вывести строку 'too low' , если значение переменной
# guess_me меньше 7, 'too high', если оно больше 7, и 'just right', если равно 7.

guess_me = 7
if (guess_me > 7):
    print('too high')
elif (guess_me == 7):
    print('just right')
else:
    print('too low')

print('---')


# 2. Присвойте значение 7 переменной guess_me и значение 1 переменной start.
# Напишите цикл while, который сравнивает переменные start и guess_me. Выве-
# дите строку 'too low', если значение переменной start меньше значения пере-
# менной guess_me. Если значение переменной start равно значению переменной
# guess_me, выведите строку 'found it!' и выйдите из цикла. Если значение пере-
# менной start больше значения переменной guess_me, выведите строку 'oops'
# и выйдите из цикла. Увеличьте значение переменной start на выходе из цикла.

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


# 3. Используйте цикл for , чтобы вывести на экран значения списка [3, 2, 1, 0]
for num in range(3, -1, -1):
    print(num)

print('---')


# 4. Используйте включение списка, чтобы создать список, который содержит не-
# четные числа в диапазоне range(10).

even_numbers = [number for number in range(10) if number % 2 == 0]
print(even_numbers)

print('---')


# 5. Используйте включение словаря, чтобы создать словарь squares . Используйте
# вызов range(10) , чтобы получить ключи, и возведите их в квадрат, чтобы полу-
# чить их значения.

squares = {number: number * number for number in range(10)}
print(squares)

print('---')


# 6. Используйте включение множества, чтобы создать множество odd, которое со-
# держит четные числа в диапазоне range(10).

odd_numbers = {number for number in range(10) if number % 2 == 1}
print(odd_numbers)

print('---')


# 7. Используйте включение генератора, чтобы вернуть строку 'Got' и количество
# чисел в диапазоне range(10). Итерируйте по нему с помощью цикла for.

for item in ('Got %s' % number for number in range(10)):
    print(item)

print('---')


# 8. Определите функцию good, которая возвращает список ['Harry', 'Ron', 'Hermione'].

def good():
    return ['Harry', 'Ron', 'Hermione']

print(good())
print('---')


# 9. Определите функцию генератора get_odds, которая возвращает четные числа из
# диапазона range(10). Используйте цикл for, чтобы найти и вывести третье воз-
# вращенное значение.

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

# Еще один вариант решения

def get_odds():
    for number in range(1, 11, 2):
        yield number

for count, number in enumerate(get_odds(), 1):
    if count == 3:
        print('The third number is:', number)
        break

print('---')


# 10. Определите декоратор test, который выводит строку 'start', когда вызывается
# функция, и строку 'end', когда функция завершает свою работу.

def test(func):
    def new_function(*args, **kwargs):
        print('start')
        result = func(*args, **kwargs)
        print('end')
        return result
    return new_function

@test
def add_two_numbers(a, b):
    return a + b

result = add_two_numbers(4, 2)
print(result)
print('---')


# 11. Определите исключение, которое называется OopsException. Сгенерируйте его,
# чтобы увидеть, что произойдет. Затем напишите код, позволяющий поймать это
# исключение и вывести строку 'Caught an oops'.

class OopsException(Exception):
    pass

try:
    raise OopsException('Oops! Exception message.')
except OopsException as ex:
    print(ex)

print('---')


# 12. Используйте функцию zip(), чтобы создать словарь movies, который объединя-
# ет в пары эти списки: titles = ['Creature of Habit', 'Crewel Fate'] и plots = ['A nun
# turns into a monster', 'A haunted yarn shop'].

titles = ['Creature of Habit', 'Crewel Fate']
plots = ['A nun turns into a monster', 'A haunted yarn shop']

movies = dict(zip(titles, plots))
print(movies)
