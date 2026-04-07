
### docker command to Build Latex documents
docker run --name LatexNotesBuild --rm -v ".:/data" blang/latex pdflatex DevNotes.tex

### Install dotnet EF tools
dotnet tool install --global dotnet-ef --version 10.0.5

### EF Core Migrations
dotnet ef migrations add InitialCreate --output-dir .\Database\Migrations

### Remove all database updates
dotnet ef database update 0


