using Lms.Profile.Domain.Entities;
using Lms.Profile.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<UserProfile>> Create(UserProfile profile)
    {
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
    }
}