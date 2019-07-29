# Замыкания
# Замыкание - функция, которая динамически генерируется другой функцией,
#             и обе они могут изменяться и запоминать значения переменных,
#             которые были созданы вне функции.

def knights(saying):
    def inner():
        return "We are the knights who say: '%s'" % saying
    return inner

a = knights('Duck')
b = knights('Hasenpfeffer')

# We are the knights who say: 'Duck'
print(a())
# We are the knights who say: 'Hasenpfeffer'
print(b())
print('---')
