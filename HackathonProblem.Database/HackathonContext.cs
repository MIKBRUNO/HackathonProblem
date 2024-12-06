using HackathonProblem.Database.DataTypes;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackathonProblem.Database;

public class HackathonContext : DbContext
{
    public required DbSet<Junior> Juniors { get; set; }
    public required DbSet<Teamlead> Teamleads { get; set; }

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
    }

    private static void ConfigureEmployee<T>(EntityTypeBuilder<T> entity) where T : Employee
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).IsRequired();
    }
}
