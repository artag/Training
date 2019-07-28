print(99, 'bottles', 'would be enough.')
print('---')

num0 = str(98.6)
num1 = str(1.0e4)
num2 = str(True)
print(num0)
print(num1)
print(num2)
print('---')

print('a\tbc')
print('a\nbc')
print('a\\bc')
print('---')

print('Release the kraken! ' + 'At once')
print('First word ' 'Second word')
print('---')

a = 'Duck.'
b = a
c = 'Grey Duck!'
print(a + b + c)
print('---')

start = 'Na ' * 4 + '\n'
middle = 'Hey ' * 3 + '\n'
end = 'Goodbye.'
print(start + start + middle + end)
print('---')

letters = 'abcdefghigklmnopqrstuvwxyz'
print(letters[0])
print(letters[1])
print(letters[-1])
print(letters[-2])
print('---')

name = 'Henny'
print(name.replace('H', 'K'))
print('---')

print('All letters [:]', letters[:])
print('[20:]', letters[20:])
print('[12:15]', letters[12:15])
print('[-3:]', letters[-3:])
print('[18:-3]', letters[18:-3])
print('[-6:-2]', letters[-6:-2])
print('[::7]', letters[::7])
print('[4:20:3]', letters[4:20:3])
print('[19::4]', letters[19::4])
print('[:21:5]', letters[:21:5])
print('Reverse order [::-1]', letters[::-1])
print('---')

print('Letters length:', len(letters))
empty = ''
print('Empty length:', len(empty))
print('---')

print('Test split')
todos = 'get gloves,get mask,give cat vitamins,call ambulance'
print(todos.split(','))
print('---')

print('Test join')
crypto_list = ['Yeti', 'Bigfoot', 'Loch Ness Monster']
crypto_string = ', '.join(crypto_list)
print(crypto_string)
print('---')

poem = '''All that doth flow we cannot liquid name
Or else would fire and water be the same;
But that is liquid which is moist and wet
Fire that property can never get.
Then 'tis not cold that doth the fire put out
But 'tis the wet that makes it die, no doubt.'''
print('The poem:\n', poem, '\n')
print('Poem length:', len(poem))
print('Starts with "All"', poem.startswith('All'))
print('End with "folks!"', poem.endswith('Folks'))
word = 'the'
print('First index for "the', poem.find(word))
print('Last index for "the"', poem.rfind(word))
print('Count of "the"', poem.count(word))
print('isalnum', poem.isalnum())
print('---')

setup = 'a duck goes into a bar...'
print('Initial string:', setup)
print('Strip:', setup.strip('.'))
print('Capitalize:', setup.capitalize())
print('Title:', setup.title())
print('In upper case:', setup.upper())
print('In lower case:', setup.lower())
print('Swap case:', setup.swapcase())
print('30 symbols. Center:', setup.center(30))
print('30 symbols. Left:', setup.ljust(30))
print('30 symbols. Right:', setup.rjust(30))
print('---')

print('Replace: ', setup.replace('duck', 'marmoset'))
print('Replace: ', setup.replace('a ', 'a famous ', 100))
