empty_tuple = ()
tuple_one_item = 'Groucho',
marx_tuple = 'Groucho', 'Chico', 'Harpo'
normal_tuple = ('Groucho', 'Chico', 'Harpo')
print(empty_tuple)
print(tuple_one_item)
print(marx_tuple)
print(normal_tuple)
print('---')

a, b, c = normal_tuple
print(a)
print(b)
print(c)
print('---')

password = 'swordfish'
icecream = 'tuttifrutti'
print('Password =', password)
print('Icecream =', icecream)
print('After swap')
password, icecream = icecream, password
print('Password =', password)
print('Icecream =', icecream)
print('---')
