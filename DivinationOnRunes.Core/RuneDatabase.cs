using Npgsql;

namespace DivinationOnRunes.Core;

/// <summary>
/// Загружает руны и их значения из базы данных
/// </summary>
public class RuneDatabase
{
    private readonly string _connectionString;

    public RuneDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IReadOnlyList<Rune> GetRunes()
    {
        const string sql = @"
            SELECT  r.id,
                    r.futhark_order,
                    r.name_ru,
                    r.name_eng,
                    r.symbol,
                    r.is_reversible,
                    m.orientation,
                    r.meaning_short,
                    m.meaning_text
            FROM    runes r
            JOIN    rune_meanings m ON m.rune_id = r.id
            WHERE   m.tradition = 'classic'
            ORDER BY r.futhark_order, m.orientation;";
        
        Dictionary<int, RuneBuilder> runesById = new Dictionary<int, RuneBuilder>();

        using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        using NpgsqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            short futharkOrder = reader.GetInt16(1);
            string nameRu = reader.GetString(2);
            string nameEng = reader.GetString(3);
            char symbol = reader.GetChar(4);
            bool isReversible = reader.GetBoolean(5);
            string orientation = reader.GetString(6);
            string meaningShort = reader.GetString(7);
            string meaningText = reader.GetString(8);

            if (!runesById.TryGetValue(id, out RuneBuilder? runeBuilder))
            {
                runeBuilder = new RuneBuilder
                {
                    Id = id,
                    FutharkOrder = futharkOrder,
                    NameRu = nameRu,
                    NameEng = nameEng,
                    Symbol = symbol,
                    IsReversible = isReversible,
                    ShortMeaning = meaningShort,
                };
                runesById[id] = runeBuilder;
            }

            if (orientation == "upright")
                runeBuilder.UprightMeaning = meaningText;
            else if (orientation == "reversed")
                runeBuilder.ReversedMeaning = meaningText;

        }
        
        return runesById.Values
            .OrderBy(b => b.FutharkOrder)
            .Select(b => new Rune(
                id: b.Id,
                name: b.NameRu,
                englishName: b.NameEng,
                symbol: b.Symbol,
                shortMeaning: b.ShortMeaning,
                uprightMeaning: b.UprightMeaning ?? "",
                reversedMeaning: b.ReversedMeaning ?? "",
                isReversible: b.IsReversible))
            .ToList();
    }
    
    private class RuneBuilder
    {
        public int Id { get; set; }
        public short FutharkOrder { get; set; }
        public string NameRu { get; set; } = "";
        public string NameEng { get; set; } = "";
        public char Symbol { get; set; }
        public bool IsReversible { get; set; }
        public string ShortMeaning { get; set; } = "";
        public string? UprightMeaning { get; set; }
        public string? ReversedMeaning { get; set; }
    }
}