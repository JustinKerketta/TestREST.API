namespace Shared.Models;

public class Group
{
  public int Id { get; set; }

  public required string Name { get; set; }

  public GroupType Type { get; set; }

  public required  string Description {get; set;}

  // Navigation property for the users that belong to this group
  public ICollection<User> Users { get; set; } = new List<User>();
}
