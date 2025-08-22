using NotesApp.Dtos.NoteDtos;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        List<NoteDto> GetAllNotes();
        NoteDto GetById(int id);
        void AddNote(AddNoteDto addNoteDto);
        void UpdateNote(UpdateNoteDto noteDto);
        void DeleteNote(int id);
    }
}
