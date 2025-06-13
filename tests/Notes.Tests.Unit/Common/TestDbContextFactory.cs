using Microsoft.EntityFrameworkCore;
using Notes.Infrastructure.Persistence;

namespace Notes.Tests.Unit.Common;

public static class TestDbContextFactory
{
    public static NotesDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<NotesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new NotesDbContext(options);
    }
}
