# Упорядочиваем по ключу с помощью OrderedDict()
# Словарь OrderedDict() запоминает порядок, в котором добавлялись ключи,
# и возвращает их в том же порядке с помощью итератора.

from collections import OrderedDict
quotes = OrderedDict([
    ('Moe', 'A wise guy, huh?'),
    ('Larry', 'Ow!'),
    ('Curly', 'Nyuk nyuk!')
])

for stooge in quotes:
    print(stooge)

# Moe
# Larry
# Curly
