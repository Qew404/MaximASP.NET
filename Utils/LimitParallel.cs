namespace ParallelLimit
{
    public class Limit
    {
        private static SemaphoreSlim _semaphore; //Семафор для ограничения количества подключений
        private readonly RequestDelegate _next; //Делегат для следующего обработчика запроса
        private readonly ILogger<Limit> _logger; //Логгер для записи инфы и предупреждений

        //Конструктор который инициализирует логгер и ставит лимит парралельных подключений
        public Limit(RequestDelegate next, IConfiguration configuration, ILogger<Limit> logger)
        {
            //Сохраняем следующий обработчик запроса
            _next = next;
            //Сохраняем логгер
            _logger = logger;
            //Получаем лимит парралельных подключений из конфигурации
            int parallelLimit = configuration.GetSection("Log:Settings").GetValue<int>("ParallelLimit");
            //Инициализируем семафор для ограничения количества подключений
            _semaphore = new SemaphoreSlim(parallelLimit);
        }

        //Обработчик запроса который обрабатывает HTTP запросы
        public async Task InvokeAsync(HttpContext context)
        {
            //Проверяем наличие семафора для ограничения количества подключений
            if (!_semaphore.Wait(0))
            {
                //Если семафор не доступен, то выводим предупреждение
                _logger.LogWarning("Сервис недоступен из-за лимита подключений.");
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }

            try
            {
                //Если доступ к семафору есть, то обрабатываем запрос
                await _next(context);
            }
            finally
            {
                //Освобождаем семафор
                _semaphore.Release();
            }
        }
    }
}