using Notes.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace Notes.Tests.Integration.Notes;

[Collection("Sequential Integration Tests")]
public class GetAllTests : NotesTestBase
{
    [Fact]
    public async Task GetAllNotes_Returns200Ok()
    {
        var response = await Client.GetAsync("/notes");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllNotes_ReturnsSeededNotes()
    {
        var response = await Client.GetAsync("/notes");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        var notes = JsonSerializer.Deserialize<List<Note>>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(notes);
        Assert.Equal(2, notes.Count);
    }
}