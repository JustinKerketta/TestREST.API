using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestREST.API.Database.DbContexts;
using TestREST.API.Dtos;
using TestREST.API.Models;

namespace TestREST.API.Endpoints;

public static class GroupEndpoints
{
  private static readonly List<GroupDto> groupDtos = [
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
    group.MapGet("/", async (IMapper mapper, RESTContext restContext) =>
    {
      List<Group> groups = await restContext.Group.ToListAsync();
      
        // Map the list of groups to a list of GroupDto using AutoMapper
      List<GroupDto> groupDtos01 = mapper.Map<List<GroupDto>>(groups);
      return Results.Ok(groupDtos01);
    });

    //GET /groups/1
    group.MapGet("/{id}", async (int id, 
      IMapper mapper, RESTContext restContext) =>
    {
      Group? retrievedGroup= await restContext.Group.SingleOrDefaultAsync(group =>
        group.Id == id
      );

      if (retrievedGroup == null)
      {
        return Results.NotFound();
      }
      Group retrievedGroupDto = mapper.Map<Group>(retrievedGroup);
      return Results.Ok(retrievedGroupDto);
    }).WithName("GetGroup")
    ;

    // POST /groups/
    group.MapPost("/", async (CreateGroupDto newCreateGroupDto,
      IMapper mapper, RESTContext restContext) =>
    {
      //int newId = groupDtos.Count + 1;
      //GroupDto newGroupDto = mapper.Map<GroupDto>(
      //  newCreateGroupDto, opt => opt.Items["NewId"] = newId);
      //groupDtos.Add(newGroupDto);

      //return Results.CreatedAtRoute("GetGroup", new { id = newGroupDto.Id }, newGroupDto);

      Group groupToAdd = mapper.Map<Group>(newCreateGroupDto);
      restContext.Group.Add(groupToAdd);
      await restContext.SaveChangesAsync();

      GroupDto groupAddedDto = mapper.Map<GroupDto>(groupToAdd);
      return Results.CreatedAtRoute("GetGroup", new { id = groupToAdd.Id }, groupAddedDto);
    })
    //.RequireAuthorization()
    ;

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
    })
    .RequireAuthorization();

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
    })
    .RequireAuthorization();
  }
}
