using AppsettingsValue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MyBlackList.Controllers
{
    public class BlackList : ControllerBase
    {
        // Поле для хранения конфигурации приложения
        private readonly IConfiguration _configuration;
        // Поле для хранения параметров приложения, загружаемых через IOptions
        private IOptions<AppSettings> appSettings;

        // Конструктор для инициализации конфигурации 
        public BlackList(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // Метод для обработки GET-запросов
        [HttpGet]
        public List<string> CheckBlackListWords(string input)
        {
            var blackListFromConfig = _configuration.GetSection("Log:Settings").GetValue<string>("BlackList");
            // Проверка на наличие черного списка в конфигурации
            if (string.IsNullOrEmpty(blackListFromConfig))
            {
                return new List<string>();
            }
            // Разделяем строку черного списка на отдельные слова, убираем пробелы и приводим к нижнему регистру
            var blackList = blackListFromConfig.Split(',').Select(w => w.Trim().ToLower()).ToList();

            // Проверяем входную строку на наличие черных слов
            var BlackWords = blackList
                .Where(word => input.ToLower().IndexOf(word) >= 0)
                .ToList();

            // Возвращаем список найденных черных слов
            return BlackWords;
        }
    }
}