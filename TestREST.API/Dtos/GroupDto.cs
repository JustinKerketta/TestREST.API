namespace TestREST.API.Dtos;

public record GroupDto(
  int Id,
  string Name,
  GroupTypeDto Type,
  string Description);
