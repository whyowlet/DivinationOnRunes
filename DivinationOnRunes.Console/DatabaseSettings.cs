namespace DivinationOnRunes.Console;

public class DatabaseSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string Database { get; set; } = "divination_on_runes";
    public string Username { get; set; }
    public string Password { get; set; }
    
    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
}