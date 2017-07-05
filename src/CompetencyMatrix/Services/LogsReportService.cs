using System.Linq;
using System.Collections.Generic;
using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLog;
using PagedList.Core;
using CompetencyMatrix.Models.Extension;

namespace CompetencyMatrix.Services
{
    public class LogsReportService : ILogsReportService
    {
        private readonly Dictionary<string, List<string>> _logLevelsSeverity = new Dictionary<string, List<string>>
        {
            {
                LogEntryViewModel.INFO, new List<string>
                {
                    LogLevel.Debug.Name,
                    LogLevel.Info.Name,
                    LogLevel.Trace.Name
                }
            },
            {
                LogEntryViewModel.WARNING, new List<string>
                {
                    LogLevel.Warn.Name
                }
            },
            {
                LogEntryViewModel.DANGER, new List<string>
                {
                    LogLevel.Error.Name,
                    LogLevel.Fatal.Name
                }
            }
        };

        public IPagedList<Log> GetLogs(int topId, int pageNumber, int itemsPerPage, string logLevel,
            ICompetencyMatrixContext dbContext)
        {
            var query = from logItem in dbContext.Log
                select logItem;

            if (logLevel != null && logLevel != LogEntryViewModel.ALL_ITEMS && _logLevelsSeverity.ContainsKey(logLevel))
            {
                query = query.Where(log => _logLevelsSeverity[logLevel].Contains(log.Level));
            }
            if (topId != -1)
            {
                query = query.Where(log => log.Id <= topId);
            }

            query = query.OrderByDescending(log => log.Id);

            return query.ToPagedListByQuery(pageNumber, itemsPerPage);
        }

        public List<SelectListItem> GetLogLevelOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Selected = true, Text = "All", Value = LogEntryViewModel.ALL_ITEMS},
                new SelectListItem {Selected = true, Text = "Errors", Value = LogEntryViewModel.DANGER},
                new SelectListItem {Selected = true, Text = "Warnings", Value = LogEntryViewModel.WARNING},
                new SelectListItem {Selected = true, Text = "Information", Value = LogEntryViewModel.INFO}
            };
        }
    }
}