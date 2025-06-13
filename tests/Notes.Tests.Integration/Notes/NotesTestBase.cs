using Microsoft.EntityFrameworkCore;
using Notes.Domain.Entities;
using Notes.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NotesDb_Test;Trusted_Connection=True;TrustServerCertificate=True;")
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
            //await Context.Database.EnsureDeletedAsync();
            await Context.DisposeAsync();
        }
    }
}
