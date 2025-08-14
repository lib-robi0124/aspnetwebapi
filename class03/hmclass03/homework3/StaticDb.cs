using homework3.Models;

namespace homework3
{
    public static class StaticDb
    {
        // List of books
        public static List<Book> Books = new List<Book>()
        {
            new Book() { Author = "George Orwell", Title = "1984" },
            new Book() { Author = "Harper Lee", Title = "To Kill a Mockingbird" },
            new Book() { Author = "J.R.R. Tolkien", Title = "The Hobbit" }
        };
    }
}
