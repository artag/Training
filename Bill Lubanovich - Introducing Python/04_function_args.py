def print_args(*args):
    print('Positional argument tuple:', args)

# Positional argument tuple: (3, 2, 1, 'wait!', 'uh...')
print_args(3, 2, 1, 'wait!', 'uh...')
print('---')


def print_more(req1, req2, *args):
    print('First', req1)
    print('Second', req2)
    print('Other', args)

# First cap
# Second gloves
# Other ('scarf', 'monocle', 'mustache wax')
print_more('cap', 'gloves', 'scarf', 'monocle', 'mustache wax')
print('---')
