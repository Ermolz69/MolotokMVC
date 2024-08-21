using System.ComponentModel.DataAnnotations;

namespace MolotokMvc.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        [MaxLength(64)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        ////
        public List<Skill> Skill { get; set; }
    }
}
