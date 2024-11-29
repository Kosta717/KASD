using System;
using System.Collections.Generic;

namespace HeapLibrary
{
    internal class Heap<T> where T : IComparable
    {
        private List<T> mas = new List<T>();

        // Построение кучи из массива
        public Heap(T[] array)
        {
            mas.AddRange(array);
            BuildHeap();
        }

        // Метод для восстановления свойств кучи сверху вниз
        private void HeapifyDown(int i)
        {
            int largest = i;
            int l = 2 * i + 1;
            int r = 2 * i + 2;

            // Находим наибольший элемент среди узла и его детей
            if (l < mas.Count && mas[l].CompareTo(mas[largest]) > 0)
            {
                largest = l;
            }

            if (r < mas.Count && mas[r].CompareTo(mas[largest]) > 0)
            {
                largest = r;
            }

            if (largest != i)
            {
                Swap(i, largest);
                HeapifyDown(largest);
            }
        }

        // Метод для восстановления свойств кучи снизу вверх
        private void HeapifyUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;

                if (mas[i].CompareTo(mas[parent]) <= 0)
                {
                    break;
                }

                Swap(i, parent);
                i = parent;
            }
        }

        // Метод для построения кучи
        private void BuildHeap()
        {
            for (int i = mas.Count / 2 - 1; i >= 0; i--)
            {
                HeapifyDown(i);
            }
        }

        // Метод для обмена элементов
        private void Swap(int i, int j)
        {
            T temp = mas[i];
            mas[i] = mas[j];
            mas[j] = temp;
        }

        // Метод для получения максимального элемента
        public T Max()
        {
            if (mas.Count == 0)
                throw new InvalidOperationException("Куча пуста.");
            return mas[0];
        }

        // Метод для удаления и возврата максимального элемента
        public T RemoveMax()
        {
            if (mas.Count == 0)
                throw new InvalidOperationException("Куча пуста.");

            T max = mas[0];
            mas[0] = mas[^1];
            mas.RemoveAt(mas.Count - 1);
            HeapifyDown(0);
            return max;
        }

        // Метод для изменения значения элемента
        public void Change(int index, T a)
        {
            if (index < 0 || index >= mas.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (a.CompareTo(mas[index]) > 0)
            {
                mas[index] = a;
                HeapifyUp(index); // Восстанавливаем свойства кучи
            }
            else
            {
                throw new ArgumentException("Новое значение меньше текущего.");
            }
        }

        // Метод для добавления нового элемента
        public void Add(T a)
        {
            mas.Add(a);
            HeapifyUp(mas.Count - 1);
        }

        // Метод для слияния двух куч
        public void Merge(Heap<T> other)
        {
            mas.AddRange(other.mas);
            BuildHeap();
        }

        // Вывод элементов кучи
        public void Output()
        {
            Console.WriteLine(string.Join(", ", mas));
        }
    }
    class Program
    {
        static void Main()
        {
            // Создаём кучу из массива
            int[] elem = { 10, 20, 5, 6, 1, 8, 9 };
            Heap<int> heap = new Heap<int>(elem);

            Console.WriteLine("Начальная куча:");
            heap.Output();

            Console.WriteLine($"Максимум: {heap.Max()}");

            Console.WriteLine($"Удаляем максимум: {heap.RemoveMax()}");
            heap.Output();

            Console.WriteLine("Добавляем элемент 25:");
            heap.Add(25);
            heap.Output();

            Console.WriteLine("Увеличиваем значение элемента на индексе 2 до 30:");
            heap.Change(2, 30);
            heap.Output();

            Console.WriteLine("Слияние с другой кучей:");
            int[] otherElem = { 15, 12, 18 };
            Heap<int> otherHeap = new Heap<int>(otherElem);
            heap.Merge(otherHeap);
            heap.Output();
        }
    }
}