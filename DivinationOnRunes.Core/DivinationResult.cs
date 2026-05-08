namespace DivinationOnRunes.Core;

/// <summary>
/// Полный результат одного гадания
/// </summary>
public class DivinationResult
{
    public string Question { get; }
    public DateTime Timestamp { get; }
    public SpreadType SpreadType { get; }
    public IReadOnlyList<RunePosition> Runes { get; }

    public DivinationResult(string question, SpreadType spreadType,
        IReadOnlyList<RunePosition> runes)
    {
        Question = question;
        Timestamp = DateTime.Now;
        SpreadType = spreadType;
        Runes = runes;
    }
}