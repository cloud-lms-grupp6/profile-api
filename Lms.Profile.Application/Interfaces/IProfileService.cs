using Lms.Profile.Application.DTOs;
using Lms.Profile.Domain.Entities;

namespace Lms.Profile.Application.Interfaces;

public interface IProfileService
{
    Task<IEnumerable<UserProfile>> GetAllAsync();
    Task<UserProfile?> GetByIdAsync(Guid id);
    Task<UserProfile> CreateAsync(CreateProfileRequest request);
    Task<bool> UpdateAsync(Guid id, UpdateProfileRequest request);
    Task<bool> DeleteAsync(Guid id);
}