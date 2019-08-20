# Утиная типизация (реализация полиморфизма)

class Quote:
    def __init__(self, person, words):
        self.person = person
        self.words = words

    def who(self):
        return self.person

    def says(self):
        return self.words + '.'

class QuestionQuote(Quote):
    def says(self):
        return self.words + '?'

class ExclamationQuote(Quote):
    def says(self):
        return self.words + '!'

hunter = Quote('Elmer Fudd', "I'm hunting wabbits")
print(hunter.who(), 'says:', hunter.says())
# Elmer Fudd says: I'm hunting wabbits.

hunted1 = QuestionQuote('Bugs Bunny', "What's up, doc")
print(hunted1.who(), 'says', hunted1.says())
# Bugs Bunny says What's up, doc?

hunted2 = ExclamationQuote('Daffy Duck', "It's rabbit season")
print(hunted2.who(), 'says', hunted2.says())
# Daffy Duck says It's rabbit season!
print('---')

# Для отдельного класса, который не является наследником от класса Quote
# полиморфизм (утиная типизация) также работает:
class BabblingBrook:
    def who(self):
        return 'Brook'

    def says(self):
        return 'Babble'

brook = BabblingBrook()

def who_says(obj):
    print(obj.who(), 'says', obj.says())

who_says(hunter)        # Elmer Fudd says I'm hunting wabbits.
who_says(hunted1)       # Bugs Bunny says What's up, doc?
who_says(hunted2)       # Daffy Duck says It's rabbit season!
who_says(brook)         # Brook says Babble
print('---')
