using Notes.Application.Models.Requests;
using Notes.Domain.Entities;

namespace Notes.Application.Interfaces;
public interface INoteService
{
    Task<IEnumerable<Note>> GetAllNotesAsync(CancellationToken cancellationToken = default);
    Task<Note?> GetNoteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(NoteRequest request, CancellationToken cancellationToken = default);

}