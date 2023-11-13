using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieTrackerMVC.Models;
using System.Drawing;

namespace MovieTrackerMVC.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Media> Media { get; set; }
    public DbSet<UserMedia> UserMedia { get; set; }
    public DbSet<UserMediaNote> UserMediaNotes { get; set; }
    public DbSet<UserMediaStatus> UserMediaStatus { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);


        builder.Entity<UserMediaStatus>()
            .Property(x => x.Color)
            .HasConversion<ColorToInt32Converter>();

        builder.Entity<UserMedia>()
            .HasKey(x => new { x.UserId, x.MediaId });
    }


    private class ColorToInt32Converter : ValueConverter<Color, int>
    {
        public ColorToInt32Converter() : base(c => c.ToArgb(), v => Color.FromArgb(v)) { }
    }
}
