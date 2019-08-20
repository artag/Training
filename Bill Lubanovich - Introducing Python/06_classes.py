# Определяем класс с помощью ключевого слова class
# Конструктор
class Person():
    def __init__(self, name):
        self.name = name

hunter = Person('Elmer Fudd')
print('The mighty hunter:', hunter.name)    # The mighty hunter: Elmer Fudd
print('---')


# Наследование и перегрузка метода
class Car():
    def exclaim(self):
        print("I'm a Car!")

class Yugo(Car):
    def exclaim(self):
        print("I'm Yugo!")

give_me_a_car = Car()
give_me_a_yougo = Yugo()

give_me_a_car.exclaim()         # I'm a Car!
give_me_a_yougo.exclaim()       # I'm Yugo!
print('---')


# Перегрузить можно любые методы, включая __init__()
class Person():
    def __init__(self, name):
        self.name = name

class MDPerson(Person):
    def __init__(self, name):
        self.name = "Doctor " + name

class JDPerson(Person):
    def __init__(self, name):
        self.name = name + ", Esquire"

person = Person('Fudd')
doctor = MDPerson('Fudd')
lawyer = JDPerson('Fudd')

print(person.name)              # Fudd
print(doctor.name)              # Doctor Fudd
print(lawyer.name)              # Fudd, Esquire
print('---')


# Просим помощи у предка с помощью ключевого слова super
class Person():
    def __init__(self, name):
        self.name = name

class EmailPerson(Person):
    def __init__(self, name, email):
        super().__init__(name)
        self.email = email

bob = EmailPerson('Bob Frapples', 'bob@frapples.com')
print('Name:', bob.name)                # Name: Bob Frapples
print('Email:', bob.email)              # Email: bob@frapples.com
print('---')


# Искажение имен для безопасности
# Получаем и устанавливаем значение атрибутов с помощью свойств
# Способ 1
class Duck:
    def __init__(self, input_name):
        self.__name = input_name

    def get_name(self):
        print('inside the getter')
        return self.__name

    def set_name(self, input_name):
        print('inside the setter')
        self.__name = input_name

    name = property(get_name, set_name)

fowl = Duck('Howard')
new_name = fowl.name                # inside the getter
print(new_name)                     # Howard

fowl.name = 'Daffy'                 # inside the setter
new_name = fowl.name                # inside the getter
print(new_name)                     # Daffy

# Методы get_name() и set_name() можно вызвать непосредственно:
fowl.set_name('Donald')         # inside the setter
new_name = fowl.get_name()      # inside getter
print(new_name)
print('---')


# Искажение имен для безопасности
# Получаем и устанавливаем значение атрибутов с помощью свойств
# Способ 2
class Circle:
    def __init__(self, radius):
        self.__radius = radius

    @property
    def radius(self):
        print('inside the radius getter')
        return self.__radius

    @radius.setter
    def radius(self, value):
        print('inside the radius setter')
        self.__radius = value

    @property
    def diameter(self):
        print('inside the diameter getter')
        return 2 * self.__radius

c = Circle(5)
r = c.radius                            # inside the radius getter
print('Radius =', r)                    # Radius = 5
d = c.diameter                          # inside the diameter getter
print('Diameter =', d)                  # Diameter = 10
print('---')

new_radius = c.radius = 12              # inside the radius setter
print('New radius =', new_radius)       # New radius = 12

# Свойство diameter только для чтения, при попытке set выдаст:
# AttributeError: can't set attribute
# c.diameter = 666

new_diameter = c.diameter               # inside the diameter getter
print('New diameter =', new_diameter)   # New diameter = 24
print('---')

# К полю __name нельзя обратиться напрямую, оно скрыто
# AttributeError: 'Duck' object has no attribute '__name'
# print(fowl.__name)

# Но, можно обратиться к искаженному имени
hidden_name = fowl._Duck__name
print('Hidden name:', hidden_name)      # Hidden name: Donald
print('---')


class Z:
    def __hidden_method(self):
        print('Calling hidden method')

    def call_hidden_method(self):
        print('Call:')
        self.__hidden_method()

z = Z()
# К методу __hidden_method() также нельзя обратиться напрямую, оно скрыто
# a.__hidden_method()
z.call_hidden_method()              # Обращение через вспомогательный метод
print('---')


# Метод класса
# Декоратор @classmethod показывает, что функция является методом класса.
# Метод класса влияет на весь класс целиком.
# Любое изменение, которое происходит с классом, влияет на все его объекты.

