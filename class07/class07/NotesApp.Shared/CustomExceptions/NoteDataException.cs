namespace NotesApp.Shared.CustomExceptions
{
    public class NoteDataException : Exception
    {
        public NoteDataException(string message) : base(message)
        {
        }
        //public NoteDataException(string message, Exception innerException) : base(message, innerException)
        //{
        //}
    }
    
}
