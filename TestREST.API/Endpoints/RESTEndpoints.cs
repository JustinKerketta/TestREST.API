using AutoMapper;
using TestREST.API.Dtos;

namespace TestREST.API.Endpoints;

public static class RESTEndpoints
{
  private static List<GroupDto> groupDtos = [
      new GroupDto(1, "Adminstrator 01",        GroupTypeDto.Security,      "Administrator of Group"),
      new GroupDto(2, "Adminstrator 02",        GroupTypeDto.Security,      "Administrator of Group"),
      new GroupDto(3, "Glenn Close group",      GroupTypeDto.Microsoft365,  "Regular User"),
      new GroupDto(4, "Michael Douglas group",  GroupTypeDto.Microsoft365,  "Regular User"),
      new GroupDto(5, "Anne Archer group",      GroupTypeDto.Security,      "Regular User"),
      new GroupDto(6, "Jane Krawkowski group",  GroupTypeDto.Security,      "Regular User"),
    ];

  public static void MapRESTEndpoints(this WebApplication app)
  {
    app.MapGet("/", () => "Welcome to groups and users!");

    RouteGroupBuilder group = app.MapGroup("/groups");

    //GET /groups
    group.MapGet("/", () =>
    {
      return Results.Ok(groupDtos);
    });

    //GET /groups/1
    group.MapGet("/{id}", (int id) =>
    {
      GroupDto? groupDto = groupDtos.Find(group => group.Id == id);
      if (groupDto == null)
      {
        return Results.NotFound();
      }
      return Results.Ok(groupDto);
    }).WithName("GetGroup");

    // POST /groups/
    group.MapPost("/", (CreateGroupDto newCreateGroupDto, IMapper mapper) =>
    {
      int newId = groupDtos.Count + 1;
      GroupDto newGroupDto = mapper.Map<GroupDto>(
        newCreateGroupDto, opt => opt.Items["NewId"] = newId);
      groupDtos.Add(newGroupDto);
      return Results.CreatedAtRoute("GetGroup", new { id = newGroupDto.Id }, newGroupDto);
    });

    // PUT /groups/id
    group.MapPut("/{id}", (int id, UpdateGroupDto updateGroupDto, IMapper mapper) =>
    {
      GroupDto? groupDto = groupDtos.Find(group => group.Id == id);
      if (groupDto != null)
      {
        GroupDto updatedGroupDto = mapper.Map<GroupDto>(
          updateGroupDto, opt => opt.Items["NewId"] = groupDto.Id);
        groupDtos.Remove(groupDto);
        groupDtos.Add(updatedGroupDto);
        return Results.NoContent();
      }
      return Results.NotFound();
    });

    // Delete /groups/id
    group.MapDelete("/{id}", (int id) =>
    {
      GroupDto? groupDto = groupDtos.Find(group => group.Id == id);
      if (groupDto != null)
      {
        groupDtos.Remove(groupDto);
        return Results.NoContent();
      }
      return Results.NotFound();
    });
  }
}
