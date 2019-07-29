def run_something(func, arg1, arg2):
    func(arg1, arg2)

def add_args(arg1, arg2):
    print(arg1 + arg2)

run_something(add_args, 4, 2)
print('---')


def run_with_positional_args(func, * args):
    return func(*args)

def sum_args(*args):
    return sum(args)

result = run_with_positional_args(sum_args, 1, 2, 3, 4)
print(result)       # 10
print('---')
