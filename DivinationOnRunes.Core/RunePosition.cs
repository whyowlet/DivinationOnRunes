namespace DivinationOnRunes.Core;

/// <summary>
/// Представляет одну вытянутую руну в раскладе
/// </summary>
public class RunePosition
{
    public Rune Rune { get; }
    public bool IsReversed { get; }
    public string Position { get; }

    public RunePosition(Rune rune, bool isReversed, string position)
    {
        Rune = rune;
        IsReversed = isReversed;
        Position = position;
    }

    /// <summary>
    /// Возвращает толкование с учётом положения.
    /// Если руна симметрична — всегда прямое толкование 
    /// </summary>
    public string GetMeaning()
    {
        if (!Rune.IsReversible || !IsReversed)
            return Rune.UprightMeaning;
        
        return Rune.ReversedMeaning;
    }

    public override string ToString()
    {
        string orientation = IsReversed && Rune.IsReversible ? "перевёрнутая" : "прямая";
        return $"{Position} — {Rune.Name} ({orientation})";
    }
}