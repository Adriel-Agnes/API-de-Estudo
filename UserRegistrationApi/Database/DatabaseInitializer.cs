using Npgsql;

namespace UserRegistrationApi.Database;

public class DatabaseInitializer
{
    private readonly IConfiguration _configuration;

    public DatabaseInitializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        const string sql = """
            CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
                name VARCHAR(100) NOT NULL UNIQUE,
                age INT NOT NULL,
                is_adult BOOLEAN NOT NULL,
                is_deleted BOOLEAN NOT NULL DEFAULT FALSE
            );
            """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        await command.ExecuteNonQueryAsync();
    }
}