using Microsoft.AspNetCore.Mvc;

namespace MaximNET.Controllers
{
    //Указывает, что данный класс является контроллером API
    [ApiController]
    [Route("api/[controller]")]
    public class StringProcessingController : ControllerBase
    {
        //Обрабатывает GET запросы
        [HttpGet]
        public async Task<ActionResult<Response>> processString([FromQuery] string input, [FromQuery] string sortMethod = "quick")
        {
            //// Проверка на пустую строку
            if (string.IsNullOrWhiteSpace(input))
            {
                return BadRequest(new { error = "Пустая строка" });
            }

            //Если найдены недопустимые символы, возвращаем ошибку
            var invalidChars = ProcessingService.getInvalidCharList(input);
            if (invalidChars.Count > 0)
            {
                return BadRequest(new { error = "Неподходящие символы: " + string.Join(", ", invalidChars) });
            }

            //Обработка входной строки
            string processedString = ProcessingService.processInput(input);
            var charCount = ProcessingService.countCharacters(processedString); //Подсчет символов в обработанной строке
            string longestVowelSubstring = ProcessingService.findLongestVowelSubstring(processedString); //Поиск самой длинной подстроки, состоящей из гласных

            //Получение случайного числа API
            int randomIndex = await ProcessingService.getRandomIndex(processedString.Length);

            //Удаление символа по случайному индексу
            string modifiedString = ProcessingService.removeCharacterAtIndex(processedString, randomIndex);

            //Выбор сортировки
            string sortedString = sortMethod.ToLower() == "tree" ? TreeSort.treeSort(processedString.ToCharArray()) : QuickSort.quickSort(processedString);

            //Возвращаем результаты в формате JSON
            return Ok(new
            {
                processedString,
                charCount,
                longestVowelSubstring,
                sortedString,
                modifiedString,
                randomIndex
            });
        }
    }
}