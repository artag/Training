stack = []
stack.append('abc')
stack.append('def')
stack.append('ghi')
print('Initial stack:', stack)

while len(stack) > 0:
    popped = stack.pop()
    print('Popped:', popped)
    print('Stack:', stack)
print('\n---\n')

queue = []
queue.append('abc')
queue.append('def')
queue.append('ghi')
print('Initial queue:', queue)

while len(queue) > 0:
    item = queue.pop(0)
    print('Item:', item)
    print('Queue:', queue)
