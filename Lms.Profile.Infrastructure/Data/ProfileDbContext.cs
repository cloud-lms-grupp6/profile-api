using Lms.Profile.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.Profile.Infrastructure.Data;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
}