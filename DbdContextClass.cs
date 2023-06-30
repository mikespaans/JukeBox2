using Microsoft.EntityFrameworkCore;
// using Pomelo.EntityFrameworkCore.MySql;
using MySql.Data.MySqlClient;

namespace JukeBox;

public class DbdContextClass : DbContext
{
    public DbSet<Genres> Genres { get; set; }
    public DbSet<Songs> Songs { get; set; }
    public DbSet<Users> Users { get; set; }
    // public DbSet<PlayListclass> PlayListclass { get; set; }
    // Define other DbSets for your entities
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // string connectionString = "server=localhost;port=3306;database=JukeBoxData;user=root;password=;";
        optionsBuilder.UseMySql("server=localhost;port=3306;database=JukeDataBase;user=root;password=;", new MySqlServerVersion(new Version(8, 0, 11)));
        //
        // // Test the connection
        // using (var connection = new MySqlConnection(connectionString))
        // {
        //     connection.Open();
        //     Console.WriteLine("Connected successfully!");
        // }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genres>(entity =>
        {
            // Set the table name (optional)
            entity.ToTable("Genres");

            // Configure the primary key
            entity.HasKey(g => g.Name); // Assuming 'Genre' property is the primary key

            // Configure other properties
            entity.Property(g => g.GenreDescription).HasMaxLength(100);

            // Add any additional configurations, such as relationships, indexes, etc.
        });

        modelBuilder.Entity<Songs>(entity =>
        {
            entity.ToTable("Songs");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name).HasMaxLength(100);
        });
        
    
        modelBuilder.Entity<Users>(entity => 
            { entity.HasKey(u => u.Id); });

        // modelBuilder.Entity<PlayListclass>()
        //     .HasNoKey(); // Specify that PlayListclass is a keyless entity type
        //

        // Add configurations for other entities, if any

        base.OnModelCreating(modelBuilder);
    }

}