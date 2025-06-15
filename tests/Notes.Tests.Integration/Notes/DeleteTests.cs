using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Notes.Tests.Integration.Notes
{
    public class DeleteTests : NotesTestBase
    {
        [Fact]
        public async Task Delete_DeletesNoteSuccessfully()
        {
            // Arrange
            var existingNote = Context.Notes.AsNoTracking().First();

            // Act
            var response = await Client.DeleteAsync($"/notes/{existingNote.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var deletedNote = await Context.Notes
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == existingNote.Id);
            Assert.Null(deletedNote);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            // Act
            var response = await Client.DeleteAsync("/notes/9999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
