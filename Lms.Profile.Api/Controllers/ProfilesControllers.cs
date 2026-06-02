using System.Security.Claims;
using Lms.Profile.Application.DTOs;
using Lms.Profile.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfileResponse>>> GetAll()
    {
        var profiles = await _profileService.GetAllAsync();
        return Ok(profiles);
    }

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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProfileResponse>> GetById(Guid id)
    {
        var profile = await _profileService.GetByIdAsync(id);

        if (profile is null)
            return NotFound();

        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<ProfileResponse>> Create(CreateProfileRequest request)
    {
        var profile = await _profileService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProfileRequest request)
    {
        var updated = await _profileService.UpdateAsync(id, request);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _profileService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private string? GetUserIdFromToken()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? User.FindFirstValue("id")
            ?? User.FindFirstValue("userId");
    }
}