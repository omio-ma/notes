using Microsoft.EntityFrameworkCore;
using Notes.Domain.Entities;

namespace Notes.Tests.Unit.Infrastructure.Repositories.Notes
{
    public class CreateTests : NoteRepositoryTestsBase
    {
        [Fact]
        public async Task CreateAsync_AddsNoteSuccessfully()
        {
            // Arrange
            var newNote = new Note
            {
                Title = "Created Note",
                Content = "This is a test note.",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await Repository.CreateAsync(newNote);

            // Assert
            var savedNote = await Context.Notes.FirstOrDefaultAsync(n => n.Title == "Created Note");
            Assert.NotNull(savedNote);
            Assert.Equal("This is a test note.", savedNote!.Content);
        }
    }
}
