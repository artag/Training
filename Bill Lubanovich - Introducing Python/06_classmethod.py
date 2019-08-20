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
A.kids()                    # A has 3 little objects.
