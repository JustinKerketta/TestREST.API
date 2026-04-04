namespace TestREST.API.Dtos;

public record GroupDto(
  int Id,
  string Name,
  GroupTypeDto type,
  string description);
