using System.ComponentModel.DataAnnotations;

namespace MovieTrackerMVC.Models
{
    /// <summary>
    /// Links the users with movies and preferences about them.
    /// </summary>
    public class UserMedia
    {
        public long UserId { get; set; }

        public long MediaId { get; set; }

        public long? UserMediaNotesId { get; set; }

        public long? UserMediaStatusId { get; set; }

        public float? Score { get; set; }


        public virtual required Media Media { get; set; }
        public virtual required User User { get; set; }
        public virtual UserMediaNote? UserMediaNotes { get; set; }
        public virtual UserMediaStatus? UserMediaStatus { get; set; }
    }
}
