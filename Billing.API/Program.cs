using Billing.API.Data.Context;
using Billing.API.Data.Repository;
using Billing.API.Interface.IRepository;
using Billing.API.Interface.IService;
using Billing.API.Service;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// 1. Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Controllers, JSON Options e AutoMapper
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.NumberHandling =
            System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
    });

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(InvoiceProfile));

// 3. Injeção de Dependência das Repositories
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// 4. Configuração do HttpClient (já faz o registro do IInvoiceService/InvoiceService)
builder.Services.AddHttpClient<IInvoiceService, InvoiceService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7186/");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

// 5. Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 6. Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();