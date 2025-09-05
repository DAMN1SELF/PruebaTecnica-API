using FluentValidation;
using INCHE.Carrito_Compras.Application.Mappers;
using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Application.UseCases;
using INCHE.Carrito_Compras.Application.Validation;
using INCHE.Carrito_Compras.Dtos;
using INCHE.Carrito_Compras.Dtos.Requests;
using INCHE.Carrito_Compras.Infraestructure.Persistence;
using INCHE.Carrito_Compras.Infraestructure.Rules;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();

// Cargar config JSON (con productId, name, price y groupAttributes)
var cfgPath = Path.Combine(builder.Environment.ContentRootPath, "config", "product-config.json");
var cfgJson = await File.ReadAllTextAsync(cfgPath);
var cfgRoot = JsonSerializer.Deserialize<GroupConfigRoot>(cfgJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
             ?? throw new InvalidOperationException("No se pudo deserializar product-config.json");


builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
builder.Services.AddSingleton<IProductRuleProvider>(sp => new ProductRuleProviderFromConfig(cfgRoot));

builder.Services.AddScoped<AddItemToCart>();
builder.Services.AddScoped<IValidator<AddToCartRequest>, AddToCartRequestValidator>();

builder.Services.AddScoped<UpdateItemInCart>();
builder.Services.AddScoped<IValidator<UpdateItemRequest>, UpdateItemRequestValidator>();

builder.Services.AddScoped<PatchItemQuantity>();
builder.Services.AddScoped<IValidator<PatchQuantityRequest>, PatchQuantityRequestValidator>();

builder.Services.AddScoped<GetCart>();
builder.Services.AddScoped<ICartMapper, CartMapper>();

builder.Services.AddScoped<RemoveItemFromCart>();
builder.Services.AddScoped<IValidator<RemoveItemRequest>, RemoveItemRouteRequestValidator>();

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
