using DivinationOnRunes.Console;
using DivinationOnRunes.Core;
using Microsoft.Extensions.Configuration;

Console.OutputEncoding = System.Text.Encoding.UTF8;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .Build();
    
DatabaseSettings databaseSettings = configuration.GetSection("Database").Get<DatabaseSettings>()
    ?? throw new InvalidOperationException("Секция Database не найдена в конфигурации (файл appsettings.json)");

if (string.IsNullOrEmpty(databaseSettings.Username))
{
    Console.Error.WriteLine("Не задано имя пользователя базы данных (файл appsettings.Local.json)");
    return 1;
}

string connectionString = databaseSettings.ConnectionString;
RuneDatabase repository = new RuneDatabase(connectionString);
IReadOnlyList<Rune> runes = repository.GetRunes();

Console.WriteLine($"Из базы данных загружено рун: {runes.Count}\n");

Console.WriteLine("Гадание на рунах");
Console.Write("Задай свой вопрос: ");
string question = Console.ReadLine() ?? "";

Console.WriteLine("Виды расклада");
Console.WriteLine("1 — Вопрос");
Console.WriteLine("2 — Прошлое, настоящее, будущее");
Console.WriteLine("3 — Ситуация, препятствие, совет");
Console.Write("Выбери расклад: ");

string type = Console.ReadLine() ?? "";
SpreadType spreadType = type switch
{
    "1" => SpreadType.Answer,
    "2" => SpreadType.PastPresentFuture,
    "3" => SpreadType.SituationObstacleAdvice,
    _ => SpreadType.Answer
};

DivinationService divinationService = new DivinationService(runes);
DivinationResult divinationResult = divinationService.Divine(question, spreadType);

Console.WriteLine($"\nРезультат гадания");

foreach (RunePosition runePosition in divinationResult.Runes)
{
    Console.WriteLine(runePosition);
    Console.WriteLine(runePosition.GetMeaning());
    Console.WriteLine();
}

return 0;