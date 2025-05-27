using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Report
{
    public sealed partial class ReportPage : UserControl
    {
        public ReportViewModel ViewModel { get; }

        public ReportPage()
        {
            this.InitializeComponent();

            ViewModel = App.Services!.GetRequiredService<ReportViewModel>();

            // Đăng ký sự kiện để vẽ lại biểu đồ khi dữ liệu thay đổi
            ((INotifyCollectionChanged)ViewModel.Reports).CollectionChanged += OnReportsCollectionChanged;

            // Tải dữ liệu ban đầu
            _ = ViewModel.LoadReportDataAsync();

            // Đăng ký sự kiện thay đổi kích thước
            ChartContainer.SizeChanged += ChartContainer_SizeChanged;
        }

        private void OnReportsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DrawChart();
        }

        private void ChartContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawChart();
        }

        private void DrawChart()
        {
            if (ViewModel.Reports.Count == 0)
                return;

            // Xóa các phần tử cũ
            ChartCanvas.Children.Clear();

            // Lấy kích thước của canvas
            double width = ChartContainer.ActualWidth;
            double height = ChartContainer.ActualHeight;

            if (width <= 0 || height <= 0)
                return;

            // Thiết lập margin
            double margin = 40;
            double chartWidth = width - 2 * margin;
            double chartHeight = height - 2 * margin;

            // Vẽ trục
            Line yAxis = new Line
            {
                X1 = margin,
                Y1 = margin,
                X2 = margin,
                Y2 = height - margin,
                Stroke = Application.Current.Resources["TextFillColorSecondaryBrush"] as SolidColorBrush,
                StrokeThickness = 1
            };

            Line xAxis = new Line
            {
                X1 = margin,
                Y1 = height - margin,
                X2 = width - margin,
                Y2 = height - margin,
                Stroke = Application.Current.Resources["TextFillColorSecondaryBrush"] as SolidColorBrush,
                StrokeThickness = 1
            };

            ChartCanvas.Children.Add(yAxis);
            ChartCanvas.Children.Add(xAxis);

            // Tìm giá trị lớn nhất cho trục Y
            decimal maxTotal = ViewModel.Reports.Max(r => r.total);
            double yScale = chartHeight / (double)maxTotal;

            // Tính toán chiều rộng của mỗi cột
            double barWidth = chartWidth / ViewModel.Reports.Count;
            double barMargin = barWidth * 0.2;
            double actualBarWidth = barWidth - barMargin;

            // Vẽ các cột và nhãn
            for (int i = 0; i < ViewModel.Reports.Count; i++)
            {
                var report = ViewModel.Reports[i];
                double barHeight = (double)report.total * yScale;

                // Vẽ cột
                Rectangle bar = new Rectangle
                {
                    Width = actualBarWidth,
                    Height = barHeight,
                    Fill = Application.Current.Resources["SystemAccentColor"] as SolidColorBrush,
                    RadiusX = 4,
                    RadiusY = 4
                };

                double x = margin + i * barWidth + barMargin / 2;
                double y = height - margin - barHeight;

                Canvas.SetLeft(bar, x);
                Canvas.SetTop(bar, y);
                ChartCanvas.Children.Add(bar);

                // Thêm nhãn tên sản phẩm
                TextBlock nameLabel = new TextBlock
                {
                    Text = report.NameProduct,
                    FontSize = 10,
                    TextWrapping = TextWrapping.Wrap,
                    Width = actualBarWidth,
                    TextAlignment = TextAlignment.Center
                };

                Canvas.SetLeft(nameLabel, x);
                Canvas.SetTop(nameLabel, height - margin + 5);
                ChartCanvas.Children.Add(nameLabel);

                // Thêm nhãn giá trị
                TextBlock valueLabel = new TextBlock
                {
                    Text = report.total.ToString("N0") + " đ",
                    FontSize = 10,
                    TextAlignment = TextAlignment.Center,
                    Width = actualBarWidth
                };

                Canvas.SetLeft(valueLabel, x);
                Canvas.SetTop(valueLabel, y - 20);
                ChartCanvas.Children.Add(valueLabel);
            }

            // Thêm các mốc giá trị trục Y
            int yDivisions = 5;
            for (int i = 0; i <= yDivisions; i++)
            {
                decimal value = maxTotal * i / yDivisions;
                double y = height - margin - ((double)value * yScale);

                // Vẽ đường kẻ ngang
                Line gridLine = new Line
                {
                    X1 = margin,
                    Y1 = y,
                    X2 = width - margin,
                    Y2 = y,
                    Stroke = Application.Current.Resources["TextFillColorTertiaryBrush"] as SolidColorBrush,
                    StrokeThickness = 0.5,
                    StrokeDashArray = new DoubleCollection { 2, 2 }
                };

                ChartCanvas.Children.Add(gridLine);

                // Thêm giá trị
                TextBlock yLabel = new TextBlock
                {
                    Text = value.ToString("N0"),
                    FontSize = 10
                };

                Canvas.SetLeft(yLabel, margin - 35);
                Canvas.SetTop(yLabel, y - 10);
                ChartCanvas.Children.Add(yLabel);
            }
        }
    }
}
