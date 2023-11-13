using Microsoft.AspNetCore.Identity;

namespace MovieTrackerMVC.Models
{
    /// <summary>
    /// The app user class. Stores informations about the user and theyre relations to the movies.
    /// </summary>
    public class User : IdentityUser<long>
    {
        private HashSet<UserMedia>? _userMedia;



        public virtual HashSet<UserMedia> UserMedia => _userMedia ??= new HashSet<UserMedia>();
    }
}
