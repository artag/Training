# Функция defaultdict() определяет значение по умолчанию для новых ключей заранее,
# при создании словаря.

# В этом примере мы передаем функцию int, которая будет вызываться как int(),
# и возвращаем значение 0.
# Любое отсутствующее значение будет заменяться целым числом (int) 0:

from collections import defaultdict
periodic_table = defaultdict(int)
periodic_table['Hydrogen'] = 1
print(periodic_table['Lead'])       # 0
print(periodic_table)               # defaultdict(<class 'int'>, {'Hydrogen': 1, 'Lead': 0})
print('---')

# Пример 2.
# Функция no_idea() вызывается всякий раз, когда нужно вернуть значение:

def no_idea():
    return 'Huh?'

bestiary = defaultdict(no_idea)
bestiary['A'] = 'Abominable Snowman'
bestiary['B'] = 'Basilisk'
print(bestiary['A'])        # Abominable Snowman
print(bestiary['B'])        # Basilisk
print(bestiary['C'])        # Huh?
print(bestiary)             # defaultdict(<function no_idea at 0x0000025B43219F78>, {'A': 'Abominable Snowman', 'B': 'Basilisk', 'C': 'Huh?'})
print('---')

# Вы можете использовать функции int(), list() или dict(), чтобы возвращать
# пустые значения по умолчанию: int() возвращает 0, list() возвращает пустой спи-
# сок ([]) и dict() возвращает пустой словарь ({}). Если вы опустите аргумент, ис-
# ходное значение нового ключа будет равно None.

# Кстати, вы можете использовать lambda для того, чтобы определить функцию по
# умолчанию изнутри вызова:

bestiary = defaultdict(lambda: 'Ooops!')
print(bestiary['E'])        # Ooops!
print('---')

# Пример 3
# Применение int - это один из способов создать ваш собственный прилавок.
# Eсли бы food_counter был обычным словарем, а не defaultdict, Python генерировал бы исключение
# всякий раз, когда бы мы пытались увеличить элемент словаря food_counter[food], поскольку он
# был бы не инициализирован. (Понадобилось сделать дополнительную работу).

food_counter = defaultdict(int)
for food in ['spam', 'spam', 'eggs', 'spam']:
    food_counter[food] += 1

for food, count in food_counter.items():
    print(food, count)

# spam 3
# eggs 1

