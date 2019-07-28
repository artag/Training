disaster = True
if disaster:
    print("Woe!")
else:
    print("Whee!")
print('---')

color = 'puce'
if color == 'red':
    print("It's tomato")
elif color == 'green':
    print("It's a green pepper")
elif color == 'bee purple':
    print("I don't know what it is, but only bees can see it")
else:
    print("I've never heard of the color", color)
print('---')

x = 7
print(5 < x and x < 10)
print(5 < x < 10)
print(5 < x and not x > 10)
print('---')

print('False examples:')
print(bool(False))
print(bool(None))
print(bool(0))
print(bool(0.0))
print(bool([]))
print(bool(''))
print(bool({}))
print(bool(set()))
