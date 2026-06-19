using Microsoft.EntityFrameworkCore;

namespace MotherInstitute.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AcademicSession> AcademicSessions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Fees> Fees { get; set; }
        public DbSet<Subjects> SubjectsList { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<BedDetails> BedDetailsList { get; set; }
        public DbSet<StudentRegd> StudentRegds { get; set; }
        public DbSet<Visitors> StudentVisitors { get; set; }
        public DbSet<StudentSubjects> StudentSubjects { get; set; }
        public DbSet<StudentFees> StudentFees { get; set; }
        public DbSet<StudentInstallment> StudentInstallments { get; set; }

        public DbSet<StudentExpense> StudentExpenses { get; set; }
        public DbSet<StudentPayment> StudentPayments { get; set; }

        public DbSet<EizocMember> EizocMembers { get; set; }

        public DbSet<SchoolMaster> SchoolMasters { get; set; }

        public DbSet<MarketingStudent> MarketingStudents { get; set; }

        public DbSet<MarketingAgent> MarketingAgents { get; set; }
        public DbSet<MarketingVisit> MarketingVisit { get; set; }
        public DbSet<ErrorTable> ErrorTable { get; set; }

        // NEW


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");
                entity.HasKey(e => e.LOGINID);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AcademicSession>(entity =>
            {
                entity.ToTable("ACADEMICSESSION");
                entity.HasKey(e => e.NAME);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("COURSE");
                entity.HasKey(e => e.NAME);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Fees>(entity =>
            {
                entity.ToTable("FEES");
                entity.HasKey(e => e.NAME);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.ToTable("SUBJECTS");
                entity.HasKey(e => e.NAME);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("ORGANIZATION");
                entity.HasKey(e => e.SLNO);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<BedDetails>(entity =>
            {
                entity.ToTable("BEDDETAILS");
                entity.HasKey(e => e.BEDNO);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentRegd>(entity =>
            {
                entity.ToTable("STUDENTREGD");
                entity.HasKey(e => e.STUDENTID);
            });

            modelBuilder.Entity<Visitors>(entity =>
            {
                entity.ToTable("STUDENTVISITORS");
                entity.HasKey(e => e.SLNO);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentSubjects>(entity =>
            {
                entity.ToTable("STUDENTSUB");
                entity.HasKey(e => e.SLNO);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentFees>(entity =>
            {
                entity.ToTable("STUDENTFEE");
                entity.HasKey(e => e.SLNO);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentInstallment>(entity =>
            {
                entity.ToTable("STUDENTINSTALLMENT");
                entity.HasKey(e => e.SLNO);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<EizocMember>(entity =>
            {
                entity.ToTable("EIZOCMEMBER");
                entity.HasKey(e => e.SLNO);
                entity.Property(e => e.SLNO).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StudentExpense>(entity =>
            {
                entity.ToTable("STUDENTEXPENSES");

                entity.HasKey(e => e.SLNO);

                entity.Property(e => e.SLNO)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.STUDENTID)
                      .IsRequired();

                entity.Property(e => e.DATE)
                      .IsRequired();

                entity.Property(e => e.CATEGORY)
                      .IsRequired();

                entity.Property(e => e.PARTICULARS)
                      .IsRequired();

                entity.Property(e => e.AMOUNT)
                      .HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Student)
                      .WithMany()
                      .HasForeignKey(e => e.STUDENTID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<StudentPayment>(entity =>
            {
                entity.ToTable("STUDENTPAYMENT");

                entity.HasKey(e => e.SLNO);

                entity.Property(e => e.SLNO)
                      .ValueGeneratedOnAdd();
            });

            // FIX AGENT REPORT ERROR
            
        }
    }
}