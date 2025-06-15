using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Notes.API.Controllers;
using Notes.Application.Interfaces;
using Notes.Application.Models.Requests;
using Notes.Application.Models.Responses;

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
                .ReturnsAsync(new List<NoteResponse>
                {
            new NoteResponse { Id = 1, Title = "Note A", Content = "A", CreatedAt = DateTime.UtcNow },
            new NoteResponse { Id = 2, Title = "Note B", Content = "B", CreatedAt = DateTime.UtcNow }
                });

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var notes = Assert.IsAssignableFrom<IEnumerable<NoteResponse>>(okResult.Value);
            Assert.Equal(2, notes.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenNoteExists()
        {
            var noteResponse = new NoteResponse
            {
                Id = 1,
                Title = "Note X",
                Content = "Test",
                CreatedAt = DateTime.UtcNow
            };

            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.GetNoteByIdAsync(1, default)).ReturnsAsync(noteResponse);

            var controller = new NotesController(mockService.Object);

            var result = await controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedNote = Assert.IsType<NoteResponse>(okResult.Value);
            Assert.Equal(1, returnedNote.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.GetNoteByIdAsync(999, default)).ReturnsAsync((NoteResponse?)null);

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

        [Theory, AutoData]
        public async Task UpdateNote_SuccessfullyUpdatesNote_AndReturnsNote(
            int noteId,
            NoteRequest validRequest,
            NoteResponse expectedResponse
        )
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService
                .Setup(s => s.UpdateAsync(noteId, validRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Put(noteId, validRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedNote = Assert.IsType<NoteResponse>(okResult.Value);
            Assert.Equal(expectedResponse.Id, returnedNote.Id);
            Assert.Equal(expectedResponse.Title, returnedNote.Title);
            Assert.Equal(expectedResponse.Content, returnedNote.Content);
        }

        [Theory, AutoData]
        public async Task UpdateNote_ReturnsNotFound(
            int noteId,
            NoteRequest request
        )
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService
                .Setup(s => s.UpdateAsync(noteId, request, It.IsAny<CancellationToken>()))
                .ReturnsAsync((NoteResponse?)null);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Put(noteId, request);

            // Assert
            var okResult = Assert.IsType<NotFoundResult>(result);
        }

        [Theory, AutoData]
        public async Task UpdateNote_ReturnsBadRequest_WhenModelIsInvalid(
            int noteId,
            NoteRequest request
        )
        {
            // Arrange
            //request.Title = string.Empty;
            var mockService = new Mock<INoteService>();
            var controller = new NotesController(mockService.Object);

            // Simulate model validation failure
            controller.ModelState.AddModelError("Title", "The Title field is required.");

            var invalidRequest = new NoteRequest();

            // Act
            var result = await controller.Put(noteId, request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errorDetails = Assert.IsType<SerializableError>(badRequest.Value);
            Assert.True(errorDetails.ContainsKey("Title"));
        }

        [Theory, AutoData]
        public async Task Patch_ReturnsOk_WhenNoteIsUpdated(
            int noteId,
            NoteRequest patchRequest,
            NoteResponse patchedResponse
        )
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService
                .Setup(s => s.PatchAsync(noteId, patchRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patchedResponse);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Patch(noteId, patchRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedNote = Assert.IsType<NoteResponse>(okResult.Value);
            Assert.Equal(patchedResponse.Title, returnedNote.Title);
        }

        [Theory, AutoData]
        public async Task Patch_ReturnsNotFound_WhenNoteDoesNotExist(
            int noteId,
            NoteRequest patchRequest
        )
        {
            // Arrange
            var mockService = new Mock<INoteService>();
            mockService
                .Setup(s => s.PatchAsync(noteId, patchRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync((NoteResponse?)null);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Patch(noteId, patchRequest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
