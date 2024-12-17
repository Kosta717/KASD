using System.Text.RegularExpressions;

public class MyHashMap<K, V>
{
    private class Entry
    {
        public K Key { get; }
        public V Value { get; set; }
        public Entry? Next { get; set; }

        public Entry(K key, V value)
        {
            Key = key;
            Value = value;
            Next = null;
        }
    }

    private Entry?[] table;
    private int size;
    private float loadFactor;

    public MyHashMap() : this(16, 0.75f) { }
    public MyHashMap(int initialCapacity) : this(initialCapacity, 0.75f) { }
    public MyHashMap(int initialCapacity, float loadFactor)
    {
        if (initialCapacity <= 0 || loadFactor <= 0)
            throw new ArgumentException("Invalid");

        table = new Entry[initialCapacity];
        this.loadFactor = loadFactor;
        size = 0;
    }

    private int GetBucketIndex(K key)
    {
        int hashCode = key?.GetHashCode() ?? 0;
        return Math.Abs(hashCode) % table.Length;
    }

    public bool ContainsKey(K key)
    {
        int index = GetBucketIndex(key);
        Entry? current = table[index];
        while (current != null)
        {
            if (EqualityComparer<K>.Default.Equals(current.Key, key))
                return true;
            current = current.Next;
        }
        return false;
    }

    public void Put(K key, V value)
    {
        int index = GetBucketIndex(key);
        Entry? current = table[index];
        while (current != null)
        {
            if (EqualityComparer<K>.Default.Equals(current.Key, key))
            {
                Console.WriteLine($"Переопределение переменной: {key}");
                return;
            }
            current = current.Next;
        }

        var newEntry = new Entry(key, value) { Next = table[index] };
        table[index] = newEntry;
        size++;

        if (size >= table.Length * loadFactor)
            Resize();
    }

    public ICollection<KeyValuePair<K, V>> EntrySet()
    {
        var entries = new List<KeyValuePair<K, V>>();
        foreach (var bucket in table)
        {
            Entry? current = bucket;
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
        var newTable = new Entry[newCapacity];

        foreach (var bucket in table)
        {
            Entry? current = bucket;
            while (current != null)
            {
                int newIndex = Math.Abs(current.Key?.GetHashCode() ?? 0) % newCapacity;
                var next = current.Next;
                current.Next = newTable[newIndex];
                newTable[newIndex] = current;
                current = next;
            }
        }
        table = newTable;
    }
}

public enum VariableType
{
    Int,
    Float,
    Double
}

class Program
{
    static void Main()
    {
        MyHashMap<string, (VariableType Type, string Value)> variableDefinitions = new MyHashMap<string, (VariableType, string)>();
        string fileContent = File.ReadAllText("input.txt");
        string pattern = @"\s*(int|float|double)\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(\d+)\s*;";
        MatchCollection matches = Regex.Matches(fileContent, pattern);

        foreach (Match match in matches)
        {
            string type = match.Groups[1].Value.ToLower();
            string variableName = match.Groups[2].Value;
            string value = match.Groups[3].Value;

            VariableType varType;
            switch (type)
            {
                case "int":
                    varType = VariableType.Int;
                    break;
                case "float":
                    varType = VariableType.Float;
                    break;
                case "double":
                    varType = VariableType.Double;
                    break;
                default:
                    Console.WriteLine($"Некорректный тип: {type}");
                    continue;
            }

            if (!variableDefinitions.ContainsKey(variableName))
            {
                variableDefinitions.Put(variableName, (varType, value));
                Console.WriteLine($"Добавлено: {type} {variableName} = {value};");
            }
            else
            {
                Console.WriteLine($"Переопределение переменной: {variableName}");
            }
        }

        // Запись результата в output.txt
        using (StreamWriter writer = new StreamWriter("output.txt"))
        {
            foreach (var entry in variableDefinitions.EntrySet())
            {
                string typeName = entry.Value.Type.ToString().ToLower();
                writer.WriteLine($"{typeName} => {entry.Key}({entry.Value.Value})");
            }
        }

        Console.WriteLine("Результаты записаны в файл output.txt.");
    }
}
