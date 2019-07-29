def outer(a, b):
    def inner(c, d):
        return c + d
    return inner(a, b)

print(outer(4, 7))      # 11
print('---')


def knights(saying):
    def inner(quote):
        return "We are the knights who say: '%s'" % quote
    return inner(saying)

# We are the knights who say: 'Ni!'
print(knights('Ni!'))
print('---')
