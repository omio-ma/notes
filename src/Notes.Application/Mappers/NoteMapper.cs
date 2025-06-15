using Notes.Application.Models.Responses;
using Notes.Domain.Entities;

namespace Notes.Application.Mappers
{
    public static class NoteMapper
    {
        public static NoteResponse ToResponse(Note note)
        {
            return new NoteResponse
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt
            };
        }
    }
}
