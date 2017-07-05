using Microsoft.Extensions.Logging;

namespace CompetencyMatrix.Infrastructure
{
    public class LoggingEvents
    {
        public static EventId GlobalUnhandledException = new EventId(0, "GLOBAL_UNHANDLED_EXCEPTION");

        public static EventId DisplayErrorPage = new EventId(1, "DISPLAY_ERROR_PAGE");

        public static EventId PositionMatrixCreation = new EventId(1001, "POSITION_MATRIX_CREATION");

        public static EventId SendEmail = new EventId(1010, "SEND_EMAIL");
    }
}