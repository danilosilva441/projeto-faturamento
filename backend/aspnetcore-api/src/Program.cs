using FaturamentoApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do IHttpClientFactory para o microserviço
builder.Services.AddHttpClient("AnalysisService", client =>
{
    client.BaseAddress = new Uri("http://localhost:3000/");
});

// Configuração dos Controllers com a serialização JSON correta
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configuração do CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // A porta do Vue
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Configuração do Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- INÍCIO DA CONFIGURAÇÃO PARA SPA ---

// 1. Diz ao ASP.NET para servir ficheiros estáticos (como index.html, css, js).
//    Em produção, o build do seu Vue ficaria numa pasta como 'wwwroot'.
app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

// A API continua a usar o seu próprio roteamento
app.UseRouting();

app.UseAuthorization();

// Mapeia os controllers da API (ex: /api/faturamentos, /api/operacoes)
app.MapControllers();

// 2. A LINHA MÁGICA:
//    Se nenhuma rota de API corresponder ao pedido do navegador,
//    devolva o ficheiro principal do Vue (`index.html`).
//    Isto permite que o Vue Router assuma o controlo da navegação no browser.
app.MapFallbackToFile("index.html");

// --- FIM DA CONFIGURAÇÃO PARA SPA ---

app.Run();

