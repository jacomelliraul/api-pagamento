using PagamentoAPI;
using PagamentoAPI.Repository;
using PagamentoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{AppDomain.CurrentDomain.FriendlyName}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//ADICIONANDO OS ESCOPOS
builder.Services.AddScoped<CartaoRepository>()
                .AddScoped<PagamentoRepository>()
                .AddScoped<MySqlDbContext>()
                .AddScoped<CartaoService>()
                .AddScoped<PagamentoService>();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
string pathAppSettings = "appsettings.json";

if (env == "Development")
{
    pathAppSettings = "appsettings.Development.json";
}

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(pathAppSettings)
    .Build();

var appSettings = config.Get<AppSettings>();

//Registra a instância como Singleton
builder.Services.AddSingleton(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
