
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

