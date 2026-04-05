using System.ComponentModel.DataAnnotations;

namespace TestREST.API.Dtos;

public record UpdateGroupDto(
  [Required]
  [StringLength(50)]
  string Name,

  GroupTypeDto Type,

  [Required]
  [StringLength(50)]
  string Description
  );
