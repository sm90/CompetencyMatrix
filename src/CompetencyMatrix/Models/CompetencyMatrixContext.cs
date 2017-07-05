using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;

namespace CompetencyMatrix.Models
{
    public partial class CompetencyMatrixContext : DbContext, ICompetencyMatrixContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);

                entity.HasMany ( r => r.PermissionOnRole )
                    .WithOne (por => por.Role )
                    .HasForeignKey(r => r.RoleId);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserRoles_UserId");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);

            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<ChangeLog>(entity =>
            {
                entity.Property(e => e.ByWhom)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.When).HasColumnType("datetime");

                entity.HasOne(d => d.ByWhomNavigation)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.ByWhom)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ChangeLog_AspNetUsers");

                entity.HasOne(d => d.Matrix)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.MatrixId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ChangeLog_EmployeeMatrix");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.ChangeLog)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ChangeLog_Skill");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Cell).HasColumnType("varchar(15)");

                entity.Property(e => e.Email)
                    .HasColumnName("EMail")
                    .HasColumnType("varchar(320)");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Office).HasMaxLength(50);

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.ProfileStatus).IsRequired();

                entity.Property(e => e.Skype).HasMaxLength(64);

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.ManagerNavigation)
                    .WithMany(p => p.InverseManagerNavigation)
                    .HasForeignKey(d => d.Manager)
                    .HasConstraintName("FK_Employee_Employee");

                entity.HasOne(d => d.Matrix)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.MatrixId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Employee_EmployeeMatrix");

                entity.HasOne ( d => d.MatrixApproval )
                    .WithOne ( p => p.Employee )
                    .HasForeignKey<EmployeeMatrixApproval> ( x => x.EmployeeId )
                    .HasConstraintName ( "FK_Employee_EmployeeMatrixApproval" );

            } );

            modelBuilder.Entity<EmployeeMatrix>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("ntext");
            });

            modelBuilder.Entity<EmployeeMatrixApproval>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ByWhom)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.When).HasColumnType("datetime");

                entity.HasOne(d => d.ByWhomNavigation)
                    .WithMany(p => p.EmployeeMatrixApproval)
                    .HasForeignKey(d => d.ByWhom)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmployeeMatrixApproval_AspNetUsers");
            });

            modelBuilder.Entity<EmployeeMatrixSkill>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Matrix)
                    .WithMany(p => p.EmployeeMatrixSkill)
                    .HasForeignKey(d => d.MatrixId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmployeeMatrixSkills_EmployeeMatrix");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.EmployeeMatrixSkill)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmployeeMatrixSkills_Skills");

                entity.HasOne(d => d.SkillLevel)
                    .WithMany(p => p.EmployeeMatrixSkill)
                    .HasForeignKey(d => d.SkillLevelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmployeeMatrixSkills_AvailableSkillLevels");
            });

            modelBuilder.Entity<EmployeePastProject>(entity =>
            {
                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Project)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Team).HasMaxLength(50);

                entity.Property(e => e.WorkPeriodEnd).HasColumnType("datetime");

                entity.Property(e => e.WorkPeriodStart).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeePastProject)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmployeePastProject_Employee");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.Controller).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<PermissionOnRole>(entity =>
            {
                entity.Property(e => e.Action).HasMaxLength(32);

                entity.Property(e => e.Controller).HasMaxLength(32);

                entity.Property(e => e.IsActive).HasDefaultValueSql("0");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.PermissionOnRole)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PermissionOnRole_Permission");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PermissionOnRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PermissionOnRole_AspNetRoles");
            });

            modelBuilder.Entity<PositionMatrix>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.Property(e=>e.OwnerId).IsRequired().HasMaxLength(450);

                entity.HasOne(d => d.Owner)
                    .WithMany()
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrix_AspNetUsers");
            });

            modelBuilder.Entity<PositionMatrixInheritance>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Matrix)
                    .WithMany(p => p.PositionMatrixInheritanceMatrix)
                    .HasForeignKey(d => d.MatrixId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrixInheritance_PositionMatrix");

                entity.HasOne(d => d.ParentMatrix)
                    .WithMany(p => p.PositionMatrixInheritanceParentMatrix)
                    .HasForeignKey(d => d.ParentMatrixId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrixInheritance_PositionMatrix1");
            });

            modelBuilder.Entity<PositionMatrixSkill>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Matrix)
                    .WithMany(p => p.PositionMatrixSkill)
                    .HasForeignKey(d => d.MatrixId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrixSkills_PositionMatrix");

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.PositionMatrixSkill)
                    .HasForeignKey(d => d.SkillGroupId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrixSkills_PositionMatrixSkillGroups");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.PositionMatrixSkill)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrixSkills_Skills");

                entity.HasOne(d => d.SkillLevel)
                    .WithMany(p => p.PositionMatrixSkill)
                    .HasForeignKey(d => d.SkillLevelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrixSkills_AvailableSkillLevels");
            });

            modelBuilder.Entity<PositionMatrixSkillGroup>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.GroupType)
                    .WithMany(p => p.PositionMatrixSkillGroup)
                    .HasForeignKey(d => d.GroupTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PositionMatrixSkillGroups_SkillGroupTypes");

                entity.HasMany(d => d.Children)
                    .WithOne(p => p.ParentGroup)
                    .HasForeignKey(f => f.ParentGroupId);
            });

            modelBuilder.Entity<RoleInProject>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Skill)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Skill_SkillCategory");

                entity.HasOne(d => d.EvaluationModel)
                    .WithMany(p => p.Skill)
                    .HasForeignKey(d => d.EvaluationModelId)
                    .HasConstraintName("FK_Skill_SkillEvaluationModel");
            });

            modelBuilder.Entity<SkillCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_SkillCategory_SkillCategory");
            });

            modelBuilder.Entity<SkillCriteria>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.SkillCriteria)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SkillCriteria_Skills1");
            });

            modelBuilder.Entity<SkillEvaluationModel>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<SkillEvaluationModelLevel>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("_id");

                entity.HasOne(d => d.SkillEvaluationModel)
                    .WithMany(p => p.SkillEvaluationModelLevel)
                    .HasForeignKey(d => d.SkillEvaluationModelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SkillEvaluationModelLevel_SkillEvaluationModel");

                entity.HasOne(d => d.SkillLevelModel)
                    .WithMany(p => p.SkillEvaluationModelLevel)
                    .HasForeignKey(d => d.SkillLevelModelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SkillEvaluationModelLevel_SkillLevelModel");
            });

            modelBuilder.Entity<SkillGroupType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SkillLevelCriteria>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.SkillCriteria)
                    .WithMany(p => p.SkillLevelCriteria)
                    .HasForeignKey(d => d.SkillCriteriaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SkillLevelCriteria_SkillCriteria");

                entity.HasOne(d => d.SkillLevelModel)
                    .WithMany(p => p.SkillLevelCriteria)
                    .HasForeignKey(d => d.SkillLevelModelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SkillLevelCriteria_AvailableSkillLevels");
            });

            modelBuilder.Entity<SkillLevelModel>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Log> ( );
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ChangeLog> ChangeLog { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeMatrix> EmployeeMatrix { get; set; }
        public virtual DbSet<EmployeeMatrixApproval> EmployeeMatrixApproval { get; set; }
        public virtual DbSet<EmployeeMatrixSkill> EmployeeMatrixSkill { get; set; }
        public virtual DbSet<EmployeePastProject> EmployeePastProject { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionOnRole> PermissionOnRole { get; set; }
        public virtual DbSet<PositionMatrix> PositionMatrix { get; set; }
        public virtual DbSet<PositionMatrixInheritance> PositionMatrixInheritance { get; set; }
        public virtual DbSet<PositionMatrixSkill> PositionMatrixSkill { get; set; }
        public virtual DbSet<PositionMatrixSkillGroup> PositionMatrixSkillGroup { get; set; }
        public virtual DbSet<RoleInProject> RoleInProject { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<SkillCategory> SkillCategory { get; set; }
        public virtual DbSet<SkillCriteria> SkillCriteria { get; set; }
        public virtual DbSet<SkillEvaluationModel> SkillEvaluationModel { get; set; }
        public virtual DbSet<SkillEvaluationModelLevel> SkillEvaluationModelLevel { get; set; }
        public virtual DbSet<SkillGroupType> SkillGroupType { get; set; }
        public virtual DbSet<SkillLevelCriteria> SkillLevelCriteria { get; set; }
        public virtual DbSet<SkillLevelModel> SkillLevelModel { get; set; }
        public virtual DbSet<TrainingSertification> TrainingSertification { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<MailTemplate> MailTemplate { get; set; }
    }
}