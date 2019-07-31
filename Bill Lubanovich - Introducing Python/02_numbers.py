# Деление с плавающей точкой
print('7 / 2 =', 7 / 2)                 # 3.5

# Целочисленное (Truncating) деление
print('7 // 2 =', 7 // 2)               # 3

# Modulus (вычисление остатка)
print('7 % 2 =', 7 % 2)                 # 1

# Возведение в степень
print('7 ** 3 =', 7 ** 3)               # 343
print('---')

# Получить частное и остаток одновременно
print('divmod(9, 5)', divmod(9, 5))     # (1, 4)

# Получить частное и остаток по отдельности
print('9 // 5 =', 9 // 5)               # 1
print('9 % 5 =', 9 % 5)                 # 4
print('---')

# Задание чисел в различных системах счисления
print('bin 10:', 0b10)                  # 2
print('oct 10:', 0o10)                  # 8
print('hex 10:', 0x10)                  # 16
print('---')

# Преобразования типов. Целые числа
print('int(True) =', int(True))         # 1
print('int(False) =', int(False))       # 0
print('int(98.6) =', int(98.6))         # 98
print('int(1.0e4) =', int(1.0e4))       # 10000
print('4 + 7.0 =', 4 + 7.0)             # 11.0
print('True + 2 =', True + 2)           # 3
print('False + 5.0 =', False + 5.0)     # 5.0
print('---')

# Преобразования типов. Числа с плавающей точкой
print(float(True))                      # 1.0
print(float(False))                     # 0.0
print(float(98))                        # 9.0
