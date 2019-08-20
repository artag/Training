# Именованный кортеж - это подкласс кортежей, с помощью которых можно
# получить доступ к значениям по имени (с помощью конструкции .name )
# и позиции (с помощью конструкции [offset]).

# Реализация с помощью классов (композиция)
class Bill:
    def __init__(self, description):
        self.description = description

class Tail:
    def __init__(self, length):
        self.length = length

class Duck:
    def __init__(self, bill, tail):
        self.bill = bill
        self.tail = tail

    def about(self):
        print('This duck has a', self.bill.description, 'bill and a',
              self.tail.length, 'tail')

bill = Bill('wide orange')
tail = Tail('long')
duck = Duck(bill, tail)
duck.about()                # This duck has a wide orange bill and a long tail
print('---')


# Реализация с помощью именованных кортежей
from collections import namedtuple

Duck2 = namedtuple('Duck', 'bill tail')
duck2 = Duck2('wide orange', 'long')

print(duck2)                # Duck(bill='wide orange', tail='long')
print(duck2.bill)           # wide orange
print(duck2.tail)           # long
print('---')

# Именованный кортеж можно сделать также на основе словаря
# Аргумент **parts извлекает ключи и значения словаря parts и
# передает их как аргументы в Duck()
parts = {'bill': 'wide orange', 'tail': 'long'}
duck3 = Duck2(**parts)
print(duck3)                # Duck(bill='wide orange', tail='long')
print('---')

# Именованные кортежи неизменяемы, но вы можете заменить одно или несколько
# полей и вернуть другой именованный кортеж:
duck4 = duck3._replace(tail='magnificent', bill='crushing')
print(duck4)                # Duck(bill='crushing', tail='magnificent')
print('---')


# Плюсы использования именованного кортежа.
# 1. Они выглядят и действуют как неизменяемый объект.
# 2. Они более эффективны, чем объекты, с точки зрения времени и
# занимаемого места.
# 3. Вы можете получить доступ к атрибутам с помощью точки вместо
# квадратных скобок, характерных для словарей.
# 4. Вы можете использовать их как ключ словаря.
