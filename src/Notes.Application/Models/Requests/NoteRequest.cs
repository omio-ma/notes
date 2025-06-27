using Notes.Domain.Entities;

namespace Notes.Application.Models.Requests
{
    public class NoteRequest : IMapTo<Note>
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        public Note Map()
        {
           return new Note
           {
               Title = Title,
               Content = Content,
           };
        }
    }
}
