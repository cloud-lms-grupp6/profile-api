using System.Security.Claims;
using Lms.Profile.Application.DTOs;
using Lms.Profile.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ProfilesController exponerar Profile API:s endpoints.
// Frontend använder dessa endpoints för att hämta och uppdatera profilinformation.
//
// AI användes som stöd för strukturering av controller-lagret och JWT-hantering.
// Implementationen anpassades därefter manuellt efter projektets krav.

namespace Lms.Profile.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    // Hämtar alla profiler från systemet.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfileResponse>>> GetAll()
    {
        var profiles = await _profileService.GetAllAsync();
        return Ok(profiles);
    }

    // Hämtar den inloggade användarens profil via JWT-token.
    [HttpGet("me")]
    public async Task<ActionResult<ProfileResponse>> GetMyProfile()
    {
        var userId = GetUserIdFromToken();

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var profile = await _profileService.GetByUserIdAsync(userId);

        if (profile is null)
            return NotFound();

        return Ok(profile);
    }

    // Returnerar en publik profil som kan visas utan inloggning.
    [AllowAnonymous]
    [HttpGet("public/{userId}")]
    public async Task<ActionResult<PublicProfileResponse>> GetPublicProfile(string userId)
    {
        var profile = await _profileService.GetPublicProfileAsync(userId);

        if (profile is null)
            return NotFound();

        return Ok(profile);
    }

    // Uppdaterar den inloggade användarens profilinformation.
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile(UpdateProfileRequest request)
    {
        var userId = GetUserIdFromToken();

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var updated = await _profileService.UpdateByUserIdAsync(userId, request);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // Uppdaterar användarens profilbild.
    [HttpPut("me/profile-image")]
    public async Task<IActionResult> UpdateMyProfileImage(UpdateProfileImageRequest request)
    {
        var userId = GetUserIdFromToken();

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var updated = await _profileService.UpdateProfileImageByUserIdAsync(userId, request);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // Hämtar en specifik profil baserat på profilens ID.
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProfileResponse>> GetById(Guid id)
    {
        var profile = await _profileService.GetByIdAsync(id);

        if (profile is null)
            return NotFound();

        return Ok(profile);
    }

    // Skapar en ny profil i databasen.
[HttpPost]
public async Task<ActionResult<ProfileResponse>> Create(CreateProfileRequest request)
{
    var profile = await _profileService.CreateAsync(request);

    return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
}

    // Uppdaterar en profil baserat på profilens ID.
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProfileRequest request)
    {
        var updated = await _profileService.UpdateAsync(id, request);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // Tar bort en profil från databasen.
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _profileService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    // Hämtar användarens ID från JWT-token.
    // Stöd finns för flera claim-namn beroende på vilket Auth API som används.
    private string? GetUserIdFromToken()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? User.FindFirstValue("id")
            ?? User.FindFirstValue("userId");
    }
}