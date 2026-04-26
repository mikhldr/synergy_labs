using System.ComponentModel.DataAnnotations;

namespace Part3_WebAPI.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string PassHash { get; set; } = string.Empty;
}


public class UserCreateDto
{
    public string Login { get; set; } = string.Empty;
    public string PassHash { get; set; } = string.Empty;
}

public class UserUpdateDto
{
    public string? Login { get; set; }
    public string? PassHash { get; set; }
}
