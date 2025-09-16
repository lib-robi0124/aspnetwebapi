namespace SEDC.MovieApp.Shared
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException(string message) : base(message)
        {
        }
    }
}
