using System;
using System.IO;

public class MyArrayList<T>
{
    private T[] elementData;
    private int size;

    public MyArrayList()
    {
        elementData = Array.Empty<T>();
        size = 0;
    }

    public MyArrayList(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        elementData = new T[a.Length];
        for (int i = 0; i < a.Length; i++)
            elementData[i] = a[i];
        size = a.Length;
    }

    public void Add(T e)
    {
        if (elementData == null || size == elementData.Length)
        {
            T[] array = new T[elementData.Length == 0 ? 1 : elementData.Length * 2];
            for (int i = 0; i < size; i++)
                array[i] = elementData[i];
            elementData = array;
        }
        elementData[size] = e;
        size++;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= size)
                throw new IndexOutOfRangeException();
            return elementData[index];
        }
    }

    public int Size() => size;
}

class Program
{
    static bool IPAddress(string s)
    {
        string[] parts = s.Split('.');
        if (parts.Length != 4) return false;

        foreach (var part in parts)
        {
            if (!int.TryParse(part, out int num)) return false;
            if (num < 0 || num > 255) return false;
            if (part.Length > 1 && part.StartsWith("0")) return false; // Исключаем ведущие нули
        }
        return true;
    }

    static void Main()
    {
        string[] inputLines = File.ReadAllLines("input.txt");
        MyArrayList<string> inputList = new MyArrayList<string>(inputLines);
        MyArrayList<string> validIps = new MyArrayList<string>();
        for (int i = 0; i < inputList.Size(); i++)
        {
            string line = inputList[i];
            string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (IPAddress(word))
                {
                    validIps.Add(word);
                }
            }
        }
        using (StreamWriter writer = new StreamWriter("output.txt"))
        {
            for (int i = 0; i < validIps.Size(); i++)
            {
                writer.WriteLine(validIps[i]);
            }
        }

        Console.WriteLine("Обработка завершена");
    }
}