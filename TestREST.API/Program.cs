using TestREST.API.Endpoints;

namespace TestREST.API;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddAutoMapper(cfg => {
      cfg.AddMaps(typeof(Program));
    });

    // Validate evey endpoint in the API.
    builder.Services.AddValidation();

    var app = builder.Build();

    app.MapRESTEndpoints();

    app.Run();
  }
}
