﻿namespace Notes.API.Requests
{
    public class NoteRequest
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
}
