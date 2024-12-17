using System;
using System.Collections.Generic;

public class MyTreeMap<K, V> where K : IComparable<K>
{
    private class TreeNode
    {
        public K Key { get; set; }
        public V Value { get; set; }
        public TreeNode Left { get; set; } 
        public TreeNode Right { get; set; } 

        public TreeNode(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }

    private readonly IComparer<K> _comparator; 
    private TreeNode root; 
    private int size;
    public MyTreeMap() : this(null) { }

    // Конструктор с указанным компаратором
    public MyTreeMap(IComparer<K> comparator)
    {
        _comparator = comparator ?? Comparer<K>.Default;
    }
    public void Clear()
    {
        root = null;
        size = 0;
    }
    public bool ContainsKey(K key)
    {
        return FindNode(key) != null;
    }
    public bool ContainsValue(V value)
    {
        return ContainsValue(root, value);
    }

    private bool ContainsValue(TreeNode node, V value)
    {
        if (node == null) return false;
        if (EqualityComparer<V>.Default.Equals(node.Value, value)) return true;
        return ContainsValue(node.Left, value) || ContainsValue(node.Right, value);
    }

    public V Get(K key)
    {
        var node = FindNode(key);
        return node != null ? node.Value : default;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }

    public int Size()
    {
        return size;
    }

    public void Put(K key, V value)
    {
        root = Insert(root, key, value);
    }

    private TreeNode Insert(TreeNode node, K key, V value)
    {
        if (node == null)
        {
            size++;
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

    public bool Remove(K key)
    {
        int initialSize = size;
        root = Remove(root, key);
        return size < initialSize;
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
            size--;

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
        TreeNode current = root;
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

    public K FirstKey()
    {
        if (root == null) throw new InvalidOperationException("Дерево пусто.");
        return FindMin(root).Key;
    }

    public K LastKey()
    {
        if (root == null) throw new InvalidOperationException("Дерево пусто.");
        TreeNode current = root;
        while (current.Right != null)
        {
            current = current.Right;
        }
        return current.Key;
    }

    public override string ToString()
    {
        var entries = new List<string>();
        InOrderTraversal(root, entries);
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
