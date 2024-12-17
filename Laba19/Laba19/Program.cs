using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class MyHashMap<K, V>
{
    private class Entry
    {
        public K Key { get; }
        public V Value { get; set; }
        public Entry Next { get; set; }

        public Entry(K key, V value)
        {
            Key = key;
            Value = value;
            Next = null;
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
        {
            throw new ArgumentException("Initial capacity and load factor must be positive.");
        }

        this.table = new Entry[initialCapacity];
        this.loadFactor = loadFactor;
        this.threshold = (int)(initialCapacity * loadFactor);
        this.size = 0;
    }

    private int GetIndex(K key)
    {
        return Math.Abs(key.GetHashCode()) % table.Length;
    }

    public void Put(K key, V value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        int index = GetIndex(key);
        Entry current = table[index];

        while (current != null)
        {
            if (current.Key.Equals(key))
            {
                current.Value = value;
                return;
            }
            current = current.Next;
        }

        Entry newEntry = new Entry(key, value);
        newEntry.Next = table[index];
        table[index] = newEntry;
        size++;

        if (size >= threshold)
        {
            Resize();
        }
    }

    public V Get(K key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        int index = GetIndex(key);
        Entry current = table[index];

        while (current != null)
        {
            if (current.Key.Equals(key))
            {
                return current.Value;
            }
            current = current.Next;
        }

        return default;
    }

    public bool ContainsKey(K key)
    {
        return Get(key) != null;
    }

    public bool ContainsValue(V value)
    {
        foreach (Entry bucket in table)
        {
            Entry current = bucket;
            while (current != null)
            {
                if (EqualityComparer<V>.Default.Equals(current.Value, value))
                {
                    return true;
                }
                current = current.Next;
            }
        }

        return false;
    }

    public void Remove(K key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        int index = GetIndex(key);
        Entry current = table[index];
        Entry prev = null;

        while (current != null)
        {
            if (current.Key.Equals(key))
            {
                if (prev == null)
                {
                    table[index] = current.Next;
                }
                else
                {
                    prev.Next = current.Next;
                }

                size--;
                return;
            }
            prev = current;
            current = current.Next;
        }
    }

    public void Clear()
    {
        table = new Entry[table.Length];
        size = 0;
    }

    public int Size()
    {
        return size;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }

    public ICollection<K> KeySet()
    {
        var keys = new List<K>();

        foreach (Entry bucket in table)
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

    public ICollection<KeyValuePair<K, V>> EntrySet()
    {
        var entries = new List<KeyValuePair<K, V>>();

        foreach (Entry bucket in table)
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

    private void Resize()
    {
        int newCapacity = table.Length * 2;
        Entry[] newTable = new Entry[newCapacity];
        threshold = (int)(newCapacity * loadFactor);

        foreach (Entry bucket in table)
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

        string filePath = "input.txt";

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            MyHashMap<string, int> tagCounts = new MyHashMap<string, int>();
            string pattern = @"<\/?[a-zA-Z][a-zA-Z0-9]*>";
            foreach (var line in File.ReadLines("input.txt"))
            {
                string cleanedLine = line.Replace(" ", "");
                MatchCollection matches = Regex.Matches(cleanedLine, pattern);
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
            Console.WriteLine("Теги:");
            foreach (var entry in map.EntrySet())
            {
                Console.WriteLine($"Тег {entry.Key}: {entry.Value} раз");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
        }
    }
}
