using Microsoft.EntityFrameworkCore;
using Notes.Domain.Entities;
using Notes.Domain.Interfaces;
using Notes.Infrastructure.Persistence;

namespace Notes.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly NotesDbContext _context;

    public NoteRepository(NotesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Note>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Notes.ToListAsync(cancellationToken);
    }

    public async Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Notes.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<int> CreateAsync(Note note, CancellationToken cancellationToken = default)
    {
        await _context.Notes.AddAsync(note, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return note.Id;
    }
}