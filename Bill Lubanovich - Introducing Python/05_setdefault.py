# Обработка отсутствующих ключей с помощью функции setdefault()

# setdefault() похожа на функцию get(), но она также присваивает
# элемент словарю, если заданный ключ отсутствует:
periodic_table = { 'Hydrogen': 1, 'Helium': 2}
print(periodic_table)       # {'Hydrogen': 1, 'Helium': 2}

# Если ключа еще нет в словаре, будет использовано новое значение:
carbon = periodic_table.setdefault('Carbon', 12)
print(carbon)               # 12
print(periodic_table)       # {'Hydrogen': 1, 'Helium': 2, 'Carbon': 12}

# Если мы пытаемся присвоить другое значение по умолчанию уже существу-
# ющему ключу, будет возвращено оригинальное значение и ничто не изменится:
helium = periodic_table.setdefault('Helium', 947)
print(helium)               # 2
print(periodic_table)       # {'Hydrogen': 1, 'Helium': 2, 'Carbon': 12}
