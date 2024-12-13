using System;
using System.Collections.Generic;

public class MyArrayDeque<T>
{
    private T[] elements;
    private int head;
    private int tail;
    private const int DefaultCapacity = 16;

    public MyArrayDeque() : this(DefaultCapacity) { }

    public MyArrayDeque(T[] a)
    {
        elements = new T[a.Length * 2];
        Array.Copy(a, 0, elements, 0, a.Length);
        head = 0;
        tail = a.Length;
    }

    public MyArrayDeque(int numElements)
    {
        elements = new T[numElements];
        head = 0;
        tail = 0;
    }

    private void EnsureCapacity()
    {
        if ((tail + 1) % elements.Length == head)
        {
            int newCapacity = elements.Length * 2;
            T[] newArray = new T[newCapacity];
            if (head <= tail)
            {
                Array.Copy(elements, head, newArray, 0, tail - head);
            }
            else
            {
                Array.Copy(elements, head, newArray, 0, elements.Length - head);
                Array.Copy(elements, 0, newArray, elements.Length - head, tail);
            }
            elements = newArray;
            head = 0;
            tail = Size();
        }
    }

    public void Add(T e)
    {
        EnsureCapacity();
        elements[tail] = e;
        tail = (tail + 1) % elements.Length;
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
        Array.Clear(elements, 0, elements.Length);
        head = tail = 0;
    }

    public bool Contains(object o)
    {
        foreach (var item in ToArray())
        {
            if (Equals(item, o))
                return true;
        }
        return false;
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

    public bool IsEmpty() => Size() == 0;

    public bool Remove(object o)
    {
        for (int i = 0; i < Size(); i++)
        {
            int index = (head + i) % elements.Length;
            if (Equals(elements[index], o))
            {
                RemoveAt(index);
                return true;
            }
        }
        return false;
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
        var set = new HashSet<T>(a);
        for (int i = 0; i < Size(); i++)
        {
            int index = (head + i) % elements.Length;
            if (!set.Contains(elements[index]))
            {
                RemoveAt(index);
                i--;
            }
        }
    }

    public int Size()
    {
        return (tail - head + elements.Length) % elements.Length;
    }

    public T[] ToArray()
    {
        T[] result = new T[Size()];
        for (int i = 0; i < Size(); i++)
        {
            result[i] = elements[(head + i) % elements.Length];
        }
        return result;
    }

    public T Element()
    {
        if (IsEmpty()) throw new InvalidOperationException("Deque is empty");
        return elements[head];
    }

    public bool Offer(T obj)
    {
        Add(obj);
        return true;
    }

    public T Peek() => IsEmpty() ? default : elements[head];

    public T Poll()
    {
        if (IsEmpty()) return default;
        T result = elements[head];
        elements[head] = default;
        head = (head + 1) % elements.Length;
        return result;
    }

    public void AddFirst(T obj)
    {
        EnsureCapacity();
        head = (head - 1 + elements.Length) % elements.Length;
        elements[head] = obj;
    }

    public void AddLast(T obj) => Add(obj);

    public T GetFirst() => Element();

    public T GetLast()
    {
        if (IsEmpty()) throw new InvalidOperationException("Deque is empty");
        return elements[(tail - 1 + elements.Length) % elements.Length];
    }

    public bool OfferFirst(T obj)
    {
        AddFirst(obj);
        return true;
    }

    public bool OfferLast(T obj) => Offer(obj);

    public T Pop() => Poll();

    public void Push(T obj) => AddFirst(obj);

    public T PeekFirst() => Peek();

    public T PeekLast()
    {
        if (IsEmpty()) return default;
        return elements[(tail - 1 + elements.Length) % elements.Length];
    }

    public T PollFirst() => Poll();

    public T PollLast()
    {
        if (IsEmpty()) return default;
        tail = (tail - 1 + elements.Length) % elements.Length;
        T result = elements[tail];
        elements[tail] = default;
        return result;
    }

    public bool RemoveLastOccurrence(object obj)
    {
        for (int i = Size() - 1; i >= 0; i--)
        {
            int index = (head + i) % elements.Length;
            if (Equals(elements[index], obj))
            {
                RemoveAt(index);
                return true;
            }
        }
        return false;
    }

    public bool RemoveFirstOccurrence(object obj) => Remove(obj);

    private void RemoveAt(int index)
    {
        if (index == head)
        {
            Poll();
        }
        else if (index == (tail - 1 + elements.Length) % elements.Length)
        {
            PollLast();
        }
        else
        {
            for (int i = index; i != tail; i = (i + 1) % elements.Length)
            {
                int nextIndex = (i + 1) % elements.Length;
                elements[i] = elements[nextIndex];
            }
            tail = (tail - 1 + elements.Length) % elements.Length;
        }
    }

}
class Program
{
    static void Main(string[] args)
    {
        MyArrayDeque<int> deque = new MyArrayDeque<int>();
        deque.Add(10);
        deque.Add(20);
        deque.Add(30);
        Console.WriteLine("Добавлены элементы 10, 20, 30");
        Console.WriteLine($"Размер очереди:{deque.Size()}");
        deque.AddFirst(5);
        deque.AddLast(40);
        Console.WriteLine("Добавлены элементы 5 в начало и 40 в конец");
        Console.WriteLine($"Элементы очереди: { string.Join(", ", deque.ToArray())}");

        // Получение первого и последнего элемента
        Console.WriteLine($"Первый элемент: {deque.GetFirst()}");
        Console.WriteLine($"Последний элемент:  {deque.GetLast()}");

        // Удаление элементов
        deque.RemoveFirstOccurrence(10);
        Console.WriteLine("Удалён первый вхождения элемента 10");
        Console.WriteLine($"Элементы очереди: {string.Join(", ", deque.ToArray())}");

        deque.RemoveLastOccurrence(40);
        Console.WriteLine("Удалено последнее вхождение элемента 40");
        Console.WriteLine($"Элементы очереди: {string.Join(", ", deque.ToArray())}");

        // Проверка методов offer, poll, peek
        deque.Offer(50);
        Console.WriteLine("Добавлен элемент 50");
        Console.WriteLine($"Первый элемент (peek): {deque.Peek()}");
        Console.WriteLine($"Размер очереди:{deque.Poll()}");
        Console.WriteLine($"Элементы очереди: {string.Join(", ", deque.ToArray())}");

        // Очистка очереди
        deque.Clear();
        Console.WriteLine("Очередь очищена");
        Console.WriteLine($"Размер пуста?:{deque.IsEmpty()}");
    }
}
