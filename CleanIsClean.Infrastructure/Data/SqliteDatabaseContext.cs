using CleanIsClean.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanIsClean.Infrastructure.Models;

public partial class SqliteDatabaseContext : DbContext
{
    public SqliteDatabaseContext()
    {
    }

    public SqliteDatabaseContext(DbContextOptions<SqliteDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }


//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         if (!optionsBuilder.IsConfigured)
//         {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//             optionsBuilder.UseSqlite("Data Source=C:\\Users\\Administrator\\Desktop\\Clean-Architecture-V2\\CleanIsClean.Infrastructure\\SqliteDatabase.sqlite3");
//         }
//     }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
