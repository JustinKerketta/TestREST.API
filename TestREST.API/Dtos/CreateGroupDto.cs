namespace TestREST.API.Dtos;

public record CreateGroupDto(
  string Name,
  GroupTypeDto Type,
  string Description);
