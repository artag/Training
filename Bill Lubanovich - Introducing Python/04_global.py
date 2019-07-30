animal = 'fruitbat'

def print_global():
    print('inside print_global:', animal)

def change_and_print_local():
    animal = 'wombat'
    print('inside change_and_print_local:', animal)

def change_and_print_global():
    global animal
    animal = 'wombat'
    print('inside change_and_print_global:', animal)

print_global()
change_and_print_local()
change_and_print_global()

print_global()
change_and_print_local()
change_and_print_global()

print('---')

# locals()
def print_locals():
    local_variable = 'variable'
    local_number = 42
    print(locals())
    print('---')

print_locals()

# globals()
def print_globals():
    local_variable = 'variable'
    local_number = 42
    print(globals())
    print('---')

print_globals()
