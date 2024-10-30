using Microsoft.Extensions.Options;
using MongoDB.Driver;
using spend_wise_api.data;
using spend_wise_api.services;

var builder = WebApplication.CreateBuilder(args);

// Configurações do MongoDB
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Registrar o cliente MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Registrar o serviço RegistrosService
builder.Services.AddSingleton<RegistrosService>();

// Adicionar controladores e Swagger
builder.Services.AddControllers(); // Adiciona o suporte a controladores
builder.Services.AddSwaggerGen();

// Adicionar configurações de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Registrar páginas Razor (caso sejam necessárias)
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Configurar Swagger para o ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty; // Serve o Swagger na URL raiz
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Aplicar a política de CORS
app.UseCors("AllowAllOrigins");

// Configurar endpoints da API e páginas
app.MapControllers(); // Certifique-se de que controladores estão mapeados
app.MapRazorPages();

app.Run();
