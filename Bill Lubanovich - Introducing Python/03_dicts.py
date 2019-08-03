# Создание словаря с помощью {}
empty_dict = {}
bierce = {
    "day": "A period of twenty-four hours, mostly misspent",
    "positive": "Mistaken at the top of one's voice",
    "misfortune": "The kind of fortune that never misses",
}
print(empty_dict)
print(bierce)
print('---')

# Преобразование в словарь с помощью функции dict()
# Во всех этих примерах получается словарь {'a': 'b', 'c': 'd', 'e': 'f'}
lol = [['a', 'b'], ['c', 'd'], ['e', 'f']]
print(dict(lol))
lot = [('a', 'b'), ('c', 'd'), ('e', 'f')]
print(dict(lot))
tol = (['a', 'b'], ['c', 'd'], ['e', 'f'])
print(dict(tol))
los = ['ab', 'cd', 'ef']
print(dict(los))
tos = ('ab', 'cd', 'ef')
print(dict(tos))
print('---')

# Добавление или изменение элемента с помощью конструкции [ключ]
pythons = {
    'Chapman': 'Graham',
    'Cleese': 'John',
}
print(pythons)

# Если ключ новый, он и указанное значение будут добавлены в словарь.
pythons['Gilliam'] = 'Gerry'
print(pythons)

# Если ключ уже существует в словаре, имеющееся значение будет заменено новым.
pythons['Gilliam'] = 'Terry'
print(pythons)
print('---')

# Объединение словарей с помощью функции update()
letters = {'one': 'a', 'two': 'b', 'three': 'c'}
others = {'four': 'e', 'two': 'z', 'five': 'o'}
print('letters: ', letters)
print('others:', others)
letters.update(others)
print('letters after update:', letters)
# {'one': 'a', 'two': 'z', 'three': 'c', 'four': 'e', 'five': 'o'}
print('---')

# Удаление элементов по их ключу с помощью del
# Удаление всех элементов с помощью функции clear()
letters = {'one': 'a', 'four': 'd', 'two': 'b'}
print('letters:', letters)                  # {'one': 'a', 'four': 'd', 'two': 'b'}
del letters['four']
print('delete "four":', letters)            # {'one': 'a', 'two': 'b'}
letters.clear()
print('After clear() execute', letters)     # {}
print('---')

# Получение элемента словаря с помощью конструкции [ключ].
# Если ключа в словаре нет, будет сгенерировано исключение.
# Решение:
# 1. Проверяем на наличие ключа с помощью in
# 2. Используем специальную функцию словаря get().
#    Указывается словарь, ключ и опциональное значение.
#    Если ключ существует, то возвращается связанное с ним значение.
#    Если такого ключа нет, возвращается опциональное значение, если оно было указано.
#    Если опциональное значение не было указано, то будет возвращен объект None
letters = {'one': 'a', 'two': 'b', 'three': 'c'}
print('letters:', letters)
print('"two" exists in letters =', 'two' in letters)    # True
three_value = letters.get('three')
four_value = letters.get('four', 'Not found')
five_value = letters.get('five')
print('Existed value "three":', three_value)            # c
print('Non existed value:', four_value)                 # Not found
print('Non existed value:', five_value)                 # None
print('---')

# Получение всех ключей с помощью функции keys()
# Получение всех значений с помощью функции values()
# Получение всех пар «ключ — значение» с помощью функции items()
signals = {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
print("--Keys")
print(signals.keys())           # 'green', 'yellow', 'red'
print(list(signals.keys()))
print("--Values")
print(signals.values())         # 'go', 'go faster', 'stop'
print(list(signals.values()))
print("--Key-value")
print(signals.items())          # ('green', 'go'), ('yellow', 'go faster'), ('red', 'stop')
print(list(signals.items()))
print('---')

# Присваиваем значения с помощью оператора =
signals = {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
save_signals = signals
signals['blue'] = 'confuses'
print('signals:', signals)              # {'green': 'go', 'yellow': 'go faster', 'red': 'stop', 'blue': 'confuses'}
print('save_signals:', save_signals)    # {'green': 'go', 'yellow': 'go faster', 'red': 'stop', 'blue': 'confuses'}
print('---')

# Копируем значения с помощью функции copy()
signals = {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
a = signals.copy()
b = dict(signals)
signals['blue'] = 'confuses'
print('signals:', signals)      # {'green': 'go', 'yellow': 'go faster', 'red': 'stop', 'blue': 'confuses'}
print('a:', a)                  # {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
print('b:', b)                  # {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
print('---')
