﻿using NotesApp.Domain.Enums;

namespace NotesApp.Dtos.NoteDtos
{
    public class NoteDto
    {
        public string Text { get; set; }
        public Priority Priority { get; set; }
        public Tag Tag { get; set; }
        public string UserFullName { get; set; }
    }
}
