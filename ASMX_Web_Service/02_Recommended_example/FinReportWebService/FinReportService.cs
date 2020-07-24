using System;
using System.Web.Services;

namespace FinReportWebService
{
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [WebService(Description = "Фин. отчеты", Namespace = XmlNS)]
    public class FinReportService : WebService
    {
        public const string XmlNS = "http://asmx.report.ru/";

        [WebMethod(Description = "Получение списка ID отчетов по периоду")]
        public GetReportIdArrayResult GetReportIdArray(GetReportIdArrayArg arg)
        {
            return new GetReportIdArrayResult
            {
                ReportIdArray = new [] {357, 358, 360, 361}
            };
        }

        [WebMethod(Description = "Получение отчета по ID")]
        public GetReportResult GetReport(GetReportArg arg)
        {
            return new GetReportResult
            {
                Report = new FinReport
                {
                    ReportId = arg.ReportId,
                    Date = new DateTime(2015, 03, 15),
                    Info = GetReportInfo(arg.ReportId)
                }
            };
        }

        private string GetReportInfo(int reportId)
        {
            return "ReportId = " + reportId;
        }
    }

    // [Serializable]
    // [XmlType(Namespace = FinReportService.XmlNS)]
    public class FinReport
    {
        public int ReportId { get; set; }
        public DateTime Date { get; set; }
        public string Info { get; set; }
    }

    public class GetReportIdArrayArg
    {
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
    }

    public class GetReportIdArrayResult
    {
        public int[] ReportIdArray { get; set; }
    }

    public class GetReportArg
    {
        public int ReportId { get; set; }
    }

    public class GetReportResult
    {
        public FinReport Report { get; set; }
    }
}
