using Microsoft.AspNetCore.Mvc;
using AppsettingsValue;
using Microsoft.Extensions.Options;

namespace MyRamdomAPI.Controllers
{
    public class RandomApi

    {
        public class RandomAPI : ControllerBase
        {
            // Поле для хранения конфигурации приложения
            private readonly IConfiguration _configuration;
            // Поле для хранения параметров приложения, загружаемых через IOptions
            private IOptions<AppSettings> appSettings;

            // Конструктор для инициализации конфигурации 
            public RandomAPI(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            // Конструктор для инициализации параметров приложения
            public RandomAPI(IOptions<AppSettings> appSettings)
            {
                this.appSettings = appSettings;
            }

            public async Task<int> GetRandomIndex(int max)
            {
                // Получение URL удаленного API из конфигурации
                var Url = _configuration.GetSection("RemoteApi").GetValue<string>("Url");

                using HttpClient client = new();
                try
                {
                    //Асинхронный запрос к API 
                    var response = await client.GetStringAsync(Url);
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
            public static string RemoveCharacterAtIndex(string str, int index)
            {
                //Удаляет часть строки начиная с заданного индекса длинной в 1 символ
                return str.Remove(index, 1);
            }
        }
    }
}