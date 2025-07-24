using EscalaMensal.Application.Services;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IEscalaRepository, EscalaRepository>();
builder.Services.AddScoped<IItemEscalaRepository, EscalaItemRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IFuncaoRepository, FuncaoRepository>();
builder.Services.AddScoped<IHistoricoEscalaRepository, HistoricoEscalaRepository>();
builder.Services.AddScoped<ICargoNivelFuncaoPermitidaRepository, CargoNivelFuncaoPermitidaRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRestricaoService, RestricaoService>();
builder.Services.AddScoped<IItemEscalaService, ItemEscalaService>();
builder.Services.AddScoped<IHistoricoEscalaService, HistoricoEscalaService>();
builder.Services.AddScoped<IFuncaoService, FuncaoService>();
builder.Services.AddScoped<IEscalaService, EscalaService>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
