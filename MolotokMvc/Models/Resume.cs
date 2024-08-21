using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MolotokMvc.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FK_UserResume_123")]
        public int UserId { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; }                  //open or closed

        ////
        public User User { get; set; }

    }
}
