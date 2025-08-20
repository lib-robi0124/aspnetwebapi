using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAnnotations.Domain.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; } // Nullable to allow for users without a first name
        [MaxLength(50)]
        public string? LastName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Usrername { get; set; } = string.Empty;
        [InverseProperty("User")] // Establishing a one-to-many relationship with Note
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}
