namespace TestREST.API.Dtos;

public record UpdateGroupDto(
  string Name,
  GroupTypeDto Type,
  string Description);
