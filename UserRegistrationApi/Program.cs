using UserRegistrationApi.Database;
using UserRegistrationApi.Repositories;
using UserRegistrationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Dependências da aplicação
builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Inicializa o banco de dados
using (var scope = app.Services.CreateScope())
{
    var databaseInitializer =
        scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

    await databaseInitializer.InitializeAsync();
}

app.MapOpenApi();

app.MapControllers();

app.Run();