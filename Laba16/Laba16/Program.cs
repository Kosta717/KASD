using System;
using System.Collections.Generic;

public class MyLinkedList<T>
{
    private class Node
    {
        public T Value;
        public Node Prev;
        public Node Next;

        public Node(T value)
        {
            Value = value;
        }
    }

    private Node first;
    private Node last;
    private int size;

    public MyLinkedList()
    {
        first = null;
        last = null;
        size = 0;
    }

    public MyLinkedList(T[] array) : this()
    {
        AddAll(array);
    }

    public void Add(T value)
    {
        var newNode = new Node(value);
        if (last == null)
        {
            first = last = newNode;
        }
        else
        {
            last.Next = newNode;
            newNode.Prev = last;
            last = newNode;
        }
        size++;
    }

    public void AddAll(T[] array)
    {
        foreach (var item in array)
        {
            Add(item);
        }
    }

    public void Clear()
    {
        first = null;
        last = null;
        size = 0;
    }

    public bool Contains(T value)
    {
        for (var current = first; current != null; current = current.Next)
        {
            if (Equals(current.Value, value))
                return true;
        }
        return false;
    }

    public bool ContainsAll(T[] array)
    {
        foreach (var item in array)
        {
            if (!Contains(item))
                return false;
        }
        return true;
    }

    public bool IsEmpty() => size == 0;

    public bool Remove(T value)
    {
        for (var current = first; current != null; current = current.Next)
        {
            if (Equals(current.Value, value))
            {
                if (current.Prev != null)
                    current.Prev.Next = current.Next;
                else
                    first = current.Next;

                if (current.Next != null)
                    current.Next.Prev = current.Prev;
                else
                    last = current.Prev;

                size--;
                return true;
            }
        }
        return false;
    }

    public void RemoveAll(T[] array)
    {
        foreach (var item in array)
        {
            Remove(item);
        }
    }

    public int Size() => size;

    public T[] ToArray()
    {
        var result = new T[size];
        var current = first;
        for (int i = 0; i < size; i++)
        {
            result[i] = current.Value;
            current = current.Next;
        }
        return result;
    }

    public override string ToString()
    {
        var result = "[";
        var current = first;
        while (current != null)
        {
            result += current.Value;
            if (current.Next != null)
                result += ", ";
            current = current.Next;
        }
        return result + "]";
    }

    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));

        var current = first;
        for (int i = 0; i < index; i++)
        {
            current = current.Next;
        }
        return current.Value;
    }

    public void Add(int index, T value)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));

        var newNode = new Node(value);

        if (index == 0)
        {
            newNode.Next = first;
            if (first != null)
                first.Prev = newNode;
            first = newNode;
            if (last == null)
                last = newNode;
        }
        else if (index == size)
        {
            Add(value);
            return;
        }
        else
        {
            var current = first;
            for (int i = 0; i < index; i++)
                current = current.Next;

            newNode.Next = current;
            newNode.Prev = current.Prev;
            current.Prev.Next = newNode;
            current.Prev = newNode;
        }
        size++;
    }

    public void AddAll(int index, T[] array)
    {
        foreach (var item in array)
        {
            Add(index++, item);
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        var list = new MyLinkedList<int>();

        list.Add(10);
        list.Add(20);
        list.Add(30);
        Console.WriteLine($"Список после добавления элементов: {list}");
        list.Add(1, 15);
        Console.WriteLine($"Список после добавления 15 на позицию 1: {list}");
        Console.WriteLine($"Содержит ли список элемент 20? {list.Contains(20)}");
        list.Remove(20);
        Console.WriteLine($"Список после удаления элемента 20: {list}");
        Console.WriteLine("Элементы списка в виде массива:");
        foreach (var item in list.ToArray())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
        list.Clear();
        Console.WriteLine($"Список после очистки: {list}");
    }
}
