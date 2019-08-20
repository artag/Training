# 1. Создайте класс, который называется Thing, не имеющий содержимого, и выведите его на экран.
# Затем создайте объект example этого класса и также выведите его.
# Совпадают ли выведенные значения?

class Thing:
    pass

print(Thing)                # <class '__main__.Thing'>

example = Thing()
print(example)              # <__main__.Thing object at 0x000001AAE19783C8>
print('---')


# 2. Создайте новый класс с именем Thing2 и присвойте его атрибуту letters значение 'abc'.
# Выведите на экран значение атрибута letters.

class Thing2:
    letters = 'abc'

print(Thing2.letters)       # abc
print('---')


# 3. Создайте еще один класс Thing3.
# В этот раз присвойте значение 'xyz' атрибуту объекта, который называется letters.
# Выведите на экран значение атрибута letters. Понадобилось ли вам создавать
# объект класса, чтобы сделать это?

class Thing3:
    def __init__(self, letters):
        self.letters = letters

# print(Thing3.letters)     # Не работает
example3 = Thing3('xyz')
print(example3.letters)     # xyz
print('---')


# 4. Создайте класс, который называется Element, имеющий атрибуты объекта
# name, symbol и number.
# Создайте объект этого класса со значениями 'Hydrogen', 'H' и 1.

class Element:
    def __init__(self, name, symbol, number):
        self.name = name
        self.symbol = symbol
        self.number = number

    def __str__(self):
        return 'Element: ' + self.name + ', symbol ' + self.symbol + ', number ' + str(self.number)

element = Element('Hydrogen', 'H', 1)
print(element)                              # Hydrogen, symbol H, number 1
print('---')


# 5. Создайте словарь со следующими ключами и значениями: 'name': 'Hydrogen' ,
# 'symbol': 'H' , 'number': 1 . Далее создайте объект с именем hydrogen класса Element
# с помощью этого словаря.

h_dict = {'name': 'Hydrogen', 'symbol': 'H', 'number': 1}

# Долгий путь
hydrogen = Element(h_dict['name'], h_dict['symbol'], h_dict['number'])
print(hydrogen)                             # Hydrogen, symbol H, number 1

# Более грамотный путь
hydrogen2 = Element(**h_dict)
print(hydrogen2)                            # Hydrogen, symbol H, number 1
print('---')


# 6. Для класса Element определите метод с именем dump(), который выводит на экран
# значения атрибутов объекта (name , symbol и number). Создайте объект hydrogen из
# этого нового определения и используйте метод dump(), чтобы вывести на экран
# его атрибуты.

# 7. Вызовите функцию print(hydrogen) . В определении класса Element измените имя
# метода dump на __str__ , создайте новый объект hydrogen и затем снова вызовите
# метод print(hydrogen).

class Element:
    def __init__(self, name, symbol, number):
        self.name = name
        self.symbol = symbol
        self.number = number

    def dump(self):
        print(self.__str__())

    def __str__(self):
        return 'Element: ' + self.name + ', symbol ' + self.symbol + ', number ' + str(self.number)

hydrogen3 = Element('Hydrogen', 'H', 1)
hydrogen3.dump()                            # Element: Hydrogen, symbol H, number 1
print(hydrogen3)                            # Element: Hydrogen, symbol H, number 1
print('---')


# 8. Модифицируйте класс Element, сделав атрибуты name, symbol и number закрытыми.
# Определите для каждого атрибута свойство получателя, возвращающее значение
# соответствующего атрибута.

class Element2:
    def __init__(self, name, symbol, number):
        self.__name = name
        self.__symbol = symbol
        self.__number = number

    @property
    def name(self):
        return self.__name

    @property
    def symbol(self):
        return self.__symbol

    @property
    def number(self):
        return self.__number

helium = Element2('Helium', 'He', 2)
print(helium.name)                      # Helium
print(helium.symbol)                    # He
print(helium.number)                    # 2
print('---')


# 9. Определите три класса: Bear, Rabbit и Octothorpe. Для каждого из них определите всего один
# метод eats(). Он должен возвращать значения
# 'berries' (для Bear), 'clover' (для Rabbit) или 'campers' (для Octothorpe).
# Создайте по одному объекту каждого класса и выведите на экран то, что ест указанное животное.

class Bear:
    def eats(self):
        return 'berries'

class Rabbit:
    def eats(self):
        return 'clover'

class Octothorpe:
    def eats(self):
        return 'campers'

bear = Bear()
rabbit = Rabbit()
octothorpe = Octothorpe()

print(bear.eats())                  # berries
print(rabbit.eats())                # clover
print(octothorpe.eats())            # campers
print('---')


# 10. Определите три класса: Laser, Claw и SmartPhone. Каждый из них имеет только
# один метод does(). Он возвращает значения
# 'disintegrate' (для Laser), 'crush' (для Claw ) или 'ring' (для SmartPhone).
# Далее определите класс Robot, который содержит по одному объекту каждого из этих классов.
# Определите метод does() для класса Robot, который выводит на экран все, что делают его компоненты.

class Laser:
    def does(self):
        return 'disintegrate'

class Claw:
    def does(self):
        return 'crush'

class SmartPhone:
    def does(self):
        return 'ring'

class Robot:
    def __init__(self):
        self.laser = Laser()
        self.claw = Claw()
        self.smartphone = SmartPhone()

    def does(self):
        return 'I have many attachements:\n' \
               'My laser, to %s\n' \
               'My claw, to %s\n' \
               'My smartphone, to %s.\n' \
               % (self.laser.does(), self.claw.does(), self.smartphone.does())

robot = Robot()
print(robot.does())
# I have many attachements:
# My laser, to disintegrate
# My claw, to crush
# My smartphone, to ring.
