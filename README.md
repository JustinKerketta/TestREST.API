
### 1. Docker command to Build Latex documents
docker run --name LatexNotesBuild --rm -v ".:/data" blang/latex pdflatex DevNotes.tex

### 2. Docker command to run SQL Server container
```
docker run -d `
   --name sql1 `
   -e "ACCEPT_EULA=y" `
   -e 'MSSQL_SA_PASSWORD=JustinK11%#' `
   -p 1433:1433 `
   -v sqlServerVolume:/var/opt/mssql `
   mcr.microsoft.com/mssql/server:2025-latest
```

### 3. Docker command to list volumes
```
docker volume ls
```

### 4. Docker command to inspect volumes

`docker volume inspect` *volumeName*


### 5. Install dotnet EF tools
dotnet tool install --global dotnet-ef --version 10.0.5

### 6. EF Core Migrations
`dotnet ef migrations add InitialCreate --output-dir` *.\Database\Migrations*

### 7. Remove all database updates
dotnet ef database update 0

### 8. Add an extension class that can be called on startup from `Program.cs` to automatically run database migrations
```
public static class DataExtensions
{
  public static void MigrateDb(this WebApplication app)
  {
    // Retrieve the scope
    using var scope = app.Services.CreateScope();

    // Create an instance of the database context
    var dbContext = scope.ServiceProvider.GetRequiredService<>(yourDbContext);

    // Use the database context to access the datbase context and run migrations
    dbContext.Database.Migrate();
  }
}
```

#### 8.1 Now in `Program.cs`, call to `app.MigrateDb()` before `app.Run()`
```
.
.
.
app.MigrateDb();
.
.
app.Run();
```
### 9. Connection string specified in `appsettings.json` for SQL Server (Notice EF Core logging changed as well)
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost,1433;Persist Security Info=True;User ID=sa;Password=Mssql2025!;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=0;Database=RESTAPI;"
  }
}
```

#### 9.1 Update `Program.cs` to add the database context to your application's services collection
```
builder.Services.AddDbContextFactory<CustomDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:DefaultConnection"])
);
```
