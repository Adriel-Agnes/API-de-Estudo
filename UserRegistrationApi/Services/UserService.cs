using UserRegistrationApi.Models;
using UserRegistrationApi.Repositories;

namespace UserRegistrationApi.Services;

public class UserService
{
    private readonly UserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> CreateAsync(string name, int age)
    {
        bool isAdult = age >= 18;

        return await _repository.CreateAsync(
            name,
            age,
            isAdult);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<User>> SearchByNameAsync(string name)
    {
        return await _repository.SearchByNameAsync(name);
    }

    public async Task<bool> UpdateAsync(
    int id,
    string name,
    int age)
    {
        bool isAdult = age >= 18;

        return await _repository.UpdateAsync(
            id,
            name,
            age,
            isAdult);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}