namespace Lms.Profile.Application.DTOs;

public class ProfileResponse
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? ProfileImageUrl { get; set; }
}