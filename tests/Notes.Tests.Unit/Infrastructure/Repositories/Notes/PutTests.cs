namespace Notes.Tests.Unit.Infrastructure.Repositories.Notes
{
    public class PutTests : NoteRepositoryTestsBase
    {
        [Fact]
        public async Task UpdateAsync_UpdatesNoteSuccessfully()
        {
            // Arrange
            var originalNote = Context.Notes.First(n => n.Id == 1);
            originalNote.Title = "New Title";
            originalNote.Content = "New Content";

            // Act
            var result = await Repository.UpdateAsync(originalNote);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Title", result!.Title);
            Assert.Equal("New Content", result.Content);
        }
    }
}
