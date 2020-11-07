using System;
using BeanlancerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BeanlancerAPI2.Models
{
    public partial class BeanlancersContext : DbContext
    {
        public BeanlancersContext()
        {
        }

        public BeanlancersContext(DbContextOptions<BeanlancersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Applies> Applies { get; set; }
        public virtual DbSet<AttachmentFile> AttachmentFile { get; set; }
        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CategoriesOfDesigner> CategoriesOfDesigner { get; set; }
        public virtual DbSet<Designer> Designer { get; set; }
        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<RequestHistory> RequestHistory { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<SkillOfDesigner> SkillOfDesigner { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Wallet> Wallet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:swd007.database.windows.net,1433;Initial Catalog=Beanlancers;Persist Security Info=False;User ID=beanlanceradmin;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applies>(entity =>
            {
                entity.HasKey(c => new{c.IdRequest, c.IdDesigner});

                entity.Property(e => e.IdDesigner)
                    .HasColumnName("ID_Designer")
                    .HasMaxLength(30);

                entity.Property(e => e.IdRequest)
                    .HasColumnName("ID_Request")
                    .HasMaxLength(20);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.Time).HasColumnName("Time").HasColumnType("datetime");

                entity.HasOne(d => d.IdDesignerNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDesigner)
                    .HasConstraintName("FK_tbl_Applying_tbl_Designer");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdRequest)
                    .HasConstraintName("FK_tbl_Applying_tbl_Request");
            });

            modelBuilder.Entity<RequestSkill>(entity => {

               entity.HasKey(c => new{c.IdSkill, c.IdRequest});

               entity.Property(e => e.IdRequest)
               .HasColumnName("ID_Request")
               .HasMaxLength(20); 

               entity.Property(e => e.IdSkill).HasColumnName("ID_Skill");

               entity.HasOne(d => d.IdRequestNavigation)
               .WithMany()
               .HasForeignKey(d => d.IdRequest)
               .HasConstraintName("FK_RequestSkill_Request");

               entity.HasOne(d => d.IdSkillNavigation)
               .WithMany()
               .HasForeignKey(d => d.IdSkill)
               .HasConstraintName("FK_RequestSkill_Skill");
            });


            modelBuilder.Entity<AttachmentFile>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdRequest)
                    .HasColumnName("ID_Request")
                    .HasMaxLength(20);

                entity.Property(e => e.Path).HasMaxLength(250);

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdRequest)
                    .HasConstraintName("FK_tbl_AttachmentFile_tbl_Request");
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(e => e.IdBudget)
                    .HasName("PK_tbl_Budget");

                entity.Property(e => e.IdBudget).HasColumnName("ID_Budget");

                entity.Property(e => e.RangeBudget)
                    .HasColumnName("Range_Budget")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.IdCategories)
                    .HasName("PK_tbl_Categories");

                entity.Property(e => e.IdCategories).HasColumnName("ID_Categories");

                entity.Property(e => e.NameCategories)
                    .HasColumnName("Name_Categories")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CategoriesOfDesigner>(entity =>
            {
                entity.HasKey(c => new { c.IdDesigner, c.IdCategories});

                entity.Property(e => e.IdCategories).HasColumnName("ID_Categories");

                entity.Property(e => e.IdDesigner)
                    .IsRequired()
                    .HasColumnName("ID_Designer")
                    .HasMaxLength(30);

                entity.HasOne(d => d.IdCategoriesNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdCategories)
                    .HasConstraintName("FK_tbl_CategoriesOfDesigner_tbl_Categories");

                entity.HasOne(d => d.IdDesignerNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDesigner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_CategoriesOfDesigner_tbl_Designer");
            });

            modelBuilder.Entity<Designer>(entity =>
            {
                entity.HasKey(e => e.IdDesigner)
                    .HasName("PK_tbl_Designer");

                entity.Property(e => e.IdDesigner)
                    .HasColumnName("ID_Designer")
                    .HasMaxLength(30);

                entity.Property(e => e.Username);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Designer)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_tbl_Designer_tbl_User");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdDesigner)
                    .HasColumnName("ID_Designer")
                    .HasMaxLength(30);

                entity.Property(e => e.Username);

                entity.HasOne(d => d.IdDesignerNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDesigner)
                    .HasConstraintName("FK_tbl_Favorite_tbl_Designer");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_tbl_Favorite_tbl_User");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.IdPayment)  
                    .HasName("PK_tbl_Payment");

                entity.Property(e => e.IdPayment)
                    .HasColumnName("ID_Payment")
                    .HasMaxLength(50);

                entity.Property(e => e.IdRequest)
                    .HasColumnName("ID_Request")
                    .HasMaxLength(20);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.IdRequest)
                    .HasConstraintName("FK_tbl_Payment_tbl_Request");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(e => e.IdRequest)
                    .HasName("PK_tbl_Request");

                entity.Property(e => e.IdRequest)
                    .HasColumnName("ID_Request")
                    .HasMaxLength(20);

                entity.Property(e => e.BeanAmount).HasColumnName("Bean_amount");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.IdBudget).HasColumnName("ID_Budget");

                entity.Property(e => e.IdCategories).HasColumnName("ID_Categories");

                entity.Property(e => e.IdDesigner)
                    .HasColumnName("ID_Designer")
                    .HasMaxLength(50);

                entity.Property(e => e.NameRequest)
                    .HasColumnName("Name_Request")
                    .HasMaxLength(50);

                entity.Property(e => e.Requirement).HasMaxLength(500);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TimeExpired)
                    .HasColumnName("Time_Expired")
                    .HasColumnType("date");

                entity.Property(e => e.Username)
                    .IsRequired();
                    

                entity.HasOne(d => d.IdBudgetNavigation)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.IdBudget)
                    .HasConstraintName("FK_tbl_Request_tbl_Budget");

                entity.HasOne(d => d.IdCategoriesNavigation)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.IdCategories)
                    .HasConstraintName("FK_tbl_Request_tbl_Categories");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Request_tbl_User");
            });

            modelBuilder.Entity<RequestHistory>(entity =>
            {
                entity.HasKey(c => new{ c.IdRequest, c.Action});

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.IdRequest)
                    .HasColumnName("ID_Request")
                    .HasMaxLength(20);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.IdRequestNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdRequest)
                    .HasConstraintName("FK_tbl_RequestHistory_tbl_Request");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK_tbl_Role");

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.IdSkill)
                    .HasName("PK_tbl_Skill");

                entity.Property(e => e.IdSkill).HasColumnName("ID_Skill");

                entity.Property(e => e.NameSkill)
                    .HasColumnName("Name_Skill")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SkillOfDesigner>(entity =>
            {
                entity.HasKey(c => new { c.IdDesigner,c.IdSkill});

                entity.Property(e => e.IdDesigner)
                    .IsRequired()
                    .HasColumnName("ID_Designer")
                    .HasMaxLength(30);

                entity.Property(e => e.IdSkill).HasColumnName("ID_Skill");

                entity.HasOne(d => d.IdDesignerNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDesigner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_SkillOfDesigner_tbl_Designer");

                entity.HasOne(d => d.IdSkillNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdSkill)
                    .HasConstraintName("FK_tbl_SkillOfDesigner_tbl_Skill");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK_tbl_User");

                entity.Property(e => e.Username);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.IdRole).HasColumnName("ID_role");

                entity.Property(e => e.Phone).HasMaxLength(11);
                entity.Property(e => e.Status);

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_User_tbl_Role");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.IdWallet)
                    .HasName("PK_tbl_Wallet");

                entity.Property(e => e.IdWallet).HasColumnName("ID_Wallet");

                entity.Property(e => e.BeanAmout).HasColumnName("Bean_amout");

                entity.Property(e => e.Username)
                    .IsRequired();
                    

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Wallet)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Wallet_tbl_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
