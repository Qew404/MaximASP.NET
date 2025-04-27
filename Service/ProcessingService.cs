public class ProcessingService
{

    public static List<char> GetInvalidCharList(string input)
    {
        //Список для хранения недопустимых символов
        List<char> invalidChars = new List<char>();
        for (int i = 0; i < input.Length; i++)
        {
            // Получаем символ по индексу
            char c = input[i];
            if (c < 'a' || c > 'z')
            {
                // Добавляем недопустимый символ в список
                invalidChars.Add(c);
            }
        }
        return invalidChars;
    }

    //Метод для обработки входной строки
    public static string ProcessInput(string input)
    {
        //Если четная
        if (input.Length % 2 == 0)
        {
            int mid = input.Length / 2; // Середина строки  
                                        //Разворачиваем половины и объединяем
            return ReverseString(input.Substring(0, mid)) + ReverseString(input.Substring(mid));
        }
        else
        {
            //Не четная. Разворачиваем и добавляем в конец
            return ReverseString(input) + input;
        }
    }

    //Метод для переворота строки
    public static string ReverseString(string str) => new string(str.Reverse().ToArray());

    //Метод для подсчета символов
    public static Dictionary<char, int> countCharacters(string processedString)
    {
        //Грепперуем и считаем символы
        return processedString.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
    }

    //Метод для поиска самой длинной подстройки из гласных
    public static string findLongestVowelSubstring(string str)
    {
        string vowels = "aeiouy";
        string longest = string.Empty;

        //Проходим по всем подстройкам
        for (int start = 0; start < str.Length; start++)
        {
            for (int end = start + 1; end < str.Length; end++)
            {
                //Проверяем что начало и конец гласные
                if (vowels.Contains(str[start]) && vowels.Contains(str[end]))
                {
                    string substring = str.Substring(start, end - start + 1); //Извлекаем подстройку
                                                                              //Если подстройка длиннее текущей обновляем ее
                    if (substring.Length > longest.Length)
                    {
                        longest = substring;
                    }
                }
            }
        }
        //Возвращаем самую длинную подстройку
        return longest;
    }
}