// Реализация класса MyTreeMap
// Этот класс представляет собой дерево поиска для хранения пар "ключ-значение" в строгом порядке
using System;
using System.Collections.Generic;

public class MyTreeMap<K, V> where K : IComparable<K>
{
    // Вложенный класс для представления узлов дерева
    private class TreeNode
    {
        public K Key { get; set; } // Ключ
        public V Value { get; set; } // Значение
        public TreeNode Left { get; set; } // Левый дочерний узел
        public TreeNode Right { get; set; } // Правый дочерний узел

        public TreeNode(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }

    private readonly IComparer<K> _comparator; // Компаратор для сравнения ключей
    private TreeNode _root; // Корневой узел дерева
    private int _size; // Количество элементов в дереве

    // Конструктор по умолчанию (с использованием естественного порядка сортировки)
    public MyTreeMap() : this(null) { }

    // Конструктор с указанным компаратором
    public MyTreeMap(IComparer<K> comparator)
    {
        _comparator = comparator ?? Comparer<K>.Default;
    }

    // Очистить дерево
    public void Clear()
    {
        _root = null;
        _size = 0;
    }

    // Проверка, содержит ли дерево указанный ключ
    public bool ContainsKey(K key)
    {
        return FindNode(key) != null;
    }

    // Проверка, содержит ли дерево указанное значение
    public bool ContainsValue(V value)
    {
        return ContainsValue(_root, value);
    }

    private bool ContainsValue(TreeNode node, V value)
    {
        if (node == null) return false;
        if (EqualityComparer<V>.Default.Equals(node.Value, value)) return true;
        return ContainsValue(node.Left, value) || ContainsValue(node.Right, value);
    }

    // Получить значение по ключу
    public V Get(K key)
    {
        var node = FindNode(key);
        return node != null ? node.Value : default;
    }

    // Проверить, пусто ли дерево
    public bool IsEmpty()
    {
        return _size == 0;
    }

    // Получить количество элементов в дереве
    public int Size()
    {
        return _size;
    }

    // Добавить пару "ключ-значение" в дерево
    public void Put(K key, V value)
    {
        _root = Insert(_root, key, value);
    }

    private TreeNode Insert(TreeNode node, K key, V value)
    {
        if (node == null)
        {
            _size++;
            return new TreeNode(key, value);
        }

        int compare = _comparator.Compare(key, node.Key);
        if (compare < 0)
            node.Left = Insert(node.Left, key, value);
        else if (compare > 0)
            node.Right = Insert(node.Right, key, value);
        else
            node.Value = value;

        return node;
    }

    // Удалить пару "ключ-значение" по ключу
    public bool Remove(K key)
    {
        int initialSize = _size;
        _root = Remove(_root, key);
        return _size < initialSize;
    }

    private TreeNode Remove(TreeNode node, K key)
    {
        if (node == null) return null;

        int compare = _comparator.Compare(key, node.Key);
        if (compare < 0)
            node.Left = Remove(node.Left, key);
        else if (compare > 0)
            node.Right = Remove(node.Right, key);
        else
        {
            _size--;

            if (node.Left == null) return node.Right;
            if (node.Right == null) return node.Left;

            TreeNode successor = FindMin(node.Right);
            node.Key = successor.Key;
            node.Value = successor.Value;
            node.Right = Remove(node.Right, successor.Key);
        }

        return node;
    }

    private TreeNode FindNode(K key)
    {
        TreeNode current = _root;
        while (current != null)
        {
            int compare = _comparator.Compare(key, current.Key);
            if (compare == 0) return current;
            current = compare < 0 ? current.Left : current.Right;
        }
        return null;
    }

    private TreeNode FindMin(TreeNode node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    // Получить первый ключ в дереве (минимальный)
    public K FirstKey()
    {
        if (_root == null) throw new InvalidOperationException("Дерево пусто.");
        return FindMin(_root).Key;
    }

    // Получить последний ключ в дереве (максимальный)
    public K LastKey()
    {
        if (_root == null) throw new InvalidOperationException("Дерево пусто.");
        TreeNode current = _root;
        while (current.Right != null)
        {
            current = current.Right;
        }
        return current.Key;
    }

    // Представление дерева в строковом виде (для отладки)
    public override string ToString()
    {
        var entries = new List<string>();
        InOrderTraversal(_root, entries);
        return string.Join(", ", entries);
    }

    private void InOrderTraversal(TreeNode node, List<string> entries)
    {
        if (node == null) return;
        InOrderTraversal(node.Left, entries);
        entries.Add($"[{node.Key}: {node.Value}]");
        InOrderTraversal(node.Right, entries);
    }
}

// Пример использования MyTreeMap
public class Program
{
    public static void Main()
    {
        var map = new MyTreeMap<int, string>();

        map.Put(5, "Five");
        map.Put(2, "Two");
        map.Put(8, "Eight");
        map.Put(1, "One");
        map.Put(3, "Three");

        Console.WriteLine("Содержимое TreeMap: " + map);

        Console.WriteLine("Содержит ключ 2: " + map.ContainsKey(2));
        Console.WriteLine("Содержит значение 'Three': " + map.ContainsValue("Three"));

        Console.WriteLine("Значение для ключа 5: " + map.Get(5));
        Console.WriteLine("Первый ключ: " + map.FirstKey());
        Console.WriteLine("Последний ключ: " + map.LastKey());

        map.Remove(2);
        Console.WriteLine("После удаления ключа 2: " + map);

        Console.WriteLine("Размер TreeMap: " + map.Size());

        map.Clear();
        Console.WriteLine("После очистки: " + (map.IsEmpty() ? "TreeMap пуст" : "TreeMap не пуст"));
    }
}
