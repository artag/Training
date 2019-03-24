def findSmallestIndex(arr):
    smallest_index = 0
    for i in range(1, len(arr)):
        if arr[i] < arr[smallest_index]:
            smallest_index = i
    return smallest_index

def selectionSort(arr):
    newArr = []
    for i in range(len(arr)):
        smallest_index = findSmallestIndex(arr)
        newArr.append(arr.pop(smallest_index))
    return newArr

print(selectionSort([5, 3, 6, 2, 10]))
