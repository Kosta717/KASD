using System;
using System.Collections.Generic;

public class MyPriorityQueue<T>
{
    private T[] queue; 
    private int size; 
    private readonly IComparer<T> comparator; 

    public MyPriorityQueue() : this(11, Comparer<T>.Default) { }

    public MyPriorityQueue(T[] a) : this(a.Length, Comparer<T>.Default)
    {
        Array.Copy(a, queue, a.Length);
        size = a.Length;
        BuildHeap();
    }
    public MyPriorityQueue(int initialCapacity) : this(initialCapacity, Comparer<T>.Default) { }

    public MyPriorityQueue(int initialCapacity, IComparer<T> comparator)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Capacity должен быть больше 0.");

        queue = new T[initialCapacity];
        size = 0;
        this.comparator = comparator;
    }

    // Конструктор с другой очередью
    public MyPriorityQueue(MyPriorityQueue<T> c) : this(c.size, c.comparator)
    {
        Array.Copy(c.queue, queue, c.size);
        size = c.size;
        BuildHeap();
    }
    public void Add(T e)
    {
        EnsureCapacity();
        queue[size] = e;
        SiftUp(size++);
    }
    public void AddAll(T[] a)
    {
        foreach (var item in a)
        {
            Add(item);
        }
    }
    public void Clear()
    {
        Array.Clear(queue, 0, size);
        size = 0;
    }
    public bool Contains(T o)
    {
        return Array.IndexOf(queue, o, 0, size) >= 0;
    }
    public bool ContainsAll(T[] a)
    {
        foreach (var item in a)
        {
            if (!Contains(item))
                return false;
        }
        return true;
    }
    public bool IsEmpty()
    {
        return size == 0;
    }
    public bool Remove(T o)
    {
        int index = Array.IndexOf(queue, o, 0, size);
        if (index < 0) return false;

        RemoveAt(index);
        return true;
    }
    public void RemoveAll(T[] a)
    {
        foreach (var item in a)
        {
            Remove(item);
        }
    }
    public void RetainAll(T[] a)
    {
        HashSet<T> set = new HashSet<T>(a);
        for (int i = size - 1; i >= 0; i--)
        {
            if (!set.Contains(queue[i]))
            {
                RemoveAt(i);
            }
        }
    }
    public int Size()
    {
        return size;
    }
    public T[] ToArray()
    {
        T[] result = new T[size];
        Array.Copy(queue, result, size);
        return result;
    }

    // Возврат верхнего элемента без удаления
    public T Element()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Queue пуст.");
        return queue[0];
    }

    // Добавление элемента с проверкой
    public bool Offer(T obj)
    {
        try
        {
            Add(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Просмотр верхнего элемента без удаления
    public T Peek()
    {
        return IsEmpty() ? default : queue[0];
    }

    // Удаление и возврат верхнего элемента
    public T Poll()
    {
        if (IsEmpty())
            return default;

        T result = queue[0];
        RemoveAt(0);
        return result;
    }
    private void EnsureCapacity()
    {
        if (size < queue.Length) return;

        int newCapacity = queue.Length < 64 ? queue.Length + 2 : queue.Length + queue.Length / 2;
        Array.Resize(ref queue, newCapacity);
    }
    private void BuildHeap()
    {
        for (int i = size / 2 - 1; i >= 0; i--)
        {
            SiftDown(i);
        }
    }

    // Проталкивание вверх
    private void SiftUp(int index)
    {
        T element = queue[index];
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (comparator.Compare(element, queue[parentIndex]) >= 0) break;

            queue[index] = queue[parentIndex];
            index = parentIndex;
        }
        queue[index] = element;
    }

    // Проталкивание вниз
    private void SiftDown(int index)
    {
        T elem = queue[index];
        int half = size / 2;

        while (index < half)
        {
            int lchild = 2 * index + 1;
            int rchild = lchild + 1;
            int smallerChild = (rchild < size && comparator.Compare(queue[rchild], queue[lchild]) < 0) ? rchild : lchild;

            if (comparator.Compare(elem, queue[smallerChild]) <= 0) break;

            queue[index] = queue[smallerChild];
            index = smallerChild;
        }
        queue[index] = elem;
    }

    private void RemoveAt(int index)
    {
        if (index == size - 1)
        {
            queue[--size] = default;
        }
        else
        {
            T moved = queue[--size];
            queue[size] = default;
            queue[index] = moved;
            SiftDown(index);

            if (queue[index].Equals(moved))
            {
                SiftUp(index);
            }
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        MyPriorityQueue<int> queue = new MyPriorityQueue<int>();

        queue.Add(5);
        queue.Add(3);
        queue.Add(8);

        Console.WriteLine("Peek: " + queue.Peek()); // 3
        Console.WriteLine("Poll: " + queue.Poll()); // 3
        Console.WriteLine("Peek: " + queue.Peek()); // 5

        queue.Add(2);
        Console.WriteLine("Contains 2: " + queue.Contains(2)); // True
        Console.WriteLine("Size: " + queue.Size()); // 3

        queue.Remove(5);
        Console.WriteLine("Contains 5: " + queue.Contains(5)); // False

        queue.Clear();
        Console.WriteLine("IsEmpty: " + queue.IsEmpty()); // True
    }
}
