using System;
using System.Web.Services;

namespace FinReportWebService
{
    public class FinReportService
    {
        [WebMethod]
        public int[] GetReportIdArray(DateTime dateBegin, DateTime dateEnd)
        {
            return new[] { 357, 358, 360, 361 };
        }

        [WebMethod]
        public FinReport GetReport(int reportId)
        {
            return new FinReport
            {
                ReportId = reportId,
                Date = new DateTime(2015, 03, 15),
                Info = "Some Info"
            };
        }
    }
}
