using Microsoft.EntityFrameworkCore;
using TestREST.API.Models;

namespace TestREST.API.Database.DbContexts;

public class RESTContext(DbContextOptions<RESTContext> dbContextoptions) : DbContext(dbContextoptions)
{
  public DbSet<Group> Group { get; set; }
  public DbSet<User> User { get; set; }
}

