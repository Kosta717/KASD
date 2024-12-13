using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class MyHashMap<K, V>
{
    private class Entry
    {
        public K Key { get; set; }
        public V Value { get; set; }
        public Entry Next { get; set; }

        public Entry(K key, V value, Entry next = null)
        {
            Key = key;
            Value = value;
            Next = next;
        }
    }

    private Entry[] table;
    private int size;
    private readonly float loadFactor;
    private int threshold;

    public MyHashMap() : this(16, 0.75f) { }

    public MyHashMap(int initialCapacity) : this(initialCapacity, 0.75f) { }

    public MyHashMap(int initialCapacity, float loadFactor)
    {
        if (initialCapacity <= 0 || loadFactor <= 0)
            throw new ArgumentException("Initial capacity and load factor must be positive.");

        table = new Entry[initialCapacity];
        this.loadFactor = loadFactor;
        threshold = (int)(initialCapacity * loadFactor);
    }

    public void Clear()
    {
        table = new Entry[table.Length];
        size = 0;
    }

    public bool ContainsKey(K key)
    {
        int index = GetIndex(key);
        Entry current = table[index];
        while (current != null)
        {
            if (Equals(current.Key, key))
                return true;
            current = current.Next;
        }
        return false;
    }

    public bool ContainsValue(V value)
    {
        foreach (var bucket in table)
        {
            Entry current = bucket;
            while (current != null)
            {
                if (Equals(current.Value, value))
                    return true;
                current = current.Next;
            }
        }
        return false;
    }

    public ICollection<KeyValuePair<K, V>> EntrySet()
    {
        var entries = new List<KeyValuePair<K, V>>();
        foreach (var bucket in table)
        {
            Entry current = bucket;
            while (current != null)
            {
                entries.Add(new KeyValuePair<K, V>(current.Key, current.Value));
                current = current.Next;
            }
        }
        return entries;
    }

    public V Get(K key)
    {
        int index = GetIndex(key);
        Entry current = table[index];
        while (current != null)
        {
            if (Equals(current.Key, key))
                return current.Value;
            current = current.Next;
        }
        return default;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }

    public ICollection<K> KeySet()
    {
        var keys = new List<K>();
        foreach (var bucket in table)
        {
            Entry current = bucket;
            while (current != null)
            {
                keys.Add(current.Key);
                current = current.Next;
            }
        }
        return keys;
    }

    public void Put(K key, V value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int index = GetIndex(key);
        Entry current = table[index];
        while (current != null)
        {
            if (Equals(current.Key, key))
            {
                current.Value = value;
                return;
            }
            current = current.Next;
        }

        table[index] = new Entry(key, value, table[index]);
        size++;

        if (size >= threshold)
            Resize();
    }

    public void Remove(K key)
    {
        int index = GetIndex(key);
        Entry current = table[index];
        Entry prev = null;

        while (current != null)
        {
            if (Equals(current.Key, key))
            {
                if (prev == null)
                    table[index] = current.Next;
                else
                    prev.Next = current.Next;

                size--;
                return;
            }
            prev = current;
            current = current.Next;
        }
    }

    public int Size()
    {
        return size;
    }

    private int GetIndex(K key)
    {
        return Math.Abs(key.GetHashCode()) % table.Length;
    }

    private void Resize()
    {
        int newCapacity = table.Length * 2;
        Entry[] newTable = new Entry[newCapacity];
        threshold = (int)(newCapacity * loadFactor);

        foreach (var bucket in table)
        {
            Entry current = bucket;
            while (current != null)
            {
                Entry next = current.Next;
                int index = Math.Abs(current.Key.GetHashCode()) % newCapacity;
                current.Next = newTable[index];
                newTable[index] = current;
                current = next;
            }
        }

        table = newTable;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        MyHashMap<string, int> map = new MyHashMap<string, int>();

        // Чтение файла и подсчёт тегов
        string filePath = "input.txt";

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            Regex tagRegex = new Regex("<\\/?[a-zA-Z][a-zA-Z0-9]*>", RegexOptions.IgnoreCase);

            foreach (string line in lines)
            {
                MatchCollection matches = tagRegex.Matches(line);
                foreach (Match match in matches)
                {
                    string tag = match.Value.ToLower().Trim('/');

                    if (map.ContainsKey(tag))
                    {
                        map.Put(tag, map.Get(tag) + 1);
                    }
                    else
                    {
                        map.Put(tag, 1);
                    }
                }
            }

            // Вывод результатов
            Console.WriteLine("Подсчёт тегов:");
            foreach (var entry in map.EntrySet())
            {
                Console.WriteLine($"Тег {entry.Key}: {entry.Value} раз(а)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
        }
    }
}
