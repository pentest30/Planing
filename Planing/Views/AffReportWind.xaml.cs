using System;
using Microsoft.Reporting.WinForms;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for AffReportWind.xaml
    /// </summary>
    public partial class AffReportWind
    {
        public AffReportWind(System.Data.DataTable table)
        {
            InitializeComponent();
            ReportViewer.LocalReport.ReportPath = ReportViewer.LocalReport.ReportPath = Environment.CurrentDirectory + @"\Reporting\ReportAff.rdlc";
            ReportViewer.LocalReport.DataSources.Clear();
            ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1",
                table));
            ReportViewer.RefreshReport();
            ReportViewer.Show();
        }
    }
}
