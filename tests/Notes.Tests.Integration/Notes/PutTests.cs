using Notes.Application.Models.Requests;
using Notes.Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Notes.Tests.Integration.Notes
{
    public class PutTests : NotesTestBase
    {
        [Fact]
        public async Task Put_UpdatesNoteSuccessfully()
        {
            // Arrange
            var existingNote = Context.Notes.First();
            var request = new NoteRequest
            {
                Title = "Updated Title",
                Content = "Updated Content"
            };

            // Act
            var response = await Client.PutAsJsonAsync($"/notes/{existingNote.Id}", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var updated = JsonSerializer.Deserialize<Note>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(updated);
            Assert.Equal("Updated Title", updated.Title);
            Assert.Equal("Updated Content", updated.Content);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidPayload = new
            {
                title = "",
                content = ""
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidPayload), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/notes/1", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            var payload = new
            {
                title = "Updated",
                content = "Updated content"
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PutAsync("/notes/9999", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
