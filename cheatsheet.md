# ✅ Notes App Setup Summary (.NET 9 – Layered Architecture with EF Core + Tests)

## 🧱 Solution & Project Structure

```bash
dotnet new sln -n Notes

# Source projects
dotnet new webapi -n Notes.API
dotnet new classlib -n Notes.Application
dotnet new classlib -n Notes.Domain
dotnet new classlib -n Notes.Infrastructure

# Test projects
dotnet new xunit -n Notes.Tests.Unit
dotnet new xunit -n Notes.Tests.Integration

# Add to solution
dotnet sln add src/Notes.API
dotnet sln add src/Notes.Application
dotnet sln add src/Notes.Domain
dotnet sln add src/Notes.Infrastructure
dotnet sln add tests/Notes.Tests.Unit
dotnet sln add tests/Notes.Tests.Integration
```

### 📎 Project References

```bash
dotnet add src/Notes.API reference src/Notes.Application
dotnet add src/Notes.Application reference src/Notes.Domain
dotnet add src/Notes.Infrastructure reference src/Notes.Application
dotnet add src/Notes.Infrastructure reference src/Notes.Domain
dotnet add tests/Notes.Tests.Unit reference src/Notes.Infrastructure
dotnet add tests/Notes.Tests.Unit reference src/Notes.Domain
dotnet add tests/Notes.Tests.Integration reference src/Notes.API
```

---

## 🔧 Program.cs Refactor (No Minimal API)

Replaced top-level statements with explicit setup in `Notes.API/Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using Notes.Infrastructure.Persistence;

namespace Notes.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<NotesDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
```

---

## 🧪 Integration Test Setup

### ✅ Installed Package

```bash
dotnet add tests/Notes.Tests.Integration package Microsoft.AspNetCore.Mvc.Testing
```

### ✅ Base Classes

**CustomWebApplicationFactory.cs**

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Notes.API.Program> { }
```

**TestBase.cs**

```csharp
public abstract class TestBase
{
    protected static readonly CustomWebApplicationFactory Factory = new();
    protected readonly HttpClient Client;

    protected TestBase() => Client = Factory.CreateClient();
}
```

---

## 📦 EF Core Setup

### ✅ Installed Packages

```bash
# In Notes.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# In Notes.API (startup project)
dotnet add package Microsoft.EntityFrameworkCore.Design
```

---

## 🧾 Domain Entity

**Notes.Domain/Entities/Note.cs**

```csharp
public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

## 🛠 NotesDbContext

**Notes.Infrastructure/Persistence/NotesDbContext.cs**

```csharp
using Microsoft.EntityFrameworkCore;
using Notes.Domain.Entities;

public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Content).IsRequired();
        });
    }
}
```

---

## 🔌 SQL Server Config

**appsettings.Development.json**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NotesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

---

## 🛠 Migrations & DB Creation

```bash
dotnet ef migrations add InitialCreate --project src/Notes.Infrastructure --startup-project src/Notes.API
dotnet ef database update --project src/Notes.Infrastructure --startup-project src/Notes.API
```

➡ DB `NotesDb` is now created and visible in SSMS.
