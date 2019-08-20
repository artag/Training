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