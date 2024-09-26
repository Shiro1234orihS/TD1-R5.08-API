using Microsoft.EntityFrameworkCore;
using TD1_code.Models.DataManager;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuration CORS : Spécifiez le domaine de votre client (par exemple "https://localhost:7016")
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7016") // Remplacez par l'URL de votre client
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajout du contexte de base de données
builder.Services.AddDbContext<DBContexte>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBContexte")));

// Dépendances pour les services
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

// Appliquer CORS avant Authorization
app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
