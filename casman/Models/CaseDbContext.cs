using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
    using System.Collections.Concurrent;

    namespace casman.Models
    {
        public class CaseDbContext : DbContext
        {
            public CaseDbContext(DbContextOptions<CaseDbContext> options) : base(options) { }
        public DbSet<IndemnifierDto> Indemnifiers { get; set; }
        public DbSet<CaseDetailDto> CaseDetails { get; set; }
        public DbSet<PractitionerDto> Practitioners { get; set; }
        public DbSet<TSpclty> TSpclty { get; set; }  // Your actual EF entity for the Speciality table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TSpclty>().ToTable("t_spclty");
            modelBuilder.Entity<IndemnifierDto>().HasNoKey();
            modelBuilder.Entity<CaseDetailDto>().HasNoKey();
            modelBuilder.Entity<PractitionerDto>().HasNoKey();

        }
    }
}
