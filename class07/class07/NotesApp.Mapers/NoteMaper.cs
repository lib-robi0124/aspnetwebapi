using NotesApp.Domain.Models;
using NotesApp.Dtos.NoteDtos;

namespace NotesApp.Mapers
{
    public static class NoteMaper
    {
        public static NoteDto ToNoteDto(this Note note)
        {
            return new NoteDto
            {
                Tag = note.Tag,
                Priority = note.Priority,
                Text = note.Text,
                UserFullName = $"{note.User.FirstName} {note.User.LastName}",
            };
        }
        public static Note ToNote(this AddNoteDto addNoteDto)
        {
            return new Note
            {
                Text = addNoteDto.Text,
                Priority = addNoteDto.Priority,
                Tag = addNoteDto.Tag,
                UserId = addNoteDto.UserId

            };
        }
        public static Note ToNote(this UpdateNoteDto noteDto, Note noteDb)
        {
           noteDb.Text = noteDto.Text;
            noteDb.Priority = noteDto.Priority;
            noteDb.Tag = noteDto.Tag;
            return noteDb;
        }
    }
}
