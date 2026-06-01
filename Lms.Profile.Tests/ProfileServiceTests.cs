using Lms.Profile.Application.DTOs;
using Lms.Profile.Infrastructure.Data;
using Lms.Profile.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Lms.Profile.Tests;

public class ProfileServiceTests
{
    private ProfileDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ProfileDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ProfileDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Profile()
    {
        var context = CreateDbContext();
        var service = new ProfileService(context);

        var request = new CreateProfileRequest
        {
            UserId = "user-1",
            FirstName = "Sahand",
            LastName = "Bagheri",
            Email = "sahand@test.se"
        };

        var result = await service.CreateAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Sahand", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Profile_When_Profile_Exists()
    {
        var context = CreateDbContext();
        var service = new ProfileService(context);

        var created = await service.CreateAsync(new CreateProfileRequest
        {
            UserId = "user-2",
            FirstName = "Ali",
            LastName = "Test",
            Email = "ali@test.se"
        });

        var result = await service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Ali", result.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Profile_When_Profile_Exists()
    {
        var context = CreateDbContext();
        var service = new ProfileService(context);

        var created = await service.CreateAsync(new CreateProfileRequest
        {
            UserId = "user-3",
            FirstName = "Old",
            LastName = "Name",
            Email = "old@test.se"
        });

        var updateRequest = new UpdateProfileRequest
        {
            FirstName = "New",
            LastName = "Name",
            Email = "new@test.se"
        };

        var updated = await service.UpdateAsync(created.Id, updateRequest);
        var result = await service.GetByIdAsync(created.Id);

        Assert.True(updated);
        Assert.NotNull(result);
        Assert.Equal("New", result.FirstName);
        Assert.Equal("new@test.se", result.Email);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Profile_When_Profile_Exists()
    {
        var context = CreateDbContext();
        var service = new ProfileService(context);

        var created = await service.CreateAsync(new CreateProfileRequest
        {
            UserId = "user-4",
            FirstName = "Delete",
            LastName = "Me",
            Email = "delete@test.se"
        });

        var deleted = await service.DeleteAsync(created.Id);
        var result = await service.GetByIdAsync(created.Id);

        Assert.True(deleted);
        Assert.Null(result);
    }
}