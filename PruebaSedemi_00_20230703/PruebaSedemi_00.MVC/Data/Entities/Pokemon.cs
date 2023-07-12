using System.ComponentModel.DataAnnotations;

namespace PruebaSedemi_00.MVC.Data.Entities
{
    public class Pokemon
    {
        [Required]
        [Key]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public bool IsSelected { get; set; }
    }
}
