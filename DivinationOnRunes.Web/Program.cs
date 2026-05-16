using DivinationOnRunes.Core;
using DivinationOnRunes.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<RuneDatabase>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var databaseSection = configuration.GetSection("Database");
    var settings = databaseSection.Get<DatabaseSettings>()
                   ?? throw new InvalidOperationException("Секция Database не найдена");
    return new RuneDatabase(settings.ConnectionString);
});

builder.Services.AddSingleton<DivinationService>(serviceProvider =>
{
    var runeDatabase = serviceProvider.GetRequiredService<RuneDatabase>();
    var runes = runeDatabase.GetRunes();
    return new DivinationService(runes);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();