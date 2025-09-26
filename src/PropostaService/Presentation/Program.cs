using PropostaService.Application.Ports;
using PropostaService.Application.UseCases;
using PropostaService.Infrastructure.Configuration;
using PropostaService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=propostas_db;Username=postgres;Password=postgres";

builder.Services.AddDatabase(connectionString);

builder.Services.AddScoped<IPropostaRepository>(provider => new PropostaRepository(connectionString));
builder.Services.AddScoped<CriarPropostaUseCase>();
builder.Services.AddScoped<ListarPropostasUseCase>();
builder.Services.AddScoped<AlterarStatusPropostaUseCase>();
builder.Services.AddScoped<ObterPropostaPorIdUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Services.MigrateDatabase();

app.Run();