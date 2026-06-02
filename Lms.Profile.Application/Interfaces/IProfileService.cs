using Lms.Profile.Application.DTOs;

namespace Lms.Profile.Application.Interfaces;

public interface IProfileService
{
    Task<IEnumerable<ProfileResponse>> GetAllAsync();
    Task<ProfileResponse?> GetByIdAsync(Guid id);
    Task<ProfileResponse?> GetByUserIdAsync(string userId);
    Task<ProfileResponse> CreateAsync(CreateProfileRequest request);
    Task<bool> UpdateAsync(Guid id, UpdateProfileRequest request);
    Task<bool> UpdateByUserIdAsync(string userId, UpdateProfileRequest request);
    Task<bool> DeleteAsync(Guid id);
}