class A:
    count = 0

    def __init__(self):
        A.count += 1        # вызвали метод A.count (атрибут класса) вместо self.count

    def exclaim(self):
        print("I'm an A!")

    @classmethod
    def kids(cls):
        print('A has', cls.count, 'little objects.')

easy_a = A()
breezy_a = A()
wheezy_a = A()
A.kids()                        # A has 3 little objects.
print('---')


# Статический метод
# Декоратор @staticmethod.
# Статический метод не влияет ни на классы, ни на объекты:
# он находится внутри класса только для удобства вместо того,
# чтобы располагаться где-то отдельно.

class CoyoteWeapon:
    @staticmethod
    def commercial():
        print('Message from CoyoteWeapon class')

# Создавать объект класса CoyoteWeapon не нужно
CoyoteWeapon.commercial()       # Message from CoyoteWeapon class
print('---')


# Утиная типизация (реализация полиморфизма)

class Quote:
    def __init__(self, person, words):
        self.person = person
        self.words = words

    def who(self):
        return self.person

    def says(self):
        return self.words + '.'

class QuestionQuote(Quote):
    def says(self):
        return self.words + '?'

class ExclamationQuote(Quote):
    def says(self):
        return self.words + '!'

hunter = Quote('Elmer Fudd', "I'm hunting wabbits")
print(hunter.who(), 'says:', hunter.says())
# Elmer Fudd says: I'm hunting wabbits.

hunted1 = QuestionQuote('Bugs Bunny', "What's up, doc")
print(hunted1.who(), 'says', hunted1.says())
# Bugs Bunny says What's up, doc?

hunted2 = ExclamationQuote('Daffy Duck', "It's rabbit season")
print(hunted2.who(), 'says', hunted2.says())
# Daffy Duck says It's rabbit season!
print('---')

# Для отдельного класса, который не является наследником от класса Quote
# полиморфизм (утиная типизация) также работает:
class BabblingBrook:
    def who(self):
        return 'Brook'

    def says(self):
        return 'Babble'

brook = BabblingBrook()

def who_says(obj):
    print(obj.who(), 'says', obj.says())

who_says(hunter)        # Elmer Fudd says I'm hunting wabbits.
who_says(hunted1)       # Bugs Bunny says What's up, doc?
who_says(hunted2)       # Daffy Duck says It's rabbit season!
who_says(brook)         # Brook says Babble
print('---')


# Особые методы
# Имена этих методов начинаются и заканчиваются двойными подчеркиваниями ( __ ).

class Word:
    def __init__(self, text):
        self.text = text

    def equals(self, word2):
        return self.text.lower() == word2.text.lower()

first = Word('ha')
second = Word('HA')
third = Word('eh')

print('first.equals(second)   ', first.equals(second))      # True
print('first.equals(third)    ', first.equals(third))       # False

# Или equals можно определить через __eq__:

class Word2:
    def __init__(self, text):
        self.text = text

    def __eq__(self, word2):
        return self.text.lower() == word2.text.lower()

first = Word2('ops')
second = Word2('OPS')
third = Word2('Ups')

print('first == second   ', first == second)                # True
print('first == third    ', first == third)                 # False
print('---')

# Остальные "магические" методы

# Магические методы для сравнения
# __eq__(self, other)           self == other
# __ne__(self, other)           self != other
# __lt__(self, other)           self < other
# __gt__(self, other)           self > other
# __le__(self, other)           self <= other
# __ge__(self, other)           self >= other

# Магические методы для вычислений
# __add__(self, other)          self + other
# __sub__(self, other)          self — other
# __mul__(self, other)          self * other
# __floordiv__(self, other)     self // other
# __truediv__(self, other)      self / other
# __mod__(self, other)          self % other
# __pow__(self, other)          self ** other

# Другие магические методы
# __str__(self)                 str(self)
# __repr__(self)                repr(self)
# __len__(self)                 len(self)

class Word3:
    def __init__(self, text):
        self.text = text

    def __eq__(self, word2):
        return self.text.lower() == word2.text.lower()

    def __str__(self):
        return self.text

    def __repr__(self):
        return 'Word3("self.text")'

first = Word3('Ho!')
# Если набрать в строке first, то должен появится результат вызова __repr__
# first
print(first)        # используется __str__, напечатается: "Ho!"
print('---')


# Композиция
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
        print('This duck has a', self.bill.description,
              'bill and a', self.tail.length, 'tail')

tail = Tail('long')
bill = Bill('wide orange')
duck = Duck(bill, tail)
duck.about()                # This duck has a wide orange bill and a long tail
print('---')

