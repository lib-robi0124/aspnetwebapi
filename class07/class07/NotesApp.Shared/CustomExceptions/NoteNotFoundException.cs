namespace NotesApp.Shared.CustomExceptions
{
    public class NoteNotFoundException : Exception
    {
        public NoteNotFoundException(int id) : base($"Note with ID {id} not found.")
        {
        }
        public NoteNotFoundException(string message) : base(message)
        {
        }
        public NoteNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    
}
