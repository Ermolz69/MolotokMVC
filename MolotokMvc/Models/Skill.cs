using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MolotokMvc.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FK_User_123")]
        public int UserId { get; set; }
        public string SkillName { get; set; }
        public string SkillExperience { get; set; }

        ////
        public User User { get; set; }

    }
}
