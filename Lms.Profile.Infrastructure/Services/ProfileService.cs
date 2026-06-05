using Lms.Profile.Application.DTOs;
using Lms.Profile.Application.Interfaces;
using Lms.Profile.Domain.Entities;
using Lms.Profile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

// ProfileService innehåller affärslogiken för användarprofiler.
// Här hanteras CRUD-operationer och uppdatering av profilbild.
//
// AI användes som stöd för att strukturera service-lagret och EF Core-frågor.
// Koden anpassades därefter manuellt efter projektets DTO:er, databasmodell och krav.

namespace Lms.Profile.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly ProfileDbContext _context;

    public ProfileService(ProfileDbContext context)
    {
        _context = context;
    }

    // Hämtar alla profiler från databasen och mappar dem till ProfileResponse.
    public async Task<IEnumerable<ProfileResponse>> GetAllAsync()
    {
        return await _context.UserProfiles
            .Select(p => new ProfileResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                ProfileImageUrl = p.ProfileImageUrl
            })
            .ToListAsync();
    }

    // Hämtar en specifik profil baserat på profilens unika ID.
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
                Email = p.Email,
                ProfileImageUrl = p.ProfileImageUrl
            })
            .FirstOrDefaultAsync();
    }

    // Hämtar en profil baserat på användarens UserId.
    // Detta används exempelvis när den inloggade användaren ska hämta sin egen profil.
    public async Task<ProfileResponse?> GetByUserIdAsync(string userId)
    {
        return await _context.UserProfiles
            .Where(p => p.UserId == userId)
            .Select(p => new ProfileResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                ProfileImageUrl = p.ProfileImageUrl
            })
            .FirstOrDefaultAsync();
    }

    // Returnerar en publik version av profilen.
    // Här skickas inte all information tillbaka, utan bara det som får visas offentligt.
    public async Task<PublicProfileResponse?> GetPublicProfileAsync(string userId)
    {
        return await _context.UserProfiles
            .Where(p => p.UserId == userId)
            .Select(p => new PublicProfileResponse
            {
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                ProfileImageUrl = p.ProfileImageUrl
            })
            .FirstOrDefaultAsync();
    }

    // Skapar en ny profil från inkommande CreateProfileRequest.
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

        // Returnerar den skapade profilen som DTO istället för entity.
        return new ProfileResponse
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email,
            ProfileImageUrl = profile.ProfileImageUrl
        };
    }

    // Uppdaterar profilinformation baserat på profilens ID.
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

    // Uppdaterar profilinformation baserat på UserId.
    // Detta är användbart för /me-endpoints där användaren uppdaterar sin egen profil.
    public async Task<bool> UpdateByUserIdAsync(string userId, UpdateProfileRequest request)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile is null)
            return false;

        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.Email = request.Email;

        await _context.SaveChangesAsync();

        return true;
    }

    // Uppdaterar endast profilbildens URL för en användare.
    // Denna metod används när frontend skickar en ny profilbild eller bild-URL.
    public async Task<bool> UpdateProfileImageByUserIdAsync(
        string userId,
        UpdateProfileImageRequest request)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile is null)
            return false;

        profile.ProfileImageUrl = request.ProfileImageUrl;

        await _context.SaveChangesAsync();

        return true;
    }

    // Tar bort en profil från databasen baserat på profilens ID.
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