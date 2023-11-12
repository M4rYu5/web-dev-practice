using Microsoft.EntityFrameworkCore;

namespace MovieTrackerMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }


    }
}
