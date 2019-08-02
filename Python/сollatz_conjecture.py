# The Collatz conjecture is a conjecture in mathematics that concerns a sequence defined as follows:
# 1. Start with any positive integer n.
# 2. Then each term is obtained from the previous term as follows:
#    - If the previous term is even, the next term is one half the previous term.
#    - If the previous term is odd, the next term is 3 times the previous term plus 1.
# The conjecture is that no matter what value of n, the sequence will always reach 1.


def input_positive_integer():
    number = -1

    while number <= 0:
        input_value = input('Enter positive number: ')

        try:
            number = int(input_value)
        except:
            print('Invalid entered value.')

    return number


def collatz(number):
    print('The Collatz conjecture:')

    step = 0
    print('step:', step, ':', number)
    
    while number != 1:

        number_is_even = number % 2 == 0
        if number_is_even:
            number = number // 2;
        else:
            number = 3 * number + 1

        step += 1
        print('step:', step, ':', number)


number = input_positive_integer()
collatz(number)
