using Microsoft.AspNetCore.Mvc;
using Moq;
using Notes.API.Controllers;
using Notes.API.Requests;
using Notes.Application.Interfaces;
using Notes.Domain.Entities;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Notes.Tests.Unit.Controllers
{
    public class NotesControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkResultWithNotes()
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.GetAllNotesAsync(default))
                .ReturnsAsync(new List<Note>
                {
                new Note { Title = "Note A", Content = "A", CreatedAt = DateTime.UtcNow },
                new Note { Title = "Note B", Content = "B", CreatedAt = DateTime.UtcNow }
                });

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var notes = Assert.IsAssignableFrom<IEnumerable<Note>>(okResult.Value);
            Assert.Equal(2, notes.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenNoteExists()
        {
            var note = new Note { Id = 1, Title = "Note X", Content = "Test", CreatedAt = DateTime.UtcNow };

            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.GetNoteByIdAsync(1, default)).ReturnsAsync(note);

            var controller = new NotesController(mockService.Object);

            var result = await controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedNote = Assert.IsType<Note>(okResult.Value);
            Assert.Equal(1, returnedNote.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.GetNoteByIdAsync(999, default)).ReturnsAsync((Note?)null);

            var controller = new NotesController(mockService.Object);

            var result = await controller.GetById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateNote_ReturnsCreatedAtActionWithId()
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService
                .Setup(s => s.CreateAsync(It.IsAny<NoteRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var controller = new NotesController(mockService.Object);

            var request = new NoteRequest
            {
                Title = "New Note",
                Content = "Content"
            };

            // Act
            var result = await controller.Post(request);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(NotesController.GetById), createdAt.ActionName);
            Assert.Equal(1, createdAt.RouteValues["id"]);
        }

        [Fact]
        public async Task CreateNote_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            var controller = new NotesController(mockService.Object);

            // Simulate model validation failure
            controller.ModelState.AddModelError("Title", "The Title field is required.");

            var invalidRequest = new NoteRequest(); 

            // Act
            var result = await controller.Post(invalidRequest);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errorDetails = Assert.IsType<SerializableError>(badRequest.Value);
            Assert.True(errorDetails.ContainsKey("Title"));
        }
    }
}
