namespace Shared.Models;

public class User
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required  string Description {get; set;}

 // Navigation property for groups the user belongs to
 public ICollection<Group> Groups { get; set; } = new List<Group>();

}
