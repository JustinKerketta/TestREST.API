using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class RESTContext(DbContextOptions<RESTContext> dbContextoptions) : DbContext(dbContextoptions)
{
  public DbSet<Group> Group { get; set; }
  public DbSet<User> User { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Many-to-many relationship between Users and Groups
    modelBuilder.Entity<Group>()
        .HasMany(g => g.Users)
        .WithMany(u => u.Groups)
        .UsingEntity<Dictionary<string, object>>(
            "UserGroup",  // Join table name
            j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
            j => j.HasOne<Group>().WithMany().HasForeignKey("GroupId")
        );

    // Optional: Seed data
    modelBuilder.Entity<Group>().HasData(
        new Group { Id = 1, Name = "Admin Group", Type = GroupType.Security, Description = "This group handles all security tasks" },
        new Group { Id = 2, Name = "Microsoft 365 Group", Type = GroupType.Microsoft365, Description = "This group is responsible for Microsoft 365 services" }
    );

    modelBuilder.Entity<User>().HasData(
        new User { Id = 1, Name = "Alice", Description = "Security Expert" },
        new User { Id = 2, Name = "Bob", Description = "Microsoft 365 Administrator" }
    );
  }
}

