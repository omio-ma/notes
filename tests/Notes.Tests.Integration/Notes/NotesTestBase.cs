using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notes.Domain.Entities;
using Notes.Infrastructure.Persistence;
using System.Data.Common;

namespace Notes.Tests.Integration.Notes
{
    public abstract class NotesTestBase : IAsyncLifetime
    {
        protected readonly HttpClient Client;
        protected readonly NotesDbContext Context;

        private readonly DbConnection _connection;

        protected NotesTestBase()
        {
            Client = new CustomWebApplicationFactory().CreateClient();

            // Load config from appsettings.Test.json and/or environment variables
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Test.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            Context = new NotesDbContext(options);
        }

        public async Task InitializeAsync()
        {
            await Context.Database.EnsureCreatedAsync();

            // Clean up any leftover data first
            Context.Notes.RemoveRange(Context.Notes);
            await Context.SaveChangesAsync();

            // Seed test data
            Context.Notes.AddRange(
                new Note { Title = "Test A", Content = "A", CreatedAt = DateTime.UtcNow },
                new Note { Title = "Test B", Content = "B", CreatedAt = DateTime.UtcNow }
            );
            await Context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            // Clean up after test
            Context.Notes.RemoveRange(Context.Notes);
            await Context.SaveChangesAsync();
            await Context.DisposeAsync();
        }
    }
}
