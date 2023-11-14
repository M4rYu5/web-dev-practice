using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace MovieTrackerMVC.Models
{
    public class UserMediaStatus
    {
        [Key]
        public long Id { get; set; }

        [Required] 
        public string Name { get; set; } = string.Empty;

        [Required]
        public Color Color { get; set; }
    }
}
