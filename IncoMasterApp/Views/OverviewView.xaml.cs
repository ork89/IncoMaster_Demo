using IncoMasterApp.Interfaces;
using IncoMasterApp.ViewModels;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace IncoMasterApp.Views
{
    /// <summary>
    /// Interaction logic for OverviewView.xaml
    /// </summary>

    public partial class OverviewView : UserControl, IView
    {
        public PieChart Chart { get { return this.OverviewPieChart; } }

        public OverviewView()
        {
            InitializeComponent();
            this.DataContext = new OverviewViewModel(this);
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
