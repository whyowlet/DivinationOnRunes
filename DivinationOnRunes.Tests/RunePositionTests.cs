using DivinationOnRunes.Core;

namespace DivinationOnRunes.Tests;

public class RunePositionTests
{
    private static Rune CreateReversibleRune() => new Rune(
        id: 1,
        name: "Феху",
        englishName: "Fehu",
        symbol: 'ᚠ',
        shortMeaning: "Скот, деньги, богатство",
        uprightMeaning: "Материальный успех, прибыль, новые возможности для заработка.",
        reversedMeaning: "Финансовые потери, упущенные возможности, разочарование в материальных вопросах.",
        isReversible: true);

    private static Rune CreateSymmetricRune() => new Rune(
        id: 7,
        name: "Гебо",
        englishName: "Gebo",
        symbol: 'ᚷ',
        shortMeaning: "Дар, копьё",
        uprightMeaning: "Взаимовыгодное сотрудничество, подарок, гармоничные отношения.",
        reversedMeaning: "",
        isReversible: false);
    
    [Fact]
    public void GetMeaning_RuneIsUpright_ReturnsUprightMeaning()
    {
        Rune rune = CreateReversibleRune();
        RunePosition runePosition = new RunePosition(rune, false, "Ответ");
        
        string meaning = runePosition.GetMeaning();
        
        Assert.Equal("Материальный успех, прибыль, новые возможности для заработка.", meaning);
    }

    [Fact]
    public void GetMeaning_RuneIsReversed_ReturnsReversedMeaning()
    {
        Rune rune = CreateReversibleRune();
        RunePosition runePosition = new RunePosition(rune, true, "Ответ");
        
        string meaning = runePosition.GetMeaning();
        
        Assert.Equal("Финансовые потери, упущенные возможности, разочарование в материальных вопросах.", meaning);
    }

    [Fact]
    public void GetMeaning_SymmetricRuneIsReversed_ReturnsUprightMeaning()
    {
        Rune rune = CreateSymmetricRune();
        RunePosition runePosition = new RunePosition(rune, true, "Ответ");
        
        string meaning = runePosition.GetMeaning();
        
        Assert.Equal("Взаимовыгодное сотрудничество, подарок, гармоничные отношения.", meaning);
    }
}