# Стек + очередь == deque

# deque — это двухсторонняя очередь, которая имеет возможности стека и очереди.
# Она полезна, когда вы хотите добавить и удалить элементы с любого конца
# последовательности.
# Функция popleft() удаляет крайний слева элемент deque и возвращает его,
# функция pop() удаляет крайний справа элемент и возвращает его.

def palindrome(word):
    from collections import deque
    dq = deque(word)
    while len(dq) > 1:
        if dq.popleft() != dq.pop():
            return False
        return True

print(palindrome('a'))          # None
print(palindrome('racecar'))    # True
print(palindrome(''))           # None
print(palindrome('radar'))      # True
print(palindrome('halibut'))    # False
print('---')


# Хотя можно реализовать palindrome гораздо проще:
def another_palindrome(word):
    return word == word[::-1]

print(another_palindrome('a'))          # True
print(another_palindrome('racecar'))    # True
print(another_palindrome(''))           # True
print(another_palindrome('radar'))      # True
print(another_palindrome('halibut'))    # False
