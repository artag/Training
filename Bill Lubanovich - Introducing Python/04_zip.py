days = ['Monday', 'Tuesday', 'Wednesday']
fruits = ['banana', 'orange', 'peach']
drinks = ['coffee', 'tea', 'beer']
desserts = ['tiramisu', 'ice cream', 'pie', 'pudding']

for day, fruit, drink, dessert in zip(days, fruits, drinks, desserts):
    print(day, ': drink', drink, 'eat', fruit, 'enjoy', dessert)
print()

english = 'Monday', 'Tuesday', 'Wednesday'
french = 'Lundi', 'Mardi', 'Mercredi'
print('English tuples:', english)
print('French tuples:', french)

print('List from zip:')
print(list(zip(english, french)))

print('Dict from zip:')
print(dict(zip(english, french)))
