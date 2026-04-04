using TestREST.API.Dtos;

namespace TestREST.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      var app = builder.Build();

      app.MapGet("/", () => "Hello World!");

      //GET inpoint /users
      app.MapGet("/groups", () => 
      {
        List<GroupDto> groupDtos = [
          new GroupDto(1, "Adminstrator 01",  GroupTypeDto.Security,      "Administrator of Group"),
          new GroupDto(2, "Adminstrator 02",  GroupTypeDto.Security,      "Administrator of Group"),
          new GroupDto(3, "Glenn Close",      GroupTypeDto.Microsoft365,  "Regular User"),
          new GroupDto(4, "Michael Douglas",  GroupTypeDto.Microsoft365,  "Regular User"),
          new GroupDto(5, "Anne Archer",      GroupTypeDto.Security,      "Regular User"),
          new GroupDto(6, "Jane Krawkowski",  GroupTypeDto.Security,      "Regular User"),
        ];

        return Results.Ok(groupDtos);
      });

      app.Run();
    }
  }
}
