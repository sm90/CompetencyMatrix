using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace CompetencyMatrix.ViewModels
{
    public class LogsReportViewModel
    {
        public IPagedList<LogEntryViewModel> LogItems { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }

        [HiddenInput]
        public int? TopId { get; set; }

        [Display(Name = "Log level filter: ")]
        public string LogLevel { get; set; }
    }
}