using HackathonProblem.Database.DataTypes;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackathonProblem.Database;

public class HackathonContext : DbContext
{
    public required DbSet<Junior> Juniors { get; set; }
    public required DbSet<Teamlead> Teamleads { get; set; }
    public required DbSet<Hackathon> Hackathons { get; set; }
    public required DbSet<TeamleadWishlist> TeamleadWishlists { get; set; }
    public required DbSet<JuniorWishlist> JuniorWishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = new SqliteConnectionStringBuilder() { DataSource = "HackathonProblem.db" }.ToString();
		optionsBuilder.UseSqlite(connectionString);
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Junior>(ConfigureEmployee);
        modelBuilder.Entity<Teamlead>(ConfigureEmployee);
        modelBuilder.Entity<Hackathon>(ConfigureHackathon);
        modelBuilder.Entity<JuniorWishlist>(ConfigureWishlist<JuniorWishlist, Junior, Teamlead>);
        modelBuilder.Entity<TeamleadWishlist>(ConfigureWishlist<TeamleadWishlist, Teamlead, Junior>);
        modelBuilder.Entity<Rate<Junior>>(ConfigureRate);
        modelBuilder.Entity<Rate<Teamlead>>(ConfigureRate);
    }

    private static void ConfigureEmployee<T>(EntityTypeBuilder<T> entity) where T : Employee
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired();
    }

    private static void ConfigureHackathon(EntityTypeBuilder<Hackathon> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasMany(e => e.TeamleadWishlists).WithOne().HasForeignKey(e => e.HackathonId);
        entity.HasMany(e => e.JuniorWishlists).WithOne().HasForeignKey(e => e.HackathonId);
    }

    private static void ConfigureWishlist<T, O, M>(EntityTypeBuilder<T> entity)
        where T : CommonWishlist<O, M>
        where O : Employee
        where M : Employee
    {
        entity.HasKey(e => e.Id);
        entity.HasMany(e => e.Mates).WithOne().HasForeignKey(e => e.WishlistId);
        entity.HasOne(e => e.Owner).WithMany().HasForeignKey(e => e.OwnerId);
    }

    private static void ConfigureRate<T>(EntityTypeBuilder<Rate<T>> entity) where T : Employee
    {
        entity.HasKey(e => new { e.WishlistId, e.Rating });
        entity.HasOne(e => e.Mate).WithMany();
    }
}
