using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using FinReportWebServiceByWsdl.ServerClass;

namespace FinReportWebServiceByWsdl
{
    [WebService(Namespace = "http://asmx.report.ru/")]
    [WebServiceBinding(Name = "FinReportServiceSoap", Namespace = "http://asmx.report.ru/")]
    public class FinReportService : WebService
    {
        [WebMethod]
        [SoapDocumentMethod(
            "http://asmx.report.ru/GetReportIdArray",
            RequestNamespace = "http://asmx.report.ru/",
            ResponseNamespace = "http://asmx.report.ru/",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public GetReportIdArrayResult GetReportIdArray(GetReportIdArrayArg arg)
        {
            return new GetReportIdArrayResult
            {
                ReportIdArray = new []{ 23, 666, 42 }
            };
        }

        [WebMethod]
        [SoapDocumentMethod(
            "http://asmx.report.ru/GetReport",
            RequestNamespace = "http://asmx.report.ru/",
            ResponseNamespace = "http://asmx.report.ru/",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public GetReportResult GetReport(GetReportArg arg)
        {
            return new GetReportResult
            {
                Report = new FinReport
                {
                    ReportID = arg.ReportID,
                    Date = new DateTime(2015, 03, 15),
                    Info = "ByWSDL"
                }
            };
        }
    }
}
