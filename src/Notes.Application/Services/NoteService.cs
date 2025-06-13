using Notes.API.Requests;
using Notes.Application.Interfaces;
using Notes.Domain.Entities;
using Notes.Domain.Interfaces;

namespace Notes.Application.Services;
public class NoteService : INoteService
{
    private readonly INoteRepository _repo;

    public NoteService(INoteRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<Note>> GetAllNotesAsync(CancellationToken cancellationToken = default)
    {
        return _repo.GetAllAsync(cancellationToken);
    }

    public Task<Note?> GetNoteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _repo.GetByIdAsync(id, cancellationToken);
    }

    public async Task<int> CreateAsync(NoteRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var note = new Note
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        var noteId = await _repo.CreateAsync(note, cancellationToken);
        return noteId;
    }
}