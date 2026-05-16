using DivinationOnRunes.Core;

namespace DivinationOnRunes.Tests;

public class DivinationServiceTests
{
    private static IReadOnlyList<Rune> CreateTestRunes()
    {
        List<Rune> runes = new List<Rune>();

        for (int i = 1; i <= 24; i++)
        {
            runes.Add(new Rune(
                id: i,
                name: $"Руна — {i}",
                englishName: $"Rune — {i}",
                symbol: '*',
                shortMeaning: $"Краткое — {i}",
                uprightMeaning: $"Прямое — {i}",
                reversedMeaning: $"Перевёрнутое — {i}",
                isReversible: i % 2 == 0));
        }
        
        return runes;
    }
    
    [Fact]
    public void Divine_AnswerSpread_ReturnsOneRune()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());

        DivinationResult divinationResult = divinationService.Divine("Вопрос", SpreadType.Answer);
        
        Assert.Single(divinationResult.Runes);
    }

    [Fact]
    public void Divine_PastPresentFutureSpread_ReturnsThreeRunes()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());
        
        DivinationResult divinationResult = divinationService.Divine("Вопрос", SpreadType.PastPresentFuture);
        
        Assert.Equal(3, divinationResult.Runes.Count);
    }

    [Fact]
    public void Divine_AnswerSpread_ReturnsCorrectPositionNames()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());
        
        DivinationResult divinationResult = divinationService.Divine("Вопрос", SpreadType.Answer);
        
        Assert.Equal("Ответ", divinationResult.Runes[0].Position);
    }
    
    [Fact]
    public void Divine_PastPresentFutureSpread_ReturnsCorrectPositionNames()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());
        
        DivinationResult divinationResult = divinationService.Divine("Вопрос", SpreadType.PastPresentFuture);
        
        Assert.Equal("Прошлое", divinationResult.Runes[0].Position);
        Assert.Equal("Настоящее", divinationResult.Runes[1].Position);
        Assert.Equal("Будущее", divinationResult.Runes[2].Position);
    }
    
    [Fact]
    public void Divine_SituationObstacleAdviceSpread_ReturnsCorrectPositionNames()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());
        
        DivinationResult divinationResult = divinationService.Divine("Вопрос", SpreadType.SituationObstacleAdvice);
        
        Assert.Equal("Ситуация", divinationResult.Runes[0].Position);
        Assert.Equal("Препятствие", divinationResult.Runes[1].Position);
        Assert.Equal("Совет", divinationResult.Runes[2].Position);
    }

    [Fact]
    public void Divine_AllRunesAreUnique()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());
        
        DivinationResult divinationResult = divinationService.Divine("Вопрос", SpreadType.SituationObstacleAdvice);

        HashSet<int> uniqueRunes = divinationResult.Runes.Select(runePosition => runePosition.Rune.Id).ToHashSet();
        Assert.Equal(uniqueRunes.Count, divinationResult.Runes.Count);
    }

    [Fact]
    public void Divine_SavesSpreadType()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());
        
        DivinationResult divinationResult = divinationService.Divine("Вопрос", SpreadType.SituationObstacleAdvice);
        
        Assert.Equal(SpreadType.SituationObstacleAdvice, divinationResult.SpreadType);
    }
    
    [Fact]
    public void Divine_SavesQuestion()
    {
        DivinationService divinationService = new DivinationService(CreateTestRunes());
        string question = "Вопрос";

        DivinationResult divinationResult = divinationService.Divine(question, SpreadType.Answer);
        
        Assert.Equal(question, divinationResult.Question);
    }

    [Fact]
    public void Constructor_NullRunes_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new DivinationService(null!));
    }

    [Fact]
    public void Constructor_EmptyRunes_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new DivinationService(new List<Rune>()));
    }
    
    [Fact]
    public void Divine_FixedRandom_ReturnsDeterminedResult()
    {
        Random fixedRandom1 = new Random(1);
        Random fixedRandom2 = new Random(1);

        DivinationService divinationService1 = new DivinationService(CreateTestRunes(), fixedRandom1);
        DivinationService divinationService2 = new DivinationService(CreateTestRunes(), fixedRandom2);

        DivinationResult divinationResult1 = divinationService1.Divine("Вопрос", SpreadType.PastPresentFuture);
        DivinationResult divinationResult2 = divinationService2.Divine("Вопрос", SpreadType.PastPresentFuture);
        
        for (int i = 0; i < divinationResult1.Runes.Count; i++)
        {
            Assert.Equal(divinationResult1.Runes[i].Rune.Id, divinationResult2.Runes[i].Rune.Id);
            Assert.Equal(divinationResult1.Runes[i].Rune.Name, divinationResult2.Runes[i].Rune.Name);
            Assert.Equal(divinationResult1.Runes[i].Rune.EnglishName, divinationResult2.Runes[i].Rune.EnglishName);
            Assert.Equal(divinationResult1.Runes[i].Rune.Symbol, divinationResult2.Runes[i].Rune.Symbol);
            Assert.Equal(divinationResult1.Runes[i].Rune.ShortMeaning, divinationResult2.Runes[i].Rune.ShortMeaning);
            Assert.Equal(divinationResult1.Runes[i].IsReversed, divinationResult2.Runes[i].IsReversed);
        }
    }

    [Fact]
    public void Divine_SpreadTypeRequiresMoreRunesThanExist_ThrowsException()
    {
        List<Rune> testRunes = new List<Rune>();

        testRunes.Add(new Rune(
            id: 1,
            name: "Феху",
            englishName: "Fehu",
            symbol: 'ᚠ',
            shortMeaning: "Скот, деньги, богатство",
            uprightMeaning: "Материальный успех, прибыль, новые возможности для заработка.",
            reversedMeaning: "Финансовые потери, упущенные возможности, разочарование в материальных вопросах.",
            isReversible: true));
        
        DivinationService divinationService = new DivinationService(testRunes);

        Assert.Throws<InvalidOperationException>(() => divinationService.Divine("Вопрос", SpreadType.SituationObstacleAdvice));
    }
}