using CompetencyMatrix.Models;
using NLog;

namespace CompetencyMatrix.ViewModels
{
    public class LogEntryViewModel
    {
        public const string DANGER = "danger";
        public const string INFO = "info";
        public const string WARNING = "warning";
        public const string ALL_ITEMS = "all";

        public Log Log { get; set; }

        public string Level { get; set; }

        public static LogEntryViewModel FromDbModel(Log log)
        {
            var result = new LogEntryViewModel
            {
                Log = log
            };

            if (log.Level == LogLevel.Error.Name || log.Level == LogLevel.Fatal.Name)
            {
                result.Level = DANGER;
            }
            else if (log.Level == LogLevel.Warn.Name)
            {
                result.Level = WARNING;
            }
            else if (log.Level == LogLevel.Info.Name)
            {
                result.Level = INFO;
            }

            return result;
        }
    }
}