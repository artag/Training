using FinReportWebServiceClient.ProxyClass;
using System;
using System.Windows;

namespace FinReportWebServiceClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            GetReportIdArray();
        }

        private void Button2_OnClick(object sender, RoutedEventArgs e)
        {
            GetReport();
        }

        private void GetReportIdArray()
        {
            using (var service = GetFinReportService())
            {
                var arg = new GetReportIdArrayArg();
                arg.DateBegin = new DateTime(2015, 03, 01);
                arg.DateEnd = new DateTime(2015, 03, 02);

                var result = service.GetReportIdArray(arg);
                MessageBox.Show($"result.ReportIdArray.Length = {result.ReportIdArray.Length}");
            }
        }

        private void GetReport()
        {
            using (var service = GetFinReportService())
            {
                var arg = new GetReportArg();
                arg.ReportId = 45;

                var result = service.GetReport(arg);
                MessageBox.Show($"result.Report.Info = \"{result.Report.Info}\"");
            }
        }

        private FinReportService GetFinReportService()
        {
            const int TimeoutMilliSeconds = 100 * 1000;

            var service = new FinReportService();
            service.Url = "https://localhost:44336/FinReport.asmx";
            service.Timeout = TimeoutMilliSeconds;
            return service;
        }
    }
}
