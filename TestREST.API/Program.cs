using AutoMapper;
using TestREST.API.Dtos;

namespace TestREST.API;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddAutoMapper(cfg => {
      cfg.AddMaps(typeof(Program));
    });
    var app = builder.Build();

    List<GroupDto> groupDtos = [
      new GroupDto(1, "Adminstrator 01",  GroupTypeDto.Security,      "Administrator of Group"),
      new GroupDto(2, "Adminstrator 02",  GroupTypeDto.Security,      "Administrator of Group"),
      new GroupDto(3, "Glenn Close",      GroupTypeDto.Microsoft365,  "Regular User"),
      new GroupDto(4, "Michael Douglas",  GroupTypeDto.Microsoft365,  "Regular User"),
      new GroupDto(5, "Anne Archer",      GroupTypeDto.Security,      "Regular User"),
      new GroupDto(6, "Jane Krawkowski",  GroupTypeDto.Security,      "Regular User"),
    ];

    app.MapGet("/", () => "Hello World!");

    //GET /groups
    app.MapGet("/groups", () => 
    {
      return Results.Ok(groupDtos);
    });

    //GET /groups/1
    app.MapGet("/groups/{id}", (int id) => 
    {
      return Results.Ok(groupDtos.Find(groupDto => groupDto.Id == id));
    }).WithName("GetGroup");

    // POST /groups/
    CreateGroupDto newGroupDto = new ("Jake Elwood", GroupTypeDto.Security, "Regular User");
    app.MapPost("/groups", (CreateGroupDto newCreateGroupDto, IMapper mapper) => {
      int newId = groupDtos.Count+1;
      GroupDto newGroupDto = mapper.Map<GroupDto>(
        newCreateGroupDto, opt => opt.Items["NewId"] = newId);
      groupDtos.Add(newGroupDto);
      return Results.CreatedAtRoute("GetGroup", new { id = newGroupDto.Id }, newGroupDto);
    });

    app.Run();
  }
}
