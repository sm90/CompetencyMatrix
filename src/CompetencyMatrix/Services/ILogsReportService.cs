using System.Collections.Generic;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;

namespace CompetencyMatrix.Services
{
    public interface ILogsReportService
    {
        IPagedList<Log> GetLogs(int topId, int pageNumber, int itemsPerPage, string logLevel,
            ICompetencyMatrixContext dbContext);

        List<SelectListItem> GetLogLevelOptions();
    }
}