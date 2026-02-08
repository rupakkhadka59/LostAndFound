using LostAndFound.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LostAndFound.Data;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
        : base(options)
    {
    }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemComment>ItemComments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public object Notification { get; internal set; }

    protected override void OnModelCreating (ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //for a item relationships
        builder.Entity<Item>()
            .HasOne(i => i.ReportedByUser)
            .WithMany(u => u.ReportedItems)
            .HasForeignKey(i => i.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Item>()
                   .HasOne(i => i.FoundByUser)
                   .WithMany()
                   .HasForeignKey(i => i.ReportedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);

        //for ItemComment REAltionshops
        builder.Entity<ItemComment>()
            .HasOne(c => c.Item)
            .WithMany(i => i.Comments)
            .HasForeignKey(c => c.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ItemComment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        //configure Notification relationships
        builder.Entity<Notification>()
            .HasOne(n => n.Item)
            .WithMany()
            .HasForeignKey(n => n.ItemId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Item>()
            .HasIndex(i => i.status);
        builder.Entity<Item>()
            .HasIndex(i => i.Category);
        builder.Entity<Item>()
            .HasIndex(i => i.DateReported);
        builder.Entity<Item>()
            .HasIndex(n => new { n.UserId, n.IsRead });
    }

    internal async Task SaveChangesASsync()
    {
        throw new NotImplementedException();
    }
}
