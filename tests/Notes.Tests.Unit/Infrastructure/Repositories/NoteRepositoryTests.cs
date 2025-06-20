using Notes.Domain.Entities;
using Notes.Infrastructure.Repositories;
using Notes.Tests.Unit.Common;

namespace Notes.Tests.Unit.Infrastructure.Repositories;

public class NoteRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllNotes()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryContext();
        context.Notes.AddRange(
            new Note { Title = "Test 1", Content = "Content 1", CreatedAt = DateTime.UtcNow },
            new Note { Title = "Test 2", Content = "Content 2", CreatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var repository = new NoteRepository(context);

        // Act
        var notes = await repository.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(notes);
        Assert.Equal(3, notes.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectNote()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryContext();
        context.Notes.AddRange(
            new Note { Id = 1, Title = "One", Content = "First", CreatedAt = DateTime.UtcNow },
            new Note { Id = 2, Title = "Two", Content = "Second", CreatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var repo = new NoteRepository(context);

        // Act
        var result = await repo.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("One", result!.Title);
    }
}
