using System;
using System.Runtime.Serialization;

namespace CrystalDecisions.CrystalReports.Engine
{
    [Serializable]
    internal class ReportLoadException : Exception
    {
        public ReportLoadException()
        {
        }

        public ReportLoadException(string message) : base(message)
        {
        }

        public ReportLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReportLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}