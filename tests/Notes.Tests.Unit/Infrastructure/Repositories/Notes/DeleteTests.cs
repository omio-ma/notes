namespace Notes.Tests.Unit.Infrastructure.Repositories.Notes
{
    public class DeleteTests : NoteRepositoryTestsBase
    {
        [Fact]
        public async Task DeleteAsync_RemovesNoteSuccessfully()
        {
            // Arrange
            var noteToDelete = Context.Notes.First();

            // Act
            await Repository.DeleteAsync(noteToDelete);

            // Assert
            var deleted = Context.Notes.FirstOrDefault(n => n.Id == noteToDelete.Id);
            Assert.Null(deleted);
        }
    }
}
