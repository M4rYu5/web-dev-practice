using System.ComponentModel.DataAnnotations;

namespace MovieTrackerMVC.Models
{
    /// <summary>
    /// Store the media details, like Title. Media can be videos, movies or other non-video types.
    /// </summary>
    public class Media
    {
        private HashSet<UserMedia>? _userMedia;


        [Key]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;


        public virtual HashSet<UserMedia> UserMedia => _userMedia ??= new HashSet<UserMedia>();
    }
}
