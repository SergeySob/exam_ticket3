using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms.DataVisualization.Charting;

namespace ticket3
{
    public partial class MainWindow : Window
    {
        private Series series;
        private Series seriessin;
        private Series seriescos;
        private Series seriesreciprocal;
        private Series seriesquadratic;
        private Series seriessqrt;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chart.ChartAreas.Add(new ChartArea("SeriaArena"));

            
            seriessin = new Series("Sin") { ChartType = SeriesChartType.Line, LegendText = "y=sin(x)" };
            seriesreciprocal = new Series("Reciprocal") { ChartType = SeriesChartType.Line, LegendText = "y=1/x" };
            seriesquadratic = new Series("Quadratic") { ChartType = SeriesChartType.Line, LegendText = "y=x^2" };
            seriessqrt = new Series("Sqrt") { ChartType = SeriesChartType.Line, LegendText = "y=sqrt(x)" };

            chart.Series.Add(seriessin);
            chart.Series.Add(seriesreciprocal);
            chart.Series.Add(seriesquadratic);
            chart.Series.Add(seriessqrt);

            foreach (var s in chart.Series)
            {
                s.ChartArea = "SeriaArena";
            }

            Legend legend = new Legend
            {
                Name = "legend",
                Font = new System.Drawing.Font("Arial", 13),
                Docking = Docking.Top
            };
            chart.Legends.Add(legend);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var s in chart.Series)
            {
                s.Points.Clear();
                s.IsVisibleInLegend = false;
            }
            tbForText.Clear();

            double a, b, h;

            try
            {
                a = Convert.ToDouble(tbA.Text);
                b = Convert.ToDouble(tbB.Text);
                h = Convert.ToDouble(tbH.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректное значение", "Неправильный формат данных");
                return;
            }

            if (cbY2.IsChecked == true)
                PlotFunction(Math.Sin, a, b, h, seriessin, "y=sin(x)");

            if (cbY4.IsChecked == true)
                PlotFunction(x => x != 0 ? 1 / x : double.NaN, a, b, h, seriesreciprocal, "y=1/x");

            if (cbY5.IsChecked == true)
                PlotFunction(x => x * x, a, b, h, seriesquadratic, "y=x^2");

            if (cbY6.IsChecked == true)
                PlotFunction(x => x >= 0 ? Math.Sqrt(x) : double.NaN, a, b, h, seriessqrt, "y=sqrt(x)");
        }

        private void PlotFunction(Func<double, double> func, double a, double b, double h, Series series, string functionName)
        {
            double x = a;
            string text = "";

            while (x <= b)
            {
                double y = func(x);
                if (!double.IsNaN(y) && !double.IsInfinity(y))
                {
                    series.Points.AddXY(x, y);
                    text += $"X:{Math.Round(x, 2)} Y:{Math.Round(y, 2)}" + Environment.NewLine;
                }
                x += h;
            }

            tbForText.Text += Environment.NewLine + $"График {functionName}" + Environment.NewLine + text;
            series.IsVisibleInLegend = true;
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var s in chart.Series)
            {
                s.Points.Clear();
            }
            tbA.Clear();
            tbB.Clear();
            tbH.Clear();
            tbForText.Clear();
        }

        private void tbA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[\d\+\-\,]");
        }

        private void tbB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[\d\+\-\,]");
        }

        private void tbH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[\d\+\-\,]");
        }
    }
}
