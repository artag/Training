voted = {}

def check_voter(name):
    print("Voter " + name)
    if voted.get(name):
        print('kick them out!')
    else:
        print('let them vote')
        voted[name] = True;
    print()

check_voter("Tom")
check_voter("Mark")
check_voter("Tom")
