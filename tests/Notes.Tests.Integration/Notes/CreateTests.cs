using Microsoft.AspNetCore.Mvc;
using Notes.Application.Models.Requests;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Notes.Tests.Integration.Notes
{
    [Collection("Sequential Integration Tests")]
    public class CreateTests : NotesTestBase
    {
        [Fact]
        public async Task CreateNote_ReturnsBadRequest_WhenInvalidRequest()
        {
            // Arrange
            var invalidRequest = new NoteRequest
            {
                Title = "", 
                Content = ""
            };

            // Act
            var response = await Client.PostAsJsonAsync("/notes", invalidRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            // Optional: deserialize validation error
            var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(
                responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(problemDetails);
            Assert.Contains("Title", problemDetails.Errors.Keys);
            Assert.Contains("Content", problemDetails.Errors.Keys);
        }

        [Fact]
        public async Task CreateNote_ReturnsCreated_WhenRequestIsValid()
        {
            // Arrange
            var validRequest = new NoteRequest
            {
                Title = "Valid Title",
                Content = "Valid content"
            };

            // Act
            var response = await Client.PostAsJsonAsync("/notes", validRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }
    }
}
