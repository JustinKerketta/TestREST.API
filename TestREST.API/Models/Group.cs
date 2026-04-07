namespace TestREST.API.Models;

public class Group
{
  public int Id { get; set; }

  public required string Name { get; set; }

  public GroupType Type { get; set; }

  public required  string Description {get; set;}
}
