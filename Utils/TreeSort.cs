public class TreeSort
{
    //Метод сортировки деревом
    public static string _TreeSort(char[] array)
    {
        TreeNode? root = null;
        // Вставка в дерево элимента
        foreach (char value in array)
        {
            root = insert(root, value);
        }

        // Отсортированные элименты
        List<char> sortedList = new List<char>();
        inOrderTraversal(root, sortedList);

        return new string(sortedList.ToArray());
    }

    // Метод для вставки элемента в дерево
    static TreeNode insert(TreeNode node, char value)
    {
        if (node == null)
        {
            return new TreeNode(value); // Создаем новый узел
        }

        if (value < node.Value)
        {
            node.Left = insert(node.Left, value); // Вставляем в левое поддерево
        }
        else
        {
            node.Right = insert(node.Right, value); // Вставляем в правое поддерево
        }

        return node; // Возвращаем текущий узел
    }

    // Метод для обхода дерева в порядке возрастания
    static void inOrderTraversal(TreeNode node, List<char> sortedList)
    {
        if (node != null)
        {
            inOrderTraversal(node.Left, sortedList); // Обход левого поддерева
            sortedList.Add(node.Value); // Добавляем текущий узел
            inOrderTraversal(node.Right, sortedList); // Обход правого поддерева
        }
    }

}