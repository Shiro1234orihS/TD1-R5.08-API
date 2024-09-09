using Microsoft.EntityFrameworkCore;
using TD1_code.Models.DataManager;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContexte>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBContexte")));

builder.Services.AddScoped<IDataRepository<TypeProduit>, TypeProduitManager>();
builder.Services.AddScoped<IDataRepository<Marque>, MarqueManager>();
builder.Services.AddScoped<IDataRepository<Produit>, ProduitManager>();

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
