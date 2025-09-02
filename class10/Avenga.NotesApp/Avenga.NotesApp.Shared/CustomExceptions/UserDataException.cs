namespace Avenga.NotesApp.Shared.CustomExceptions
{
    public class UserDataException : Exception
    {
        public UserDataException()
        {
        }
        public UserDataException(string message) : base(message)
        {
        }
        public UserDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
