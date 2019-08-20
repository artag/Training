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
