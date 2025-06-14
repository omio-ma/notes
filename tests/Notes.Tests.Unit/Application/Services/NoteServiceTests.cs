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
            mockRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
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
    }
}
