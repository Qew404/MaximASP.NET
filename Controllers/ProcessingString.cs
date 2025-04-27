using Microsoft.AspNetCore.Mvc;
using MyBlackList.Controllers;
using static MyRamdomAPI.Controllers.RandomApi;

namespace MaximNET.Controllers
{
    //Указывает, что данный класс является контроллером API
    [ApiController]
    [Route("api/[controller]")]
    public class StringProcessingController : ControllerBase
    {
        // Конфигурация приложения для доступа к настройкам
        private readonly IConfiguration _configuration;
        // HTTP клиент для выполнения запросов
        private readonly HttpClient _httpClient;

        // Конструктор контроллера, принимает конфигурацию и фабрику HTTP клиентов
        public StringProcessingController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration; // Инициализация конфигурации
            _httpClient = httpClientFactory.CreateClient(); // Создание HTTP клиента
        }

        //Обрабатывает GET запросы
        [HttpGet]
        public async Task<ActionResult<Response>> processString([FromQuery] string input, [FromQuery] string sortMethod = "quick")
        {
            // Проверка на пустую строку
            if (string.IsNullOrWhiteSpace(input))
            {
                return BadRequest(new { error = "Пустая строка" });
            }

            //Проверка на черный список 
            var blackListController = new BlackList(_configuration);
            // Поиск слов из черного списка в входной строке
            var BlackWords = blackListController.CheckBlackListWords(input);
            if (BlackWords.Any())
            {
                // Возврат ошибки с найденными словами
                return BadRequest($"Слова из черного списка: {string.Join(", ", BlackWords)}");
            }

            var randomController = new RandomAPI(_configuration);
            var randomIndexApi = randomController.GetRandomIndex;

            //Если найдены недопустимые символы, возвращаем ошибку
            var invalidChars = ProcessingService.GetInvalidCharList(input);
            if (invalidChars.Count > 0)
            {
                return BadRequest(new { error = "Неподходящие символы: " + string.Join(", ", invalidChars) });
            }

            // Получение URL удаленного API из конфигурации
            var apiUrl = _configuration.GetSection("RemoteApi").GetValue<string>("Url");

            //Обработка входной строки
            string processedString = ProcessingService.ProcessInput(input);
            var charCount = ProcessingService.countCharacters(processedString); //Подсчет символов в обработанной строке
            string longestVowelSubstring = ProcessingService.findLongestVowelSubstring(processedString); //Поиск самой длинной подстроки, состоящей из 
            //Получение случайного индекса входной строки
            int randomIndex = await randomIndexApi(input.Length);
            //Удаление символа по случайному индексу
            string modifiedString = RandomAPI.RemoveCharacterAtIndex(processedString, randomIndex);
            //Выбор сортировки
            string sortedString = sortMethod.ToLower() == "tree" ? TreeSort._TreeSort(processedString.ToCharArray()) : QuickSort._QuickSort(processedString);

            //Возвращаем результаты в формате JSON
            return Ok(new
            {
                processedString,
                charCount,
                longestVowelSubstring,
                sortedString,
                BlackWords,
                modifiedString,
                randomIndex
            });
        }
    }
}