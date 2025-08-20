using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluentAPI.Domain.Models
{
    internal class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FirstName { get; set; } // Nullable to allow for users without a first name
        public string? LastName { get; set; }
        public string Usrername { get; set; }
        public List<Note> Notes { get; set; }
       public int Age { get; set; } = 0; // Default age to 0 if not specified
       
       
    }
}
