using System.ComponentModel.DataAnnotations;

namespace MovieTrackerMVC.Models
{
    /// <summary>
    /// User's private notes on a specific media
    /// </summary>
    public class UserMediaNote
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserMediaId { get; set; }

        [Required]
        public string Note {  get; set; } = string.Empty;

        
        public virtual required UserMedia UserMedia { get; set; }
    }
}
