using Notes.Domain.Entities;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Notes.Tests.Integration.Notes
{
    [Collection("Sequential Integration Tests")]
    public class PatchTests : NotesTestBase
    {
        [Fact]
        public async Task Patch_UpdatesPartialNoteSuccessfully()
        {
            // Arrange
            var existingNote = Context.Notes.First();
            var partialRequest = new { title = "Partially Updated Title", content = existingNote.Content };
            var content = new StringContent(JsonSerializer.Serialize(partialRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PatchAsync($"/notes/{existingNote.Id}", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updated = JsonSerializer.Deserialize<Note>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(updated);
            Assert.Equal("Partially Updated Title", updated!.Title);
            Assert.Equal(existingNote.Content, updated.Content); // unchanged
        }

        [Fact]
        public async Task Patch_UpdatesFullNoteSuccessfully()
        {
            // Arrange
            var existingNote = Context.Notes.First();
            var fullRequest = new { title = "Fully Updated", content = "New Content" };
            var content = new StringContent(JsonSerializer.Serialize(fullRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PatchAsync($"/notes/{existingNote.Id}", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updated = JsonSerializer.Deserialize<Note>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(updated);
            Assert.Equal("Fully Updated", updated!.Title);
            Assert.Equal("New Content", updated.Content);
        }

        [Fact]
        public async Task Patch_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var badRequest = new { title = "" }; // empty title is invalid
            var content = new StringContent(JsonSerializer.Serialize(badRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PatchAsync("/notes/1", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Patch_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            // Arrange
            var request = new { title = "Doesn't matter", content = "Doesnt matter" };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PatchAsync("/notes/99999", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
