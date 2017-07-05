using System;

namespace CompetencyMatrix.Infrastructure
{

    public class TimeExpiredException: Exception
    {
        public TimeExpiredException(): base() {
        }

        /// <summary>
        /// Create the exception with description
        /// </summary>
        /// <param name="message">Exception description</param>
        public TimeExpiredException(String message) : base(message) {
        }

        /// <summary>
        /// Create the exception with description and inner cause
        /// </summary>
        /// <param name="message">Exception description</param>
        /// <param name="innerException">Exception inner cause</param>
        public TimeExpiredException(String message, Exception innerException) : base(message, innerException) {
        }
    }
}
