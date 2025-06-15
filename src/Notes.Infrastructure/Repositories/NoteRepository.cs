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

    public async Task<Note?> GetByIdAsync(int id, bool track = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Notes.AsQueryable();

        if (!track)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<int> CreateAsync(Note note, CancellationToken cancellationToken = default)
    {
        await _context.Notes.AddAsync(note, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return note.Id;
    }

    public async Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync(cancellationToken);
        return note;
    }
}