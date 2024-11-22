using System;
using System.Linq;

class MyVector<T>
{
    private T[] elementData;
    private int elementCount;
    private int capacityIncrement;

    public MyVector(int initialCapacity, int capacityIncrement)
    {
        elementData = new T[initialCapacity];
        elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }

    public MyVector(int initialCapacity)
    {
        elementData = new T[initialCapacity];
        capacityIncrement = 0;
        elementCount = 0;
    }

    public MyVector()
    {
        elementData = new T[10];
        capacityIncrement = 0;
        elementCount = 0;
    }

    public MyVector(T[] a)
    {
        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        elementCount = a.Length;
    }

    public void Add(T e)
    {
        EnsureCapacity();
        elementData[elementCount++] = e;
    }

    public void AddAll(T[] a)
    {
        foreach (T item in a)
            Add(item);
    }

    public void Clear()
    {
        elementData = new T[elementData.Length];
        elementCount = 0;
    }

    public bool Contains(object o)
    {
        for (int i = 0; i < elementCount; i++)
            if (elementData[i].Equals(o))
                return true;
        return false;
    }

    public bool ContainsAll(T[] a)
    {
        return a.All(item => Contains(item));
    }

    public bool IsEmpty()
    {
        return elementCount == 0;
    }

    public void Remove(object o)
    {
        int index = IndexOf(o);
        if (index >= 0)
            RemoveAt(index);
    }

    public void RemoveAll(T[] a)
    {
        foreach (T item in a)
            Remove(item);
    }

    public void RetainAll(T[] a) // Оставление только указанных объектов
    {
        elementData = elementData.Where(item => a.Contains(item)).ToArray();
        elementCount = elementData.Length;
    }

    public int Size()
    {
        return elementCount;
    }

    public T[] ToArray()
    {
        T[] result = new T[elementCount];
        Array.Copy(elementData, result, elementCount);
        return result;
    }

    public T[] ToArray(T[]? a) // Преобразование в массив с заданным размером
    {
        if (a == null || a.Length < elementCount)
            a = new T[elementCount];
        Array.Copy(elementData, a, elementCount);
        return a;
    }

    public void Add(int index, T e) // Добавление элемента в заданную позицию
    {
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        EnsureCapacity();
        Array.Copy(elementData, index, elementData, index + 1, elementCount - index);
        elementData[index] = e;
        elementCount++;
    }

    public void AddAll(int index, T[] a) // Добавление массива элементов в указанную позицию
    {
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        EnsureCapacity(a.Length);
        Array.Copy(elementData, index, elementData, index + a.Length, elementCount - index);
        Array.Copy(a, 0, elementData, index, a.Length);
        elementCount += a.Length;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }

    public int IndexOf(object o) // Получение индекса первого вхождения объекта
    {
        for (int i = 0; i < elementCount; i++)
            if (elementData[i].Equals(o))
                return i;
        return -1;
    }

    public int LastIndexOf(object o) // Получение индекса последнего вхождения объекта
    {
        for (int i = elementCount - 1; i >= 0; i--)
            if (elementData[i].Equals(o))
                return i;
        return -1;
    }

    public T RemoveInd(int index) // Удаление и возвращение элемента по индексу
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        T e = elementData[index];
        RemoveAt(index);
        return e;
    }

    public void Set(int index, T e)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        elementData[index] = e;
    }

    public MyVector<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();

        T[] subArray = new T[toIndex - fromIndex];
        Array.Copy(elementData, fromIndex, subArray, 0, toIndex - fromIndex);
        return new MyVector<T>(subArray);
    }

    public T FirstElement() // Получение первого элемента
    {
        if (IsEmpty())
            throw new InvalidOperationException("Vector is empty");
        return elementData[0];
    }

    public T LastElement() // Получение последнего элемента
    {
        if (IsEmpty())
            throw new InvalidOperationException("Vector is empty");
        return elementData[elementCount - 1];
    }

    public void RemoveElementAt(int pos) // Удаление элемента по индексу
    {
        RemoveAt(pos);
    }

    public void RemoveRange(int begin, int end) // Удаление диапазона элементов
    {
        if (begin < 0 || end > elementCount || begin >= end)
            throw new ArgumentOutOfRangeException();

        int len = end - begin;
        Array.Copy(elementData, end, elementData, begin, elementCount - end);
        elementCount -= len;
    }

    private void RemoveAt(int index) // Вспомогательный метод для удаления элемента по индексу
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        Array.Copy(elementData, index + 1, elementData, index, elementCount - index - 1);
        elementData[--elementCount] = default!;
    }

    private void EnsureCapacity(int additionalCapacity = 1) // Вспомогательный метод для увеличения ёмкости
    {
        if (elementCount + additionalCapacity > elementData.Length)
        {
            int newCapacity = elementData.Length + (capacityIncrement > 0 ? capacityIncrement : elementData.Length);
            Array.Resize(ref elementData, Math.Max(newCapacity, elementCount + additionalCapacity));
        }
    }

    public void Print()
    {
        for (int i = 0; i < elementCount; i++)
            Console.Write($"{elementData[i]} ");
        Console.WriteLine();
    }
}

class MyStack<T> : MyVector<T>
{
    public MyStack() : base() { }
    public void Push(T item)
    {
        Add(item); 
    }

    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack пустой.");

        return RemoveInd(Size() - 1); 
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack пустой.");

        return Get(Size() - 1); 
    }

    public bool Empty()
    {
        return IsEmpty();
    }

    // Метод для поиска глубины элемента
    public int Search(T item)
    {
        for (int i = Size() - 1; i >= 0; i--)
        {
            if (Get(i).Equals(item))
                return Size() - i;
        }
        return -1; 
    }

    public void PrintStack()
    {
        Console.WriteLine("Stack элементы (сверху в низ):");
        for (int i = Size() - 1; i >= 0; i--)
            Console.WriteLine(Get(i));
    }
}
class Program
{
    static void Main(string[] args)
    {
        MyStack<int> stack = new MyStack<int>();

        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        stack.PrintStack();

        Console.WriteLine($"Peek: {stack.Peek()}");
        Console.WriteLine($"Pop: {stack.Pop()}");
        Console.WriteLine($"Is Empty: {stack.Empty()}");

        stack.Push(40);
        stack.Push(50);
        Console.WriteLine($"Search 40: {stack.Search(40)}");
        stack.PrintStack();
    }
}

