using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Tests.Unit.Infrastructure.Repositories.Notes;

public class GetAllTests : NoteRepositoryTestsBase
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllNotes()
    {
        var notes = await Repository.GetAllAsync(CancellationToken.None);

        Assert.NotNull(notes);
        Assert.Equal(2, notes.Count());
    }
}
