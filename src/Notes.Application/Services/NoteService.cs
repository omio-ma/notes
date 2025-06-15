using Notes.Application.Interfaces;
using Notes.Application.Mappers;
using Notes.Application.Models.Requests;
using Notes.Application.Models.Responses;
using Notes.Domain.Interfaces;

namespace Notes.Application.Services;
public class NoteService : INoteService
{
    private readonly INoteRepository _repo;

    public NoteService(INoteRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<NoteResponse>> GetAllNotesAsync(CancellationToken cancellationToken = default)
    {
        var notes = await _repo.GetAllAsync(cancellationToken);
        return notes.Select(NoteMapper.ToResponse);
    }

    public async Task<NoteResponse?> GetNoteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await _repo.GetByIdAsync(id, cancellationToken: cancellationToken);
        return note == null ? null : NoteMapper.ToResponse(note);
    }

    public async Task<int> CreateAsync(NoteRequest request, CancellationToken cancellationToken = default)
    {
        var noteId = await _repo.CreateAsync(request.Map(), cancellationToken);

        return noteId;
    }

    public async Task<NoteResponse?> UpdateAsync(int id, NoteRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdAsync(id, false, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        var note = request.Map();
        note.Id = id;

        return NoteMapper.ToResponse(await _repo.UpdateAsync(note, cancellationToken));
    }

    public async Task<NoteResponse?> PatchAsync(int id, NoteRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByIdAsync(id, false, cancellationToken);
        if (existing is null)
        {
            return null;
        }
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            existing.Title = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.Content))
        {
            existing.Content = request.Content;
        }

        var updated = await _repo.UpdateAsync(existing, cancellationToken);
        return NoteMapper.ToResponse(updated);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}