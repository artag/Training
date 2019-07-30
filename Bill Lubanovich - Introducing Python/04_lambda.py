def edit_story(words, func):
    for word in words:
        print(func(word))

stairs = ('thud', 'meow', 'thud', 'hiss')
edit_story(stairs, lambda word: word.capitalize() + '!')
# Thud!
# Meow!
# Thud!
# Hiss!
print('---')
