using System.ComponentModel.DataAnnotations;

namespace MolotokMvc.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TagName { get; set; }

    }
}
