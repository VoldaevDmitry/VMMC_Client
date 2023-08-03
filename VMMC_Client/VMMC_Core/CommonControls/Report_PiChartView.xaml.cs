using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LiveCharts;
using LiveCharts.Wpf;
using System.Data.SqlClient;

namespace VMMC_Core.CommonControls
{
    /// <summary>
    /// Логика взаимодействия для Report_PiChartView.xaml
    /// </summary>
    public partial class Report_PiChartView : UserControl
    {

        
        public Report_PiChartView()
        {
            InitializeComponent();
            VMMC_Core.CommonControls.Report_PiChartViewModel reportPiChartViewModel = new VMMC_Core.CommonControls.Report_PiChartViewModel(null);

            reportPiChartViewModel.PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = reportPiChartViewModel;
        }
        

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            PieSeries selectedSeries = (PieSeries)chartpoint.SeriesView;
            if (selectedSeries.PushOut == 8)
            {
                VMMC_Core.CommonControls.Report_PiChartViewModel report_PiChartViewModel = (Report_PiChartViewModel)this.DataContext;
                if (report_PiChartViewModel.sessionInfo != null)
                {

                    List<PieSeries> listPieSeries = new List<PieSeries>();
                    if (report_PiChartViewModel.reportName == "РД-Тег")
                    {
                        selectedSeries.PushOut = 0;
                        //listPieSeries = report_PiChartViewModel.GetRelRDTagReport();
                        report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelRDTagCountDetailsReport("");
                        DocumetnsGrid.ItemsSource = report_PiChartViewModel.ReportItemsCollection;
                    }
                    else if (report_PiChartViewModel.reportName == "РД-3D")
                    {
                        selectedSeries.PushOut = 0;
                        //listPieSeries = report_PiChartViewModel.GetRelRDTagReport();
                        report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelRD3DCountDetailsReport("");
                        DocumetnsGrid.ItemsSource = report_PiChartViewModel.ReportItemsCollection;
                    }
                    else if (report_PiChartViewModel.reportName == "Документы по типам")
                    {
                        report_PiChartViewModel.reportName = "Документы по классам";
                        listPieSeries = report_PiChartViewModel.GetRelDocClassCountReport(selectedSeries.Title);
                        report_PiChartViewModel.LegendSeriesCollection = listPieSeries;
                        LegendGrid.ItemsSource = report_PiChartViewModel.LegendSeriesCollection;
                    }
                    if (listPieSeries.Count > 0)
                    {
                        chart = (LiveCharts.Wpf.PieChart)Chart1;
                        chart.Series.Clear();

                        foreach (PieSeries series in listPieSeries)
                            chart.Series.Add(series);
                    }
                }
            }
            else
            {
                foreach (PieSeries series in chart.Series)
                    series.PushOut = 0;

                selectedSeries.PushOut = 8;

                VMMC_Core.CommonControls.Report_PiChartViewModel report_PiChartViewModel = (Report_PiChartViewModel)this.DataContext;
                if (report_PiChartViewModel.reportName == "РД-Тег") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelRDTagCountDetailsReport(selectedSeries.Title);
                else if (report_PiChartViewModel.reportName == "РД-3D") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelRD3DCountDetailsReport(selectedSeries.Title);
                else if (report_PiChartViewModel.reportName == "Документы по типам") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelDocTypeCountDetailsReport(selectedSeries.Title);
                else if (report_PiChartViewModel.reportName == "Документы по классам") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelDocClassCountDetailsReport(selectedSeries.Title);
                DocumetnsGrid.ItemsSource = report_PiChartViewModel.ReportItemsCollection;
            }
        }

        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            UpdateReport();
        }
        public void UpdateReport() 
        {
            VMMC_Core.CommonControls.Report_PiChartViewModel report_PiChartViewModel = (Report_PiChartViewModel)this.DataContext;
            if (report_PiChartViewModel.sessionInfo != null)
            {

                List<PieSeries> listPieSeries = new List<PieSeries>();
                if (report_PiChartViewModel.reportName == "РД-Тег") listPieSeries = report_PiChartViewModel.GetRelRDTagReport();
                else if (report_PiChartViewModel.reportName == "РД-3D") listPieSeries = report_PiChartViewModel.GetRelRD3DReport();
                else if (report_PiChartViewModel.reportName == "Документы по типам") listPieSeries = report_PiChartViewModel.GetRelDocTypeCountReport();

                var chart = (LiveCharts.Wpf.PieChart)Chart1;
                chart.Series.Clear();

                foreach (PieSeries series in listPieSeries)
                    chart.Series.Add(series);

                report_PiChartViewModel.LegendSeriesCollection = listPieSeries;
                LegendGrid.ItemsSource = report_PiChartViewModel.LegendSeriesCollection;
            }
        }
        private void DocumetnsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            VMMC_Core.ReportItem selectedItem = (VMMC_Core.ReportItem)DocumetnsGrid.SelectedItem;
            if (selectedItem != null)
            {
                VMMC_Core.Document selectedDocument = new VMMC_Core.Document(selectedItem.Item.sessionInfo);
                if (!selectedItem.Item.IsExistInDB) 
                {
                    //string st = selectedItem.StatusInfo;
                    selectedDocument = selectedDocument.GetDocument(selectedItem.Item.ObjectCode);
                    //selectedItem.StatusInfo = st;
                }
                if (selectedDocument.Revisions == null) selectedDocument.Revisions = new VMMC_Core.Revision(selectedDocument.sessionInfo).GetDbDocumentRevisionsList(selectedDocument.DocumentId);
                VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(selectedDocument, selectedDocument.Revisions.Where(x=> x.IsCurrent=true).FirstOrDefault(), null, !selectedDocument.IsExistInDB, selectedDocument.sessionInfo);
                DocumentViewControl.DataContext = DocumentViewControl_DataContext;
            }
        }
        private void LegendGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PieSeries selectedSeries = (PieSeries)LegendGrid.SelectedItem;
            if (selectedSeries != null)
            {
                var chart = (LiveCharts.Wpf.PieChart)Chart1;

                foreach (PieSeries series in chart.Series)
                    series.PushOut = 0;

                selectedSeries.PushOut = 8;

                VMMC_Core.CommonControls.Report_PiChartViewModel report_PiChartViewModel = (Report_PiChartViewModel)this.DataContext;
                if (report_PiChartViewModel.reportName == "РД-Тег") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelRDTagCountDetailsReport(selectedSeries.Title);
                else if (report_PiChartViewModel.reportName == "РД-3D") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelRD3DCountDetailsReport(selectedSeries.Title);
                else if (report_PiChartViewModel.reportName == "Документы по типам") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelDocTypeCountDetailsReport(selectedSeries.Title);
                else if (report_PiChartViewModel.reportName == "Документы по классам") report_PiChartViewModel.ReportItemsCollection = report_PiChartViewModel.GetRelDocClassCountDetailsReport(selectedSeries.Title);
                DocumetnsGrid.ItemsSource = report_PiChartViewModel.ReportItemsCollection;

            }
        }
    }
}
