using Microsoft.EntityFrameworkCore;
using Notes.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace Notes.Tests.Integration.Notes
{
    [Collection("Sequential Integration Tests")]
    public class GetByIdTests : NotesTestBase
    {
        [Fact]
        public async Task GetById_ReturnsExpectedNote()
        {
            // Arrange
            var note = await Context.Notes.FirstAsync(n => n.Title == "Test A");

            // Act
            var response = await Client.GetAsync($"/notes/{note.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var returnedNote = JsonSerializer.Deserialize<Note>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(returnedNote);
            Assert.Equal("Test A", note.Title);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var response = await Client.GetAsync("/notes/99999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
