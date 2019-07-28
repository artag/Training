rabbits = ['Flopsy', 'Mopsy', 'Cottontail', 'Peter']
for rabbit in rabbits:
    print(rabbit)
print('---')

accusation = {'room': 'ballroom', 'weapon': 'lead pipe', 'person': 'Mustard'}
for card in accusation:
    print(card)
print()
for value in accusation.values():
    print(value)
print()
for item in accusation.items():
    print(item)
for card, contents in accusation.items():
    print('Card', card, 'has the contents', contents)
print('---')

cheeses = []
for cheese in cheeses:
    print('This shop has some lovely', cheese)
    break
else:
    print('This is not much of a cheese shop, is it?')
print('---')
