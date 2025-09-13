using FaturamentoApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- INÍCIO DA CONFIGURAÇÃO DO CORS ---
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // AVISO PARA A API: "Permita requisições que venham DESTA origem"
                          policy.WithOrigins("http://localhost:5173") // A porta do seu VUE
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
// --- FIM DA CONFIGURAÇÃO DO CORS ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- ATIVANDO A POLÍTICA DE CORS ---
// Esta linha é crucial e deve vir aqui
app.UseCors(MyAllowSpecificOrigins);
// ------------------------------------

app.UseAuthorization();
app.MapControllers();
app.Run();