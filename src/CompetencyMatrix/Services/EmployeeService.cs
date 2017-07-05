using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompetencyMatrix.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly ICompetencyMatrixContext _dbContext;
        private readonly IServerVariables _serverVariables;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateService _templateService;
        private readonly ILoggerFactory _loggerFactory;

        public EmployeeService(ICompetencyMatrixContext dbContext, IServerVariables serverVariables, IEmailSender emailSender, ITemplateService templateservice, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _serverVariables = serverVariables;
            _emailSender = emailSender;
            _templateService = templateservice;
            _loggerFactory = loggerFactory;
        }

        public List<EmployeeMatrixSkill> GetTransactionSkills(int matrixId)
        {
            var levelModels = _dbContext.SkillLevelModel.ToList();

            var changeLogs = _dbContext.ChangeLog
                                    .Include(x => x.Skill)
                                    .Where(y => y.MatrixId == matrixId
                                        && (y.Status == EmplyeeProfileStatus.Open || y.Status == EmplyeeProfileStatus.Submitted))
                                    .ToList();

            var employeeSkills = _dbContext.EmployeeMatrixSkill
                                    .Where(a => (a.MatrixId == matrixId))
                                        .Include(ems => ems.SkillLevel)
                                        .Include(a => a.Skill).ThenInclude(a => a.ChangeLog)
                                    .ToList();


            foreach (var log in changeLogs)
            {
                if (log.Status == EmplyeeProfileStatus.Open || log.Status == EmplyeeProfileStatus.Submitted)
                {
                    if (log.Action == ChangeLogAction.Add)
                    {
                        employeeSkills.Add(EmployeeMatrixSkill.FromChangeLogDbModel(log, levelModels));
                    }
                    else if (log.Action == ChangeLogAction.Update)
                    {
                        for (int i = 0; i < employeeSkills.Count; i++)
                        {
                            if (employeeSkills[i].SkillId == log.SkillId)
                            {
                                employeeSkills[i].SkillLevelId = log.SkillLevelId;
                                continue;
                            }
                        }
                    }
                    else if (log.Action == ChangeLogAction.Delete)
                    {
                        for (int i = employeeSkills.Count - 1; i >= 0; i--)
                        {
                            if (employeeSkills[i].SkillId == log.SkillId)
                            {
                                employeeSkills.RemoveAt(i);
                                continue;
                            }
                        }
                    }
                }
            }

            return employeeSkills;

        }

        public EmployeeSkillStorageModel GetEmployeeSkillStorageModel(int employeeId)
        {
            var employee = GetEmployee(employeeId);
            var levelModel = _dbContext.SkillLevelModel.ToList();

            return new EmployeeSkillStorageModel()
            {
                EmployeeId = employeeId,
                Skills = EmployeeSkillModel.FromDbModelWithLogData(GetTransactionSkills(employee.MatrixId), levelModel, employee.MatrixId)
            };

        }

        public void SaveEmployee(int employeeId, EmplyeeProfileStatus status)
        {
            var employee = GetEmployee(employeeId);
            var levelModel = _dbContext.SkillLevelModel.ToList();

            EmployeeSkillStorageModel employeeMatrixSkills = null;

            if (_serverVariables.CurrentUserSkills != null)
            {
                employeeMatrixSkills = _serverVariables.CurrentUserSkills;
            }
            else
            {
                employeeMatrixSkills = new EmployeeSkillStorageModel()
                {
                    EmployeeId = employeeId,
                    Skills = EmployeeSkillModel.FromDbModelWithLogData(GetTransactionSkills(employee.MatrixId), levelModel, employee.MatrixId)
                };
            }

            var employeeSkills = employeeMatrixSkills.Skills.ToList();

            try
            {
                _dbContext.Database.BeginTransaction();

                employeeMatrixSkills.Skills
                    .Where(x => x.State != EntityState.Unchanged)
                    .ToList()
                    .ForEach(a =>
                    {
                        var log = _dbContext.ChangeLog.Where(y => y.MatrixId == employee.MatrixId && a.SkillId == y.SkillId && y.Status == EmplyeeProfileStatus.Open).FirstOrDefault();

                        if (log != null)
                        {
                            _dbContext.Entry(log).State = EntityState.Modified;
                        }
                        else
                        {
                            log = new ChangeLog();
                            log.MatrixId = employee.MatrixId;
                            log.SkillId = a.SkillId;
                            _dbContext.Entry(log).State = EntityState.Added;
                        }

                        log.ByWhom = _serverVariables.CurrentAspUserId;
                        log.Status = status;
                        log.SkillLevelId = a.LevelId;
                        log.When = a.LastUsed ?? DateTime.Now;

                        //If EmployeeSkillId <= 0 it means that was modified newly added skill
                        log.Action = a.State == EntityState.Added || a.EmployeeSkillId <= 0 ? ChangeLogAction.Add : ChangeLogAction.Update;

                        if (log.Action != ChangeLogAction.Add)
                        {
                            var logs = _dbContext.ChangeLog.Where(y => y.MatrixId == employee.MatrixId && a.SkillId == y.SkillId);
                            if (logs != null)
                            {
                                log.OldSkillLevelId = logs.ToList().OrderByDescending(m => m.When).First().SkillLevelId;
                            }
                        }
                    });

                employee.ProfileStatus = status;

                _dbContext.Entry(employee).State = EntityState.Modified;

                //Remove absent items
                var ids = employeeSkills.Where(a => a.EmployeeSkillId != 0).Select(a => a.SkillId).ToList();
                var forRemove = _dbContext.EmployeeMatrixSkill.Where(x => x.MatrixId == employee.MatrixId && !ids.Contains(x.SkillId)).ToList();
                forRemove.ToList().ForEach(a =>
                            {
                                var changeLog = _dbContext.ChangeLog.Where(y => y.MatrixId == employee.MatrixId && a.SkillId == y.SkillId && y.Status == EmplyeeProfileStatus.Open).FirstOrDefault();

                                if (changeLog == null)
                                {
                                    changeLog = new ChangeLog();
                                    _dbContext.Entry(changeLog).State = EntityState.Added;
                                }
                                else
                                {
                                    _dbContext.Entry(changeLog).State = EntityState.Modified;
                                }

                                changeLog.MatrixId = a.MatrixId;
                                changeLog.SkillId = a.SkillId;
                                changeLog.SkillLevelId = a.SkillLevelId;
                                changeLog.When = DateTime.Now;
                                changeLog.ByWhom = _serverVariables.CurrentAspUserId;
                                changeLog.Status = status;
                                changeLog.Action = ChangeLogAction.Delete;
                            });

                var changeLogSkills = _dbContext.ChangeLog.Where(x => x.MatrixId == employee.MatrixId
                                                                    && !ids.Contains(x.SkillId)
                                                                    && x.Status == EmplyeeProfileStatus.Open
                                                                    && x.Action == ChangeLogAction.Add);

                foreach (var skill in changeLogSkills)
                {
                    _dbContext.Entry(skill).State = EntityState.Deleted;
                }

                _dbContext.SaveChanges();


                if (employee.Manager.HasValue && status == EmplyeeProfileStatus.Submitted)
                {
                    var manager = GetEmployee(employee.Manager.Value);

                    var template = _templateService.GetInitializedTemplate(MailTemplateType.SubmitEmployeeProfile, employee, manager);
                    _emailSender.SendEmailAsync(manager.Email, template.Subject, template.Body);
                }
                _dbContext.Database.CommitTransaction();
            }
            catch (Exception)
            {

                _dbContext.Database.RollbackTransaction();
            }
        }

        private Employee GetEmployee(int userId)
        {
            return _dbContext.Employee
                .Include(x => x.MatrixApproval)
                .Single(a => a.Id == userId);
        }

        private void EmployeeWorkflow(int id, EmplyeeProfileStatus status, int approverId)
        {
            try
            {
                var changeLogStatus = status == EmplyeeProfileStatus.Approved ? EmplyeeProfileStatus.Approved //Approve
                                                                                : EmplyeeProfileStatus.Open;        //Reject

                _dbContext.Database.BeginTransaction();

                var employee = GetEmployee(id);

                employee.ProfileStatus = status;

                if (status == EmplyeeProfileStatus.Approved)
                {
                    if (employee.MatrixApproval == null)
                    {
                        employee.MatrixApproval = new EmployeeMatrixApproval();
                        _dbContext.Entry(employee.MatrixApproval).State = EntityState.Added;
                    }

                    employee.MatrixApproval.EmployeeId = employee.Id;
                    employee.MatrixApproval.When = DateTime.Now;

                    var manager = GetEmployee(approverId);

                    employee.MatrixApproval.ByWhom = manager.Name;
                }

                _dbContext.Entry(employee).State = EntityState.Modified;

                var changeLogItems = _dbContext.ChangeLog.Where(c => c.MatrixId == employee.MatrixId && c.Status == EmplyeeProfileStatus.Submitted);

                changeLogItems.Select(x => x).ToList().ForEach(changeLog =>
                    {
                        changeLog.Status = changeLogStatus;
                    }
                );
                if (status == EmplyeeProfileStatus.Approved)
                {
                    UpdateEmployeeSkills(changeLogItems.ToList(), ChangeLogAction.Add, employee.MatrixId);
                    UpdateEmployeeSkills(changeLogItems.ToList(), ChangeLogAction.Update, employee.MatrixId);
                    UpdateEmployeeSkills(changeLogItems.ToList(), ChangeLogAction.Delete, employee.MatrixId);
                }

                _dbContext.ChangeLog.UpdateRange(changeLogItems);

                _dbContext.SaveChanges();

                if (employee.Manager.HasValue)
                {
                    var manager = GetEmployee(employee.Manager.Value);

                    var templateType = status == EmplyeeProfileStatus.Approved ? MailTemplateType.ApproveEmployeeProfile : MailTemplateType.RejectEmployeeProfile;

                    var template = _templateService.GetInitializedTemplate(templateType, employee, manager);
                    _emailSender.SendEmailAsync(manager.Email, template.Subject, template.Body);
                }

                _dbContext.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _dbContext.Database.RollbackTransaction();
                throw;
            }
            finally
            {

            }
        }

        private void UpdateEmployeeSkills(List<ChangeLog> changeLogItems, ChangeLogAction action, int matrixId)
        {
            var changeLogSkills = changeLogItems.Where(x => x.Action == action);

            if (action == ChangeLogAction.Add)
            {
                var employeeSkills = changeLogSkills
                    .Select(item => new EmployeeMatrixSkill()
                    {
                        MatrixId = matrixId,
                        SkillId = item.SkillId,
                        Skill = item.Skill,
                        SkillLevelId = item.SkillLevelId                       
                    }).ToList();


                _dbContext.EmployeeMatrixSkill.AddRange(employeeSkills);
            }
            else if (action == ChangeLogAction.Update)
            {
                var skills = _dbContext.EmployeeMatrixSkill.Where(x => changeLogSkills.Any(s => s.SkillId == x.SkillId && x.MatrixId == matrixId)).ToList();

                skills.ForEach(skill =>
                {
                    var changeLog = changeLogSkills.First(x => x.SkillId == skill.SkillId);

                    skill.SkillLevelId = changeLog.SkillLevelId;

                });

                _dbContext.EmployeeMatrixSkill.UpdateRange(skills);

            }
            else if (action == ChangeLogAction.Delete)
            {
                var skills = _dbContext.EmployeeMatrixSkill.Where(x => changeLogSkills.Any(s => s.SkillId == x.SkillId && x.MatrixId == matrixId));

                _dbContext.EmployeeMatrixSkill.RemoveRange(skills);
            }


        }

        public void ApproveEmployee(int id, int approverId)
        {
            EmployeeWorkflow(id, EmplyeeProfileStatus.Approved, approverId);
        }

        public void RejectEmployee(int id, int approverId)
        {
            EmployeeWorkflow(id, EmplyeeProfileStatus.Open, approverId);
        }

        public void CancelEmployeeTransaction(int id)
        {
            try
            {
                _dbContext.Database.BeginTransaction();

                _serverVariables.CurrentUserSkills = null;

                var logs = _dbContext.ChangeLog.Where(x => x.Status == EmplyeeProfileStatus.Open);

                foreach (var log in logs)
                {
                    log.Status = EmplyeeProfileStatus.Cancelled;
                    _dbContext.Entry(log).State = EntityState.Modified;
                }

                var employee = GetEmployee(id);
                employee.ProfileStatus = EmplyeeProfileStatus.Approved;
                _dbContext.Entry(employee).State = EntityState.Modified;

                _dbContext.SaveChanges();

                _dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<Program>();
                logger.LogError(LoggingEvents.SendEmail, ex, ex.Message);

                _dbContext.Database.RollbackTransaction();
            }
        }
    }
}
