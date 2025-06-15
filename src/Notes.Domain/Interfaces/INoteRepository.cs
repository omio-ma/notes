using Notes.Domain.Entities;

namespace Notes.Domain.Interfaces;
public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Note?> GetByIdAsync(int id, bool track = true, CancellationToken cancellationToken = default);

    Task<int> CreateAsync(Note note, CancellationToken cancellationToken = default);

    Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default);

}