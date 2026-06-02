using System.ComponentModel.DataAnnotations;

namespace Lms.Profile.Application.DTOs;

public class UpdateProfileImageRequest
{
    [Required]
    [Url]
    public string ProfileImageUrl { get; set; } = string.Empty;
}