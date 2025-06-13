using Notes.Domain.Entities;
using Notes.Infrastructure.Persistence;
using Notes.Infrastructure.Repositories;
using Notes.Tests.Unit.Common;

namespace Notes.Tests.Unit.Infrastructure.Repositories.Notes;

public abstract class NoteRepositoryTestsBase : IDisposable
{
    protected readonly NotesDbContext Context;
    protected readonly NoteRepository Repository;

    protected NoteRepositoryTestsBase()
    {
        Context = TestDbContextFactory.CreateInMemoryContext();
        SeedData();
        Repository = new NoteRepository(Context);
    }

    private void SeedData()
    {
        Context.Notes.AddRange(new[]
        {
            new Note { Id = 1, Title = "One", Content = "First", CreatedAt = DateTime.UtcNow },
            new Note { Id = 2, Title = "Two", Content = "Second", CreatedAt = DateTime.UtcNow }
        });
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
