using AutoFixture.Xunit2;
using Moq;
using Notes.Application.Models.Requests;
using Notes.Application.Services;
using Notes.Domain.Entities;
using Notes.Domain.Interfaces;

namespace Notes.Tests.Unit.Application.Services
{
    public class NoteServiceTests
    {
        [Fact]
        public async Task GetAllNotesAsync_ReturnsNotes()
        {
            // Arrange
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Note>
                {
                new Note { Title = "T1", Content = "C1", CreatedAt = DateTime.UtcNow },
                new Note { Title = "T2", Content = "C2", CreatedAt = DateTime.UtcNow }
                });

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.GetAllNotesAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetNoteByIdAsync_ReturnsCorrectNote()
        {
            // Arrange
            var note = new Note { Id = 1, Title = "One", Content = "First", CreatedAt = DateTime.UtcNow };

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(1, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(note);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.GetNoteByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("One", result!.Title);
        }

        [Fact]
        public async Task CreateAsync_ReturnsNewNoteId()
        {
            // Arrange
            var fakeRepo = new Mock<INoteRepository>();
            fakeRepo
                .Setup(r => r.CreateAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(42);

            var service = new NoteService(fakeRepo.Object);

            var request = new NoteRequest
            {
                Title = "Service Test",
                Content = "From test"
            };

            // Act
            var resultId = await service.CreateAsync(request);

            // Assert
            Assert.Equal(42, resultId);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_UpdatesNoteSuccessfully(
            NoteRequest request,
            int noteId
        )
        {
            // Arrange
            var expectedNote = new Note
            {
                Id = noteId,
                Title = request.Title,
                Content = request.Content
            };

            var mockRepo = new Mock<INoteRepository>();
            mockRepo
            .Setup(r => r.GetByIdAsync(noteId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedNote);

            mockRepo.Setup(r =>
                r.UpdateAsync(It.Is<Note>(n => n.Id == noteId), It.IsAny<CancellationToken>())
            ).ReturnsAsync(expectedNote);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.UpdateAsync(noteId, request);

            // Assert
            Assert.Equal(expectedNote.Title, result!.Title);
            Assert.Equal(expectedNote.Content, result.Content);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ReturnsNull_WhenNotFound(
            NoteRequest request,
            int noteId
        )
        {
            // Arrange
            var expectedNote = new Note
            {
                Id = noteId,
                Title = request.Title,
                Content = request.Content
            };

            var mockRepo = new Mock<INoteRepository>();
            mockRepo
            .Setup(r => r.GetByIdAsync(noteId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Note?)null);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.UpdateAsync(noteId, request);

            // Assert
            Assert.Null(result);
        }

        [Theory, AutoData]
        public async Task PatchAsync_ReturnsUpdatedNote_WhenSuccessful(NoteRequest request, int noteId)
        {
            // Arrange
            var existingNote = new Note
            {
                Id = noteId,
                Title = "Old Title",
                Content = "Old Content",
                CreatedAt = DateTime.UtcNow
            };

            var updatedNote = new Note
            {
                Id = noteId,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = existingNote.CreatedAt
            };

            var mockRepo = new Mock<INoteRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(noteId, false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingNote);

            mockRepo.Setup(r => r.UpdateAsync(It.Is<Note>(n =>
                n.Id == noteId &&
                n.Title == request.Title &&
                n.Content == request.Content
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedNote);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.PatchAsync(noteId, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Title, result!.Title);
            Assert.Equal(request.Content, result.Content);
        }

        [Theory, AutoData]
        public async Task PatchAsync_ReturnsNull_WhenNoteNotFound(NoteRequest request, int noteId)
        {
            // Arrange
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(noteId, false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Note?)null);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.PatchAsync(noteId, request);

            // Assert
            Assert.Null(result);
        }

        [Theory, AutoData]
        public async Task DeleteAsync_ReturnsTrue_WhenNoteExists(Note note)
        {
            // Arrange
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(note.Id, false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(note);
            mockRepo.Setup(r => r.DeleteAsync(note, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(note.Id);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.DeleteAsync(note, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task DeleteAsync_ReturnsFalse_WhenNoteDoesNotExist(int noteId)
        {
            // Arrange
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(noteId, false, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Note?)null);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.DeleteAsync(noteId);

            // Assert
            Assert.False(result);
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Note>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
