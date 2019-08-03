# Создание кортежей с помощью оператора ()
empty_tuple = ()

# Это вариант для кортежей с одним элементом:
tuple_one_item = 'Groucho',

# Если в вашем кортеже более одного элемента, ставьте запятую после каждого
# из них, кроме последнего:
marx_tuple = 'Groucho', 'Chico', 'Harpo'
normal_tuple = ('Groucho', 'Chico', 'Harpo')
print(empty_tuple)
print(tuple_one_item)
print(marx_tuple)
print(normal_tuple)
print('---')

# Кортежи позволяют вам присвоить несколько переменных за один раз:
a, b, c = normal_tuple
print(a)                # Groucho
print(b)                # Chico
print(c)                # Harpo
print('---')

# Можно использовать кортежи, чтобы обменять значения с помощью одного
# выражения, без применения временной переменной:
password = 'swordfish'
icecream = 'tuttifrutti'
print('Password =', password)       # Password = swordfish
print('Icecream =', icecream)       # Icecream = tuttifrutti
print('After swap')
password, icecream = icecream, password
print('Password =', password)       # Password = tuttifrutti
print('Icecream =', icecream)       # Icecream = swordfish
print('---')
