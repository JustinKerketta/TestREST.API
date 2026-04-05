using AutoMapper;

namespace TestREST.API.Dtos;

// Classes that map from one type to another should inherit from class Profile  so that these classes can be
// detected by the class AutoMapper when Services are being added in "Program.cs".
public class DtotoDtoMapper : Profile
{
  public DtotoDtoMapper()
  {
      CreateMap<CreateGroupDto, GroupDto>()
          // "Id" must match the name of the parameter in your record definition
          .ForCtorParam("Id", opt => opt.MapFrom((src, context) => 
          {
              // Rename the "Id" field in the destination "context" as "NewId"
              return context.Items["NewId"];
          }));
  }
}
