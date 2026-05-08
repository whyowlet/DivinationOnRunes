namespace DivinationOnRunes.Core;

/// <summary>
/// Выполняет гадание, выбирает руны для расклада случайным образом
/// </summary>
public class DivinationService
{
    // Полный набор рун
    private readonly IReadOnlyList<Rune> _allRunes;
    
    private readonly Random _random;

    public DivinationService(IReadOnlyList<Rune> allRunes, Random? random = null)
    {
        if (allRunes == null || allRunes.Count == 0)
            throw new ArgumentException("Для гадания нужен непустой набор рун", nameof(allRunes));
        
        _allRunes = allRunes;
        _random = random ?? new Random();
    }

    /// <summary>
    /// Выполняет одно гадание и возвращает результат
    /// </summary>
    public DivinationResult Divine(string question, SpreadType spreadType)
    {
        string[] positionNames = GetPositionNames(spreadType);
        int runeCount = positionNames.Length;

        if (runeCount > _allRunes.Count)
            throw new InvalidOperationException("В наборе недостаточно рун для расклада");

        List<Rune> deck = new List<Rune>(_allRunes);
        List<RunePosition> drawn = new List<RunePosition>(runeCount);

        for (int i = 0; i < runeCount; i++)
        {
            int index = _random.Next(deck.Count);
            Rune rune = deck[index];
            deck.RemoveAt(index);
            
            bool isReversed = _random.Next(2) == 1;
            
            drawn.Add(new RunePosition(rune, isReversed, positionNames[i]));
        }
        
        return new DivinationResult(question, spreadType, drawn);
    }

    // Названия позиций для каждого типа расклада
    private static string[] GetPositionNames(SpreadType spreadType) => spreadType switch
    {
        SpreadType.Answer => new[] { "Ответ" },
        SpreadType.PastPresentFuture => new[] { "Прошлое", "Настоящее", "Будущее" },
        SpreadType.SituationObstacleAdvice => new[] { "Ситуация", "Препятствие", "Совет" },
        _ => throw new ArgumentException($"Неизвестный тип расклада: {spreadType}", nameof(spreadType))
    };
}