using Lms.Profile.Application.DTOs;
using Lms.Profile.Application.Interfaces;
using Lms.Profile.Domain.Entities;
using Lms.Profile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Profile.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly ProfileDbContext _context;

    public ProfileService(ProfileDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserProfile>> GetAllAsync()
    {
        return await _context.UserProfiles.ToListAsync();
    }

    public async Task<UserProfile?> GetByIdAsync(Guid id)
    {
        return await _context.UserProfiles.FindAsync(id);
    }

    public async Task<UserProfile> CreateAsync(CreateProfileRequest request)
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

        return profile;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProfileRequest request)
    {
        var profile = await _context.UserProfiles.FindAsync(id);

        if (profile is null)
            return false;

        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.Email = request.Email;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);

        if (profile is null)
            return false;

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync();

        return true;
    }
}