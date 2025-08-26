namespace NotesApp.Domain.Models
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        //public string Password { get; set; }
        public List<Note> Notes { get; set; }
        //[NotMapped] //will not be sent (mapped) to the db
        //public int Age { get; set; }
    }
}
