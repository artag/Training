# Выводим данные на экран красиво с помощью функции pprint()

from collections import OrderedDict
from pprint import pprint
quotes = OrderedDict([
    ('Moe', 'A wise guy, huh?'),
    ('Larry', 'Ow!'),
    ('Curly', 'Nyuk nyuk!')
])

# Просто выводит информацию в одну строку, без форматирования
print(quotes)
print('---')

# pprint() пытается выровнять элементы для лучшей читаемости:
pprint(quotes)

# OrderedDict([('Moe', 'A wise guy, huh?'),
#              ('Larry', 'Ow!'),
#              ('Curly', 'Nyuk nyuk!')])
