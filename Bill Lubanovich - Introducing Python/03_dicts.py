empty_dict = {}
bierce = {
    "day": "A period of twenty-four hours, mostly misspent",
    "positive": "Mistaken at the top of one's voice",
    "misfortune": "The kind of fortune that never misses",
}
print(empty_dict)
print(bierce)
print('---')

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

pythons = {
    'Chapman': 'Graham',
    'Cleese': 'John',
}
print(pythons)
pythons['Gilliam'] = 'Gerry'
print(pythons)
pythons['Gilliam'] = 'Terry'
print(pythons)
print('---')

letters = {'one': 'a', 'two': 'b', 'three': 'c'}
others = {'four': 'e', 'two': 'z', 'five': 'o'}
print('letters: ', letters)
print('others:', others)
letters.update(others)
print('letters after update:', letters)
print('---')

letters = {'one': 'a', 'four': 'd', 'two': 'b'}
print('letters:', letters)
del letters['four']
print('delete "four":', letters)
letters.clear()
print('After clear() execute', letters)
print('---')

letters = {'one': 'a', 'two': 'b', 'three': 'c'}
print('letters:', letters)
print('"two" exists in letters =', 'two' in letters)
three_value = letters.get('three')
five_value = letters.get('five')
print('Existed value "three":', three_value)
print('Non existed value:', five_value)
print('---')

signals = {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
print("--Keys")
print(signals.keys())
print(list(signals.keys()))
print("--Values")
print(signals.values())
print(list(signals.values()))
print("--Key-value")
print(signals.items())
print(list(signals.items()))
print('---')

signals = {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
save_signals = signals
signals['blue'] = 'confuses'
print('signals:', signals)
print('save_signals:', save_signals)
print('---')

signals = {'green': 'go', 'yellow': 'go faster', 'red': 'stop'}
a = signals.copy()
b = dict(signals)
signals['blue'] = 'confuses'
print('signals:', signals)
print('a:', a)
print('b:', b)
print('---')
