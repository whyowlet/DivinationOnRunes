namespace DivinationOnRunes.Core;

/// <summary>
/// Описывает одну руну Старшего Футарка
/// </summary>
public class Rune
{
    // Уникальный идентификатор руны
    public int Id { get; }
    
    // Название руны на русском языке
    public string Name { get; }
    
    // Название руны на английском языке
    public string EnglishName { get; }
    
    // Краткое значение, перевод названия руны
    public string ShortMeaning { get; }
    
    // Толкование в прямом положении
    public string UprightMeaning { get; }
    
    // Толкование в перевёрнутом положении
    public string ReversedMeaning { get; }
    
    // Может ли руна выпасть в перевёрнутом положении
    public bool IsReversible { get; }

    public Rune(int id, string name, string englishName, string shortMeaning,
        string uprightMeaning, string reversedMeaning, bool isReversible)
    {
        Id = id;
        Name = name;
        EnglishName = englishName;
        ShortMeaning = shortMeaning;
        UprightMeaning = uprightMeaning;
        ReversedMeaning = reversedMeaning;
        IsReversible = isReversible;
    }
    
    public override string ToString() => $"{Name} ({EnglishName})";
}