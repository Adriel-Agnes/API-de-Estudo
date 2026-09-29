using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Models;
using UserRegistrationApi.Services;

namespace UserRegistrationApi.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request)
    {
        var id = await _service.CreateAsync(
            request.Name,
            request.Age);

        return Ok(new
        {
            id
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var users = await _service.SearchByNameAsync(name);

            return Ok(users);
        }

        var allUsers = await _service.GetAllAsync();

        return Ok(allUsers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById( [FromRoute] int id)
    {
        var user = await _service.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateUserRequest request)
    {
        var updated = await _service.UpdateAsync(
            id,
            request.Name,
            request.Age);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }
}

public class UpdateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}