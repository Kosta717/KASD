using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab4
{
    public class MyArrayList<T>
    {
        private T[] elementData;
        private int size;

        public MyArrayList()
        {
            elementData = new T[10]; 
            size = 0;
        }

        public MyArrayList(T[] a)
        {
            elementData = new T[a.Length];
            Array.Copy(a, elementData, a.Length);
            size = a.Length;
        }

        public MyArrayList(int capacity)
        {
            elementData = new T[capacity];
            size = 0;
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

        public void AddAll(T[] a)
        {
            foreach (T item in a)
                Add(item);
        }

        public void Clear()
        {
            Array.Clear(elementData, 0, size);
            size = 0;
        }

        public bool Contains(object o)
        {
            if (elementData != null && size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (o.Equals(elementData[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsAll(T[] a)
        {
            foreach (T item in a)
            {
                if (!Contains(item)) return false;
            }
            return true;
        }

        public bool IsEmpty()
        {
            if (size == 0) return true;
            else return false;
        }

        public void Remove(object o)
        {
            if (o == null || elementData == null)
                return;
            if (Contains(o))
            {
                int index = IndexOf(o);
                if (index >= 0)
                {
                    for (int i = index; i < size - 1; i++)
                    {
                        elementData[i]= elementData[i + 1];
                    }
                    elementData[size - 1] = default!;
                    size--;
                    Remove(o);
                }
            }
        }

        public void RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            foreach (T item in a)
                if (item != null)
                    Remove(item);
        }
        public void RetainAll(T[] a)
        {
            if (a== null)
            {
                throw new ArgumentNullException(nameof(a));
            }
            T[] newArray = new T[size];
            int newSize = 0;
            foreach(T item in a)
            {
                for (int i = 0; i<size; i++)
                {
                    if (item != null)
                    {
                        if (item.Equals(elementData[i]))
                        {
                            newArray[newSize]=elementData[i];
                            newSize++;
                        }
                    }
                }
            }
            elementData = newArray;
            size = newSize;
        }

        public int Size()
        {
            return size;
        }

        public object?[] ToArray()
        {
            object?[] a = new object?[size];
            for (int i = 0; i < size; i++)
                if (elementData[i] != null)
                    a[i] = elementData[i];
            return a;
        }

        public object?[] ToArray(T[] a) //для возвращения массива объектов, содержащего все элементы динамического массива.Если аргумент a равен null, то создаётся новыймассив, в который копируются элементы
        {
            if (a == null)
            {
                a = new T[size];
            }
            for (int i = 0; i < size && i < a.Length; i++)
                a[i] = elementData[i];
            return a.Cast<object?>().ToArray(); // Приводим T[] к object?[] и возвращаем
        }

        public void Add(int index, T e) 
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException("index");

            if (elementData == null)
                elementData = new T[1];

            if (size == elementData.Length)
            {
                T[] newArray = new T[size + size / 2 + 1];
                for (int i = 0; i < size; i++)
                    newArray[i] = elementData[i];
                elementData = newArray;
            }

            for (int i = size; i > index; i--)
                elementData[i] = elementData[i - 1];

            elementData[index] = e;
            size++;
        }

        public void AddAll(int index, T[] a) //для добавления элементов в указанную позицию
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            foreach (T item in a)
            {
                Add(index, item);
                index++;
            }
        }

        public T Get(int index)
        {
            if (index < 0 || index >= size) throw new ArgumentOutOfRangeException();
            return elementData[index];
        }

        public int IndexOf(object o)
        {
            for (int i = 0; i < size; i++)
            {
                if (o.Equals(elementData[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(T o)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], o))
                {
                    return i;
                }
            }
            return -1;
        }


        public T RemoveInd(int index)
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException(nameof(index));
            T e = elementData[index];
            if (e != null)
                Remove(e);
            return e;
        }

        public void Set(int index, T e)
        {
            if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));
            if (e == null) throw new ArgumentNullException();
            elementData[index] = e;
        }

        public MyArrayList<T> SubList(int fromIndex, int toIndex) //для возвращения части динамического массива, т.е. элементов в диапазоне [fromIndex; toIndex).
        {
            if (fromIndex > toIndex)
                throw new ArgumentException("fromIndex > toIndex");
            if (fromIndex < 0 || fromIndex >= size)
                throw new ArgumentOutOfRangeException("fromIndex");
            if (toIndex < 0 || toIndex >= size)
                throw new ArgumentOutOfRangeException("toIndex");
            MyArrayList<T> list = new MyArrayList<T>(toIndex - fromIndex);
            for (int i = 0; i < list.size; i++)
                list.Set(i, elementData[i + fromIndex]);
            return list;
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
                Console.WriteLine("Массив пустой");
            }
        }


    }
    internal class Program
    {
        static void Main(string[] args)
        {
            MyArrayList<int> array = new MyArrayList<int>(7);
            array.print();
            int[] a = new int[20];
            Random rand = new Random();
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = rand.Next(0, 100);
            }
            array.AddAll(a);
            array.print();

            int size = array.Size();
            Console.WriteLine(size);
            bool cont = array.Contains(25);
            Console.WriteLine(cont);
            array.Add(3, 25);
            array.Add(2, 25);
            array.print();
            int ind = array.LastIndexOf(25);
            Console.WriteLine(ind);
            array.Remove(25);
            array.print();
            array.Clear();
            array.print();
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = rand.Next(0, 100);
            }
            array.AddAll(a);
            array.print();
            try
            {
                array = array.SubList(4, 12);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            array.print();
            array.RemoveInd(3);
            array.print();
            try
            {
                array.Set(4, 505);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            array.print();
            int w = array.LastIndexOf(505);
            Console.WriteLine(w);
            object t;
            try
            {
                t = array.Get(5);
                Console.WriteLine(t);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            object?[] r = array.ToArray();
            Console.Write("Array of object: ");
            for (int i = 0; i < r.Length; i++)
                Console.Write($"{r[i]} ");
            Console.WriteLine();
            bool emp = array.IsEmpty();
            Console.WriteLine(emp);
            Thread.Sleep(100000);
        }
    }
}
