using FaturamentoApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization; // Importante para o ReferenceHandler

var builder = WebApplication.CreateBuilder(args);

// --- Configuração dos Controllers com a Serialização JSON CORRIGIDA ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ESTA É A MUDANÇA: IgnoreCycles resolve o loop e gera um JSON limpo.
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// -------------------------------------------------------------------

// Configuração do CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // Use a porta do seu Vue
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

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

app.Run();

