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

    public async Task<IEnumerable<ProfileResponse>> GetAllAsync()
    {
        return await _context.UserProfiles
            .Select(p => new ProfileResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email
            })
            .ToListAsync();
    }

    public async Task<ProfileResponse?> GetByIdAsync(Guid id)
    {
        return await _context.UserProfiles
            .Where(p => p.Id == id)
            .Select(p => new ProfileResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProfileResponse> CreateAsync(CreateProfileRequest request)
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

        return new ProfileResponse
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email
        };
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