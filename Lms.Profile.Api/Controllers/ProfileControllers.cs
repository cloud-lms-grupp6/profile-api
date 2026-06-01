using Lms.Profile.Application.DTOs;
using Lms.Profile.Application.Interfaces;
using Lms.Profile.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lms.Profile.Api.Controllers;

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
    public async Task<ActionResult<IEnumerable<UserProfile>>> GetAll()
    {
        var profiles = await _profileService.GetAllAsync();
        return Ok(profiles);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserProfile>> GetById(Guid id)
    {
        var profile = await _profileService.GetByIdAsync(id);

        if (profile is null)
            return NotFound();

        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<UserProfile>> Create(CreateProfileRequest request)
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
}