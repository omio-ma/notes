using Notes.Application.Models.Requests;
using Notes.Application.Models.Responses;

namespace Notes.Application.Interfaces;
public interface INoteService
{
    Task<IEnumerable<NoteResponse>> GetAllNotesAsync(CancellationToken cancellationToken = default);
    Task<NoteResponse?> GetNoteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(NoteRequest request, CancellationToken cancellationToken = default);
    Task<NoteResponse?> UpdateAsync(int id, NoteRequest request, CancellationToken cancellationToken = default);
    Task<NoteResponse?> PatchAsync(int id, NoteRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

}