namespace Notes.Tests.Unit.Infrastructure.Repositories.Notes;

public class GetByIdTests : NoteRepositoryTestsBase
{
    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectNote()
    {
        var result = await Repository.GetByIdAsync(1, true, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("One", result!.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNoteDoesNotExist()
    {
        var result = await Repository.GetByIdAsync(999, true, CancellationToken.None);

        Assert.Null(result);
    }
}
