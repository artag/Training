# Просим помощи у предка с помощью ключевого слова super
class Person():
    def __init__(self, name):
        self.name = name

class EmailPerson(Person):
    def __init__(self, name, email):
        super().__init__(name)
        self.email = email

bob = EmailPerson('Bob Frapples', 'bob@frapples.com')
print('Name:', bob.name)                # Name: Bob Frapples
print('Email:', bob.email)              # Email: bob@frapples.com
