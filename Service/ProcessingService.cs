public class ProcessingService
{

    public static List<char> getInvalidCharList(string input)
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
    public static string processInput(string input)
    {
        //Если четная
        if (input.Length % 2 == 0)
        {
            int mid = input.Length / 2; // Середина строки  
                                        //Разворачиваем половины и объединяем
            return reverseString(input.Substring(0, mid)) + reverseString(input.Substring(mid));
        }
        else
        {
            //Не четная. Разворачиваем и добавляем в конец
            return reverseString(input) + input;
        }
    }

    //Метод для переворота строки
    public static string reverseString(string str) => new string(str.Reverse().ToArray());

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

    //Метод для получения случайного числа
    public static async Task<int> getRandomIndex(int max)
    {
        using HttpClient client = new();
        string url = $"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={max - 1}&count=1";
        try
        {
            //Асинхронный запрос к API 
            var response = await client.GetStringAsync(url);
            //Убираем из ответа скобки и разбиваем в массив чисел
            var numbers = response.Trim('[', ']').Split(',');
            //Парсим первое число из массива и возращаем его как целое
            return int.Parse(numbers[0]);
        }
        catch (Exception)
        {
            //Если API недоступен, то генерируем случайное число
            Random random = new Random();
            //Возвращаем случайное число от 0 до конца строки
            return random.Next(0, max);
        }
    }
    //Метод для удаления символа по индексу
    public static string removeCharacterAtIndex(string str, int index)
    {
        //Удаляет часть строки начиная с заданного индекса длинной в 1 символ
        return str.Remove(index, 1);
    }
}