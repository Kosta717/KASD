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
        if (initialCapacity <= 0) throw new ArgumentException("Capacity must be greater than zero.");
        queue = new T[initialCapacity];
        size = 0;
        this.comparator = comparator;
    }

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
            if (!Contains(item)) return false;
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

    public T Element()
    {
        if (IsEmpty()) throw new InvalidOperationException("Queue is empty.");
        return queue[0];
    }

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

    public T Peek()
    {
        return IsEmpty() ? default : queue[0];
    }

    public T Poll()
    {
        if (IsEmpty()) return default;
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

    private void SiftDown(int index)
    {
        T element = queue[index];
        int half = size / 2;
        while (index < half)
        {
            int leftChild = 2 * index + 1;
            int rightChild = leftChild + 1;
            int smallerChild = (rightChild < size && comparator.Compare(queue[rightChild], queue[leftChild]) < 0) ? rightChild : leftChild;

            if (comparator.Compare(element, queue[smallerChild]) <= 0) break;

            queue[index] = queue[smallerChild];
            index = smallerChild;
        }
        queue[index] = element;
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
            if (queue[index].Equals(moved)) SiftUp(index);
        }
    }
}
public class Request : IComparable<Request>
{
    public int Id { get; }
    public int Priority { get; }
    public int Step { get; }

    public Request(int id, int priority, int step)
    {
        Id = id;
        Priority = priority;
        Step = step;
    }

    public int CompareTo(Request other)
    {
        // Приоритет обратный: меньший номер приоритета — выше приоритет
        return Priority.CompareTo(other.Priority);
    }

    public override string ToString()
    {
        return $"Номер: {Id}, Приоритет: {Priority}, Шаги: {Step}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        const int N = 10; // Количество шагов генерации заявок
        Random random = new Random();
        var queue = new MyPriorityQueue<Request>();
        int requestCount = 0; // Счетчик заявок
        Request longWaitRequest = null;
        int maxWaitTime = 0;

        using (StreamWriter log = new StreamWriter("log.txt"))
        {
            for (int step = 1; step <= N; step++)
            {
                int requestsToAdd = random.Next(1, 11); // От 1 до 10 заявок

                for (int i = 0; i < requestsToAdd; i++)
                {
                    int priority = random.Next(1, 6); // Приоритет от 1 до 5
                    var request = new Request(++requestCount, priority, step);
                    queue.Add(request);
                    log.WriteLine($"Добавить {request.Id} {request.Priority} {request.Step}");
                }

                // Удаление заявки с наибольшим приоритетом
                if (!queue.IsEmpty())
                {
                    var removed = queue.Poll();
                    log.WriteLine($"Удалить {removed.Id} {removed.Priority} {removed.Step}");

                    int waitTime = step - removed.Step;
                    if (waitTime > maxWaitTime)
                    {
                        maxWaitTime = waitTime;
                        longWaitRequest = removed;
                    }
                }
            }

            // Удаление оставшихся заявок
            int currentStep = N + 1;
            while (!queue.IsEmpty())
            {
                var removed = queue.Poll();
                log.WriteLine($"Удалить {removed.Id} {removed.Priority} {removed.Step}");

                int waitTime = currentStep - removed.Step;
                if (waitTime > maxWaitTime)
                {
                    maxWaitTime = waitTime;
                    longWaitRequest = removed;
                }

                currentStep++;
            }
        }

        // Вывод информации о заявке с максимальным временем ожидания
        Console.WriteLine("Заявка с максимальным временем ожидания:");
        Console.WriteLine(longWaitRequest);
        Console.WriteLine($"Максимальное время ожидания: {maxWaitTime}");
    }
}

