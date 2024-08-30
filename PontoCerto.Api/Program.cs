using PontoCerto.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PontoCerto.Infrastructure.Data;
using PontoCerto.Application.Mappings;
using PontoCerto.Application.Services;
using PontoCerto.Application.Interfaces;
using PontoCerto.Infrastructure.Repositories;
using PontoCerto.Application.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
builder.Services.AddControllers();

// Configurar DbContext
builder.Services.AddDbContext<PontoCertoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositórios
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IRegistroPontoRepository, RegistroPontoRepository>();

// Registrar serviços
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<IRegistroPontoService, RegistroPontoService>();
builder.Services.AddScoped<IValidadorErro, ValidadorErro>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
