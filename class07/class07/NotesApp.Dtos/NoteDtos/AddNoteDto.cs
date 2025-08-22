using NotesApp.Domain.Enums;

namespace NotesApp.Dtos.NoteDtos
{
    public class AddNoteDto
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public Priority Priority { get; set; }
        public Tag Tag { get; set; }    

    }
}
