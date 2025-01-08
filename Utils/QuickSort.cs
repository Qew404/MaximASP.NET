public class QuickSort
{
    //Метод для быстрой сортировки
    public static string quickSort(string str)
    {
        if (str.Length <= 1)
            return str;

        //Выбор опорного элемента
        char pivot = str[str.Length / 2];
        //Элемент меньше опорного
        string less = new string(str.Where(c => c < pivot).ToArray());
        //Элемент равен опорному
        string equal = new string(str.Where(c => c == pivot).ToArray());
        //Элементы больше опорного
        string greater = new string(str.Where(c => c > pivot).ToArray());
        //Рекурсивная сортировка и объединение результата
        return quickSort(less) + equal + quickSort(greater);
    }
}
