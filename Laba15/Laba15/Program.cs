using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASD15
{

    class MyArrayDeque<T>
    {
        private T[] elements;
        private int head;
        private int tail;
        public MyArrayDeque()
        {
            elements = new T[16];
            head = 0;
            tail = -1;
        }
        public MyArrayDeque(T[] A)
        {
            elements = new T[A.Length];
            for (int i = 0; i < A.Length; i++)
                elements[i] = A[i];
            head = 0;
            tail = A.Length - 1;
        }
        public MyArrayDeque(int numElements)
        {
            elements = new T[numElements];
            head = 0;
            tail = -1;
        }
        public void Add(T element)
        {
            if (tail + 1 < elements.Length)
            {
                tail++;
                elements[tail] = element;
                return;
            }
            if (Size() < elements.Length)
            {
                head--;
                for (int i = head; i < tail; i++)
                    elements[i] = elements[i + 1];
                elements[tail] = element;
                return;
            }
            T[] newElements = new T[2 * (elements.Length + 1)];
            for (int i = head; i <= tail; i++)
                newElements[i] = elements[i];
            tail++;
            newElements[tail] = element;
            elements = newElements;
        }
        public void AddAll(T[] A)
        {
            for (int i = 0; i < A.Length; i++)
                Add(A[i]);
        }
        public void Clear()
        {
            head = 0;
            tail = -1;
        }
        public bool Contains(object obj)
        {
            for (int i = head; i <= tail; i++)
                if (Equals(obj, elements[i]))
                    return true;
            return false;
        }
        public bool ContainsAll(T[] A)
        {
            bool flag;
            for (int i = 0; i < A.Length; i++)
            {
                flag = false;
                for (int j = head; j <= tail; j++)
                    if (Equals(A[i], elements[j]))
                        flag = true;
                if (!flag)
                    return false;
            }
            return true;
        }
        public bool IsEmpty()
        {
            return tail < head;
        }
        public void Remove(object obj)
        {
            for (int i = head; i <= tail; i++)
                if (Equals(obj, elements[i]))
                {
                    for (int j = i; j < tail; j++)
                        elements[j] = elements[j + 1];
                    tail--;
                    i--;
                }
        }
        public void RemoveAll(T[] A)
        {
            for (int i = 0; i < A.Length; i++)
                Remove(A[i]);
        }
        public void RetainAll(T[] A)
        {
            bool flag;
            for (int i = head; i <= tail; i++)
            {
                flag = false;
                for (int j = 0; j < A.Length; j++)
                    if (Equals(elements[i], A[j]))
                        flag = true;
                if (!flag)
                    Remove(A[i]);
            }
        }
        public int Size()
        {
            return tail - head + 1;
        }
        public T[] ToArray()
        {
            T[] A = new T[Size()];
            int index = 0;
            for (int i = head; i <= tail; i++)
            {
                A[index] = elements[i];
                index++;
            }
            return A;
        }
        public void ToArray(ref T[] A)
        {
            if (A == null)
            {
                A = ToArray();
                return;
            }
            int index = 0;
            if (A.Length == Size())
            {
                for (int i = head; i <= tail; i++)
                {
                    A[index] = elements[i];
                    index++;
                }
                return;
            }
            A = new T[Size()];
            for (int i = head; i <= tail; i++)
            {
                A[index] = elements[i];
                index++;
            }
        }
        public T Element()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return elements[head];
        }
        private int Amount(T element)
        {
            int amount = 0;
            for (int i = head; i <= tail; i++)
                if (Equals(element, elements[i]))
                    amount++;
            return amount;
        }
        public bool Offer(T element)
        {
            int oldAmount = Amount(element);
            Add(element);
            int newAmount = Amount(element);
            if (oldAmount != newAmount)
                return true;
            return false;
        }
        public T Peek()
        {
            if (Size() == 0)
                return default;
            return elements[head];
        }
        public T Poll()
        {
            if (Size() == 0)
                return default;
            head++;
            return elements[head - 1];
        }
        public void AddFirst(T element)
        {
            if (head - 1 >= 0)
            {
                head--;
                elements[head] = element;
                return;
            }
            if (Size() < elements.Length)
            {
                tail++;
                for (int i = tail; i > head; i--)
                    elements[i] = elements[i - 1];
                elements[head] = element;
                return;
            }
            T[] newElements = new T[2 * (elements.Length + 1)];
            for (int i = head; i <= tail; i++)
                newElements[i + 1] = elements[i];
            newElements[head] = element;
            elements = newElements;
        }
        public void AddLast(T element)
        {
            Add(element);
        }
        public T GetFirst()
        {
            return Element();
        }
        public T GetLast()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return elements[tail];
        }
        public bool OfferFirst(T element)
        {
            if (Size() == elements.Length)
                return false;
            AddFirst(element);
            return true;
        }
        public bool OfferLast(T element)
        {
            if (Size() == elements.Length)
                return false;
            AddLast(element);
            return true;
        }
        public T Pop()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return Poll();
        }
        public void Push(T element)
        {
            AddFirst(element);
        }
        public T PeekFirst()
        {
            return Peek();
        }
        public T PeekLast()
        {
            if (Size() == 0)
                return default;
            return elements[tail];
        }
        public T PollFirst()
        {
            return Poll();
        }
        public T PollLast()
        {
            if (Size() == 0)
                return default;
            tail--;
            return elements[tail + 1];
        }
        public T RemoveFirst()
        {
            return Pop();
        }
        public T RemoveLast()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            tail--;
            return elements[tail + 1];
        }
        public bool RemoveFirstOccurance(object obj)
        {
            for (int i = head; i <= tail; i++)
                if (Equals(obj, elements[i]))
                {
                    for (int j = i; j < tail; j++)
                        elements[j] = elements[j + 1];
                    tail--;
                    return true;
                }
            return false;
        }
        public bool RemoveLastOccurance(object obj)
        {
            for (int i = tail; i >= head; i--)
                if (Equals(obj, elements[i]))
                {
                    for (int j = i; j < tail; j++)
                        elements[j] = elements[j + 1];
                    tail--;
                    return true;
                }
            return false;
        }
        public void Print()
        {
            for (int i = head; i <= tail; i++)
                Console.WriteLine(elements[i]);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите число n: ");
                int n = int.Parse(Console.ReadLine());
                const string f1 = "input.txt";
                const string f2 = "sorted.txt";
                StreamReader input = new StreamReader(f1);
                MyArrayDeque<string> deque = new MyArrayDeque<string>();
                MyArrayDeque<char> digits = new MyArrayDeque<char>();
                for (int i = (int)'0'; i <= (int)'9'; i++)
                    digits.Push((char)i);
                string line;
                int firstCount = 0;
                if (!input.EndOfStream)
                {
                    line = input.ReadLine();
                    deque.Push(line);
                    firstCount = Count(line, digits);
                }
                int digitsCount;
                while (!input.EndOfStream)
                {
                    line = input.ReadLine();
                    digitsCount = Count(line, digits);
                    if (digitsCount > firstCount)
                        deque.AddLast(line);
                    else
                    {
                        deque.AddFirst(line);
                        firstCount = digitsCount;
                    }
                }
                StreamWriter sorted = new StreamWriter(f2);
                string[] D = deque.ToArray();
                for (int i = 0; i < D.Length; i++)
                    sorted.Write(D[i] + "\n");
                for (int i = 0; i < D.Length; i++)
                    if (Count(D[i], ' ') > n)
                        deque.Remove(D[i]);
                deque.Print();
                input.Close();
                sorted.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static int Count(string line, MyArrayDeque<char> alphabet)
        {
            int digitsCount = 0;
            for (int i = 0; i < line.Length; i++)
                if (alphabet.Contains(line[i]))
                    digitsCount++;
            return digitsCount;
        }
        static int Count(string line, char symbol)
        {
            int digitsCount = 0;
            for (int i = 0; i < line.Length; i++)
                if (line[i] == symbol)
                    digitsCount++;
            return digitsCount;
        }
    }
}