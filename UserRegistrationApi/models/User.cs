namespace UserRegistrationApi.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public bool IsAdult { get; set; }

    public bool IsDeleted { get; set; }
}