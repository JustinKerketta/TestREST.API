using AutoMapper;
using TestREST.API.Models;

namespace TestREST.API.Dtos;

// Classes that map from one type to another should inherit from class Profile  so that these classes can be
// detected by the class AutoMapper when Services are being added in "Program.cs".
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateGroupDto, GroupDto>()
        // "Id" must match the name of the parameter in your record definition
        .ForCtorParam("Id", opt => opt.MapFrom((src, context) =>
        {
          // Rename the "Id" field in the destination "context" as "NewId"
          return context.Items["NewId"];
        }));

    CreateMap<UpdateGroupDto, GroupDto>()
        // "Id" must match the name of the parameter in your record definition
        .ForCtorParam("Id", opt => opt.MapFrom((src, context) =>
        {
          // Rename the "Id" field in the destination "context" as "NewId"
          return context.Items["NewId"];
        }));

    // Mapping from CreateGroupDto to Group
    CreateMap<CreateGroupDto, Group>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type)) // Mapping GroupTypeDto to GroupType
        .ForMember(dest => dest.Users, opt => opt.Ignore()); // Users property is not part of the DTO

    // Mapping from GroupTypeDto to GroupType (assuming they are different types)
    CreateMap<GroupTypeDto, GroupType>();

    // Mapping from Group to GroupDto
    CreateMap<Group, GroupDto>();

    CreateMap<UpdateGroupDto, Group>();
  }
}
