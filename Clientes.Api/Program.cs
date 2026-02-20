using Clientes.Application.Abstracoes;
using Clientes.Application.Clientes;
using Clientes.Infrastructure.EventStore;
using Clientes.Infrastructure.Leitura;
using Clientes.Infrastructure.Repositorios;
using Clientes.Infrastructure.Servicos;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsAberto", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var useInMemory = string.Equals(builder.Configuration["USE_INMEMORY"], "true", StringComparison.OrdinalIgnoreCase);
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (!useInMemory && !string.IsNullOrWhiteSpace(conn))
{
    builder.Services.AddDbContext<ContextoLeitura>(opt =>
        opt.UseNpgsql(conn));
    builder.Services.AddScoped<IRepositorioEvento, RepositorioEvento>(sp =>
    {
        var ctx = sp.GetRequiredService<ContextoLeitura>();
        return new RepositorioEvento(ctx);
    });
}
else
{
    builder.Services.AddDbContext<ContextoLeitura>(opt =>
        opt.UseInMemoryDatabase("leitura-dev"));
    builder.Services.AddSingleton<IRepositorioEvento, RepositorioEventoMemoria>();
}

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriarClienteHandler).Assembly));
builder.Services.AddScoped<IRepositorioLeituraCliente, RepositorioLeituraCliente>();
builder.Services.AddValidatorsFromAssemblyContaining<CriarClienteValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpClient<IServicoViaCep, ServicoViaCep>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ContextoLeitura>();
    ctx.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors("CorsAberto");

app.MapControllers();

app.Run();
