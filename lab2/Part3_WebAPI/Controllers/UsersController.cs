using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part3_WebAPI.Data;
using Part3_WebAPI.Models;

namespace Part3_WebAPI.Controllers;

[ApiController]
[Route("user")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _ctx;

    public UsersController(AppDbContext ctx) => _ctx = ctx;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.PassHash))
            return BadRequest(new { error = "Login и PassHash обязательны" });

        if (await _ctx.Users.AnyAsync(u => u.Login == dto.Login))
            return Conflict(new { error = "Пользователь с таким логином уже существует" });

        var user = new User { Login = dto.Login, PassHash = dto.PassHash };
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id },
            new { user.Id, user.Login, message = "Пользователь создан" });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _ctx.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { error = "Пользователь не найден" });

        return Ok(new { user.Id, user.Login, user.PassHash });
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
    {
        var user = await _ctx.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { error = "Пользователь не найден" });

        if (!string.IsNullOrWhiteSpace(dto.Login))
        {
            if (await _ctx.Users.AnyAsync(u => u.Login == dto.Login && u.Id != id))
                return Conflict(new { error = "Логин занят" });
            user.Login = dto.Login;
        }

        if (!string.IsNullOrWhiteSpace(dto.PassHash))
            user.PassHash = dto.PassHash;

        await _ctx.SaveChangesAsync();
        return Ok(new { user.Id, user.Login, message = "Данные обновлены" });
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _ctx.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { error = "Пользователь не найден" });

        _ctx.Users.Remove(user);
        await _ctx.SaveChangesAsync();

        return Ok(new { message = $"Пользователь {id} удалён" });
    }
}
