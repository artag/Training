# Подсчитываем элементы с помощью функции Counter()

from collections import Counter
breakfast = ['spam', 'spam', 'eggs', 'spam']
breakfast_counter = Counter(breakfast)
print(breakfast_counter)                # Counter({'spam': 3, 'eggs': 1})
print('---')

# Функция most_common() возвращает все элементы в убывающем порядке или
# лишь те элементы, количество которых больше, чем заданный аргумент count:

items = breakfast_counter.most_common()
print(items)                            # [('spam', 3), ('eggs', 1)]
items = breakfast_counter.most_common(1)
print(items)                            # [('spam', 3)]
print('---')

# Счетчики можно объединять.
lunch = ['eggs', 'eggs', 'bacon']
lunch_counter = Counter(lunch)      # Counter({'eggs': 2, 'bacon': 1})
print(lunch_counter)

counter_sum = breakfast_counter + lunch_counter
print(counter_sum)                  # Counter({'spam': 3, 'eggs': 3, 'bacon': 1})

# Счетчики можно вычитать друг из друга:

# Что мы будем есть на завтрак, но не на обед?
counter_diff1 = breakfast_counter - lunch_counter
print(counter_diff1)                # Counter({'spam': 3})

# Что мы можем съесть на обед, но не можем на завтрак:
counter_diff2 = lunch_counter - breakfast_counter
print(counter_diff2)                # Counter({'eggs': 1, 'bacon': 1})

# Получить общие элементы с помощью оператора пересечения &:
union = breakfast_counter & lunch_counter
print(union)                        # Counter({'eggs': 1})

# Получить все элементы с помощью оператора объединения |:
all = breakfast_counter | lunch_counter
print(all)      # Counter({'spam': 3, 'eggs': 2, 'bacon': 1})

print('---')