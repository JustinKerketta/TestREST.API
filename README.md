
### 1. Docker command to Build Latex documents
docker run --name LatexNotesBuild --rm -v ".:/data" blang/latex pdflatex DevNotes.tex

### 2. Install dotnet EF tools
dotnet tool install --global dotnet-ef --version 10.0.5

### 3. EF Core Migrations
dotnet ef migrations add InitialCreate --output-dir .\Database\Migrations

### 4. Remove all database updates
dotnet ef database update 0

### 5. Add an extension class that can be called on startup from `Program.cs` to automatically run database migrations
```
public static class DataExtensions
{
  public static void MigrateDb(this WebApplication app)
  {
    // Retrieve the scope
    using var scope = app.Services.CreateScope();

    // Create an instance of the database context
    var dbContext = scope.ServiceProvider.getRequiredService<>(yourDbContext);

    // Use the database context to access the datbase context and run migrations
    dbContext.Database.Migrate();
  }
}
```

#### 5.1 Now in `Program.cs`, call to `app.MigrateDb()` before `app.Run()`
```
.
.
.
app.MigrateDb();
.
.
app.Run();
```
### 6 Connection string specified in `appsettings.json` for SQL Server
```
"ConnectionStrings": {
  "DefaultConnection": "Data Source=localhost,1433;Persist Security Info=True;User ID=sa;Password=CustomPassword;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=0;Database=DatabaseName;"
}
```

#### 6.1 Update `Program.cs` to add the database context to your application's services collection
```
builder.Services.AddDbContextFactory<CustomDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:DefaultConnection"])
);


```
