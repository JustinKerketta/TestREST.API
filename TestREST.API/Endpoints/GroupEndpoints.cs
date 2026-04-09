using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using TestREST.API.Dtos;

namespace TestREST.API.Endpoints;

public static class GroupEndpoints
{
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
      Group? retrievedGroup = await restContext.Group.SingleOrDefaultAsync(group =>
        group.Id == id
      );

      if (retrievedGroup == null)
      {
        return Results.NotFound();
      }
      GroupDto retrievedGroupDto = mapper.Map<GroupDto>(retrievedGroup);
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

      // Retrieve the newly created group using its Id
      Group? retrievedGroup = await restContext.Group
          .SingleOrDefaultAsync(g => g.Id == groupToAdd.Id);

      if (retrievedGroup == null)
      {
        return Results.BadRequest();
      }
      GroupDto groupAddedDto = mapper.Map<GroupDto>(retrievedGroup);
      return Results.CreatedAtRoute("GetGroup", new { id = groupToAdd.Id }, groupAddedDto);
    })
    //.RequireAuthorization()
    ;

    // PUT /groups/id
    group.MapPut("/{id}", async (int id, UpdateGroupDto updateGroupDto,
      IMapper mapper, RESTContext restContext) =>
    {
      Group? groupToUpdate = await restContext.Group.FindAsync(id);
      if (groupToUpdate != null)
      {
        groupToUpdate.Name = updateGroupDto.Name;
        groupToUpdate.Type = (GroupType)updateGroupDto.Type;
        groupToUpdate.Description = updateGroupDto.Description;

        await restContext.SaveChangesAsync();

        // Retrieve the newly created group using its Id
        Group? retrievedGroup = await restContext.Group
            .SingleOrDefaultAsync(g => g.Id == id);

        if (retrievedGroup == null)
        {
          return Results.BadRequest();
        }
        GroupDto retrievedDto = mapper.Map<GroupDto>(retrievedGroup);

        return Results.Ok(retrievedDto);
      }
      return Results.NotFound();
    })
    //.RequireAuthorization()
    ;

    // Delete /groups/id
    group.MapDelete("/{id}", async (int id, RESTContext restContext) =>
    {
      Group? group= await restContext.Group.FindAsync(id);
      if (group != null)
      {
        //There is no need to "await restContext.SaveChangesAsync();". All records that
        // match the specified `id` are deleted at once.
        await restContext.Group.Where(group => group.Id == id).ExecuteDeleteAsync();

        return Results.NoContent();
      }
      return Results.NotFound();
    })
    //.RequireAuthorization()
    ;
  }
}
