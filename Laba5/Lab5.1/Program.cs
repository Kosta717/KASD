using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
        elementData = new T[(int)(a.Length)];
        for (int i = 0; i < a.Length; i++)
            elementData[i] = a[i];
        size = a.Length;
    }

    public MyArrayList(int capacity)
    {
        elementData = new T[capacity];
        size = capacity;
    }

    public void Add(T e)
    {

        if (elementData == null)
        {
            elementData = new T[1];
        }
        if (size == elementData.Length)
        {
            T[] array = new T[(int)(elementData.Length * 1.5) + 1];
            for (int i = 0; i < size; i++)
                array[i] = elementData[i];
            elementData = array;
        }
        elementData[size] = e;
        size++;
    }

    public bool Contains(string o)
    {
        if (elementData != null && size != 0)
        {
            for (int i = 0; i < size; i++)
            {
                string? el = elementData[i]?.ToString();
                if (el == null)
                    el = string.Empty;
                if (lowReg(o).Equals(lowReg(el)))
                    return true;
            }
        }
        return false;
    }

    public int Size()
    {
        return size;
    }
    public void print()
    {
        if (size != 0)
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write($"{elementData[i]} ");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Массив пуст");
        }
    }

    private string lowReg(string a)
    {
        return a.ToLower().Replace("/", "");
    }

}
class Program
{
    static void Main()
    {
        StreamReader f = new StreamReader("input.txt");
        string str = f.ReadToEnd();
        string? res = null;
        MyArrayList<string> result = new MyArrayList<string>();
        char start = '<';
        char final = '>';
        int flag;
        for (int i = 0; i < str.Length; i++)
        {
            flag = 0;
            res = null;
            if (str[i] == start)
            {
                if (Char.IsLetter(str[i + 1]) || str[i + 1] == '/')
                {
                    res += "<";
                    res += str[i + 1];
                    int j = i + 2;
                    while (str[j] != final)
                    {
                        if ((!Char.IsLetter(str[j])) && (!Char.IsNumber(str[j])))
                        {
                            flag = 1;
                        }
                        if (flag == 1)
                        {
                            res = null;
                            break;
                        }
                        res += str[j];
                        j++;
                    }
                    if (flag == 0)
                    {
                        res += '>';
                    }
                }
            }
            if (flag == 0 && res != null && !result.Contains(res))
            {
                result.Add(res);
            }
        }
        Console.WriteLine($"Количество слов: {result.Size()}\nСлова:");
        result.print();

    }
}