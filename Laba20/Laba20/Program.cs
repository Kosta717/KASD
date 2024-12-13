using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// Перечисление для типов переменных
public enum VariableType
{
    Int,
    Float,
    Double
}

// Класс, представляющий значение переменной
public class VariableValue
{
    public VariableType Type { get; set; }
    public string Value { get; set; }
}

// Собственная реализация хеш-таблицы
public class MyHashMap
{
    private const int Size = 1024;
    private readonly LinkedList<KeyValuePair<string, VariableValue>>[] buckets;

    public MyHashMap()
    {
        buckets = new LinkedList<KeyValuePair<string, VariableValue>>[Size];
    }

    private int GetBucketIndex(string key)
    {
        return Math.Abs(key.GetHashCode()) % Size;
    }

    public bool TryAdd(string key, VariableValue value)
    {
        int index = GetBucketIndex(key);
        if (buckets[index] == null)
        {
            buckets[index] = new LinkedList<KeyValuePair<string, VariableValue>>();
        }

        foreach (var pair in buckets[index])
        {
            if (pair.Key == key)
            {
                return false; // Переопределение переменной
            }
        }

        buckets[index].AddLast(new KeyValuePair<string, VariableValue>(key, value));
        return true;
    }

    public IEnumerable<KeyValuePair<string, VariableValue>> GetAll()
    {
        foreach (var bucket in buckets)
        {
            if (bucket != null)
            {
                foreach (var pair in bucket)
                {
                    yield return pair;
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFile = "input.txt";
        string outputFile = "output.txt";

        // Регулярное выражение для определения переменных
        string pattern = @"^(int|float|double)\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(\d+);$";
        Regex regex = new Regex(pattern);

        var hashMap = new MyHashMap();
        var errors = new List<string>();
        var redefinitions = new List<string>();

        // Чтение файла
        string[] lines = File.ReadAllLines(inputFile);
        string currentDefinition = "";

        foreach (var line in lines)
        {
            if (line.Trim().EndsWith(";"))
            {
                currentDefinition += line.Trim();
                Match match = regex.Match(currentDefinition);

                if (match.Success)
                {
                    string typeStr = match.Groups[1].Value;
                    string name = match.Groups[2].Value;
                    string value = match.Groups[3].Value;

                    if (Enum.TryParse(typeStr, true, out VariableType type))
                    {
                        var variable = new VariableValue { Type = type, Value = value };

                        if (!hashMap.TryAdd(name, variable))
                        {
                            redefinitions.Add(name);
                        }
                    }
                    else
                    {
                        errors.Add(currentDefinition);
                    }
                }
                else
                {
                    errors.Add(currentDefinition);
                }

                currentDefinition = ""; // Сброс текущего определения
            }
            else
            {
                currentDefinition += line.Trim();
            }
        }

        // Запись результата в файл
        using (var writer = new StreamWriter(outputFile))
        {
            foreach (var pair in hashMap.GetAll())
            {
                writer.WriteLine($"{pair.Value.Type} => {pair.Key}({pair.Value.Value})");
            }

            if (errors.Count > 0)
            {
                writer.WriteLine("\nНекорректные определения:");
                foreach (var error in errors)
                {
                    writer.WriteLine(error);
                }
            }

            if (redefinitions.Count > 0)
            {
                writer.WriteLine("\nПереопределения переменных:");
                foreach (var name in redefinitions)
                {
                    writer.WriteLine(name);
                }
            }
        }

        Console.WriteLine("Результат записан в файл.");
    }
}
