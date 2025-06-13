using Microsoft.EntityFrameworkCore;
using Notes.Domain.Entities;
using Notes.Infrastructure.Repositories;
using Notes.Tests.Unit.Common;

namespace Notes.Tests.Unit.Infrastructure.Repositories.Notes
{
    public class CreateTests
    {
        [Fact]
        public async Task CreateAsync_AddsNoteSuccessfully()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            var repository = new NoteRepository(context);

            var note = new Note
            {
                Title = "Created Note",
                Content = "This is a test note.",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await repository.CreateAsync(note);

            // Assert
            var savedNote = await context.Notes.FirstOrDefaultAsync(n => n.Title == "Created Note");
            Assert.NotNull(savedNote);
            Assert.Equal("This is a test note.", savedNote!.Content);
        }

    }
}
