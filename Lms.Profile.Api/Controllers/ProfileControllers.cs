using Lms.Profile.Domain.Entities;
using Lms.Profile.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Profile.Application.DTOs;

namespace Lms.Profile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly ProfileDbContext _context;

    public ProfilesController(ProfileDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserProfile>>> GetAll()
    {
        return await _context.UserProfiles.ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserProfile>> GetById(Guid id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);

        if (profile is null)
            return NotFound();

        return profile;
    }

    [HttpPost]
public async Task<ActionResult<UserProfile>> Create(CreateProfileRequest request)
{
    var profile = new UserProfile
    {
        UserId = request.UserId,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email
    };

    _context.UserProfiles.Add(profile);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
}

    [HttpPut("{id:guid}")]
public async Task<IActionResult> Update(Guid id, UpdateProfileRequest request)
{
    var profile = await _context.UserProfiles.FindAsync(id);

    if (profile is null)
        return NotFound();

    profile.FirstName = request.FirstName;
    profile.LastName = request.LastName;
    profile.Email = request.Email;

    await _context.SaveChangesAsync();

    return NoContent();
}

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);

        if (profile is null)
            return NotFound();

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}