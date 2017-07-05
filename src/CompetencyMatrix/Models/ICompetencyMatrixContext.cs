using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CompetencyMatrix.Models
{
    public interface ICompetencyMatrixContext
    {
        int SaveChanges();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        DbSet<AspNetRoles> AspNetRoles { get; set; }
        DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        DbSet<AspNetUsers> AspNetUsers { get; set; }
        DbSet<ChangeLog> ChangeLog { get; set; }
        DbSet<Employee> Employee { get; set; }
        DbSet<EmployeeMatrix> EmployeeMatrix { get; set; }
        DbSet<EmployeeMatrixApproval> EmployeeMatrixApproval { get; set; }
        DbSet<EmployeeMatrixSkill> EmployeeMatrixSkill { get; set; }
        DbSet<EmployeePastProject> EmployeePastProject { get; set; }
        DbSet<Permission> Permission { get; set; }
        DbSet<PermissionOnRole> PermissionOnRole { get; set; }
        DbSet<PositionMatrix> PositionMatrix { get; set; }
        DbSet<PositionMatrixInheritance> PositionMatrixInheritance { get; set; }
        DbSet<PositionMatrixSkill> PositionMatrixSkill { get; set; }
        DbSet<PositionMatrixSkillGroup> PositionMatrixSkillGroup { get; set; }
        DbSet<RoleInProject> RoleInProject { get; set; }
        DbSet<Skill> Skill { get; set; }
        DbSet<SkillCategory> SkillCategory { get; set; }
        DbSet<SkillCriteria> SkillCriteria { get; set; }
        DbSet<SkillEvaluationModel> SkillEvaluationModel { get; set; }
        DbSet<SkillEvaluationModelLevel> SkillEvaluationModelLevel { get; set; }
        DbSet<SkillGroupType> SkillGroupType { get; set; }
        DbSet<SkillLevelCriteria> SkillLevelCriteria { get; set; }
        DbSet<SkillLevelModel> SkillLevelModel { get; set; }
        DbSet<TrainingSertification> TrainingSertification { get; set; }
        DbSet<Log> Log { get; set; }
        DbSet<MailTemplate> MailTemplate { get; set; }

        DatabaseFacade Database { get; }
    }
}