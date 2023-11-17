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

        builder.Entity<UserMediaStatus>()
            .HasData(
                // You should not need to change this. If you do, read the limitations of seed data
                // https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#limitations-of-model-seed-data
                // hint: Id values; hint2: careful on DELETE, make sure appropiate fields get updated
                new UserMediaStatus { Id = -100, Name = "Now", Color = ColorTranslator.FromHtml("#008000") },
                new UserMediaStatus { Id = -90, Name = "Finished", Color = ColorTranslator.FromHtml("#0000FF") },
                new UserMediaStatus { Id = -85, Name = "Next", Color = ColorTranslator.FromHtml("#800080") },
                new UserMediaStatus { Id = -80, Name = "Interested", Color = ColorTranslator.FromHtml("#800080") },
                new UserMediaStatus { Id = -70, Name = "On Hold", Color = ColorTranslator.FromHtml("#FFFF00") },
                new UserMediaStatus { Id = -60, Name = "Dropped", Color = ColorTranslator.FromHtml("#FF0000") },
                new UserMediaStatus { Id = -50, Name = "Uninterested", Color = ColorTranslator.FromHtml("#808080") });
    }


    private class ColorToInt32Converter : ValueConverter<Color, int>
    {
        public ColorToInt32Converter() : base(c => c.ToArgb(), v => Color.FromArgb(v)) { }
    }
}
