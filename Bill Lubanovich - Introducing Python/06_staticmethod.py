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
