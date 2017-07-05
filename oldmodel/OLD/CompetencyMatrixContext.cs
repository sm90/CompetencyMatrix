using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CompetencyMatrix.Models
{
    public partial class CompetencyMatrixContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=prognimak2\sqlserver2016;Database=CompetencyMatrix;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<AvailableSkillLevels> AvailableSkillLevels { get; set; }
        public virtual DbSet<ChangeLog> ChangeLog { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeMatrix> EmployeeMatrix { get; set; }
        public virtual DbSet<EmployeeMatrixApproval> EmployeeMatrixApproval { get; set; }
        public virtual DbSet<EmployeeMatrixSkills> EmployeeMatrixSkills { get; set; }
        public virtual DbSet<PositionMatrix> PositionMatrix { get; set; }
        public virtual DbSet<PositionMatrixInheritance> PositionMatrixInheritance { get; set; }
        public virtual DbSet<PositionMatrixSkillGroups> PositionMatrixSkillGroups { get; set; }
        public virtual DbSet<PositionMatrixSkills> PositionMatrixSkills { get; set; }
        public virtual DbSet<SkillCategory> SkillCategory { get; set; }
        public virtual DbSet<SkillCriteria> SkillCriteria { get; set; }
        public virtual DbSet<SkillGroupTypes> SkillGroupTypes { get; set; }
        public virtual DbSet<SkillLevelCriteria> SkillLevelCriteria { get; set; }
        public virtual DbSet<SkillLevels> SkillLevels { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}