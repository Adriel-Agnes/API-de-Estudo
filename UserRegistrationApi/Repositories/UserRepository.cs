using Npgsql;
using UserRegistrationApi.Models;

namespace UserRegistrationApi.Repositories;

public class UserRepository
{
    private readonly IConfiguration _configuration;

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> CreateAsync(
        string name,
        int age,
        bool isAdult)
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        const string sql = """
            INSERT INTO users
                (name, age, is_adult)
            VALUES
                (@name, @age, @isAdult)
            RETURNING id;
            """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@age", age);
        command.Parameters.AddWithValue("@isAdult", isAdult);

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<List<User>> GetAllAsync()
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        const string sql = """
        SELECT
            id,
            name,
            age,
            is_adult,
            is_deleted
        FROM users
        WHERE is_deleted = FALSE
        ORDER BY id;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        await using var reader =
            await command.ExecuteReaderAsync();

        var users = new List<User>();

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Age = reader.GetInt32(reader.GetOrdinal("age")),
                IsAdult = reader.GetBoolean(reader.GetOrdinal("is_adult")),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("is_deleted"))
            });
        }

        return users;
    }


    public async Task<User?> GetByIdAsync(int id)
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        const string sql = """
        SELECT
            id,
            name,
            age,
            is_adult,
            is_deleted
        FROM users
        WHERE id = @id
          AND is_deleted = FALSE;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);

        await using var reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new User
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Age = reader.GetInt32(reader.GetOrdinal("age")),
            IsAdult = reader.GetBoolean(reader.GetOrdinal("is_adult")),
            IsDeleted = reader.GetBoolean(reader.GetOrdinal("is_deleted"))
        };
    }


    public async Task<List<User>> SearchByNameAsync(string name)
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        const string sql = """
        SELECT
            id,
            name,
            age,
            is_adult,
            is_deleted
        FROM users
        WHERE name ILIKE @name
          AND is_deleted = FALSE
        ORDER BY name;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@name", $"{name}%");

        await using var reader =
            await command.ExecuteReaderAsync();

        var users = new List<User>();

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Age = reader.GetInt32(reader.GetOrdinal("age")),
                IsAdult = reader.GetBoolean(reader.GetOrdinal("is_adult")),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("is_deleted"))
            });
        }

        return users;
    }


    public async Task<bool> UpdateAsync(
    int id,
    string name,
    int age,
    bool isAdult)
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        const string sql = """
        UPDATE users
        SET
            name = @name,
            age = @age,
            is_adult = @isAdult
        WHERE id = @id
          AND is_deleted = FALSE;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@age", age);
        command.Parameters.AddWithValue("@isAdult", isAdult);

        var affectedRows =
            await command.ExecuteNonQueryAsync();

        return affectedRows > 0;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        const string sql = """
        UPDATE users
        SET is_deleted = TRUE
        WHERE id = @id
          AND is_deleted = FALSE;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);

        var affectedRows =
            await command.ExecuteNonQueryAsync();

        return affectedRows > 0;
    }
}