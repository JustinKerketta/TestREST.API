using Microsoft.EntityFrameworkCore;
using TestREST.API.Database.DbContexts;
using TestREST.API.Endpoints;

namespace TestREST.API;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddAutoMapper(cfg =>
    {
      cfg.AddMaps(typeof(Program));
    });

    builder.Services.AddDbContextFactory<RESTContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:DefaultConnection"]
        ));

    // Validate evey endpoint in the API. This validates the data annotations specified for each field of Dtos.
    builder.Services.AddValidation();

    // Add an authentication scheme
    builder.Services
      .AddAuthentication()

      // Requires Microsoft.AspNetCore.Authentication.JwtBearer
      .AddJwtBearer(options =>
      {
        // URL of the tenant that generates the access tokens. This is the OpenID Connect metadata document value
        // from the Entra application's Endpoints.
        // So basically, this is the token-issuing authority.
        options.Authority = "https://016874f5-6379-40f5-a2e8-a27da43c3a3f.ciamlogin.com/016874f5-6379-40f5-a2e8-a27da43c3a3f/v2.0";

        // A unique value that identifies who these tokens are intended for, i.e., a unique identifier
        // of the API that the access token is intended for. This value comes from the Entra application Client ID.
        options.Audience = "bfc23544-10df-48fa-bfa2-5f0ba5703206";
      })
      ;

    // Add middleware to enforce the authorization policy that has been defined in the application.
    builder.Services.AddAuthorizationBuilder();

    var app = builder.Build();

    app.MapRESTEndpoints();

    app.Run();
  }
}
