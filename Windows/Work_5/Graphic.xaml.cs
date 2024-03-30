using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace practice.Windows.Work_5
{
    /// <summary>
    /// Логика взаимодействия для Graphic.xaml
    /// </summary>
    public partial class Graphic : Window
    {
        const int countDot = 16;
        double[] dots = new double[countDot];

        DrawingGroup drawingGroup = new DrawingGroup();

        double xd(double x)
        {
            return x * x - Math.Cos(x);
        }

        void DataFill()
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i] = xd(i - countDot / 2);
            }
        }

        void BackgroundFun()
        {
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            RectangleGeometry rectGeometry = new RectangleGeometry();

            rectGeometry.Rect = new Rect(-0.5, -0.4, 2, 1);
            geometryDrawing.Geometry = rectGeometry;

            geometryDrawing.Pen = new Pen(Brushes.Red, 0.005);
            geometryDrawing.Brush = Brushes.Beige;

            drawingGroup.Children.Add(geometryDrawing);
        }

        private void GridFun()
        {
            GeometryGroup geometryGroup = new GeometryGroup();

            for (int i = 1; i < 10; i++)
            {
                LineGeometry line = new LineGeometry(new Point(-.5, (-0.4) + i * 0.1),
                    new Point(1.6, (-0.4) + i * 0.1));
                geometryGroup.Children.Add(line);
            }

            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = geometryGroup;

            geometryDrawing.Pen = new Pen(Brushes.Gray, 0.01);
            double[] dashes = { 1, 1, 1, 1, 1 };
            geometryDrawing.Pen.DashStyle = new DashStyle(dashes, -.1);

            geometryDrawing.Brush = Brushes.Beige;

            drawingGroup.Children.Add(geometryDrawing);
        }

        private void Sinful()
        {
            GeometryGroup geometryGroup = new GeometryGroup();

            double lowerK = 80;

            for (int i = 0; i < dots.Length - 1; i++)
            {
                LineGeometry line = new LineGeometry(
                new Point((double)i / (double)countDot,
                    .5 - (dots[i] / lowerK)),

                new Point((double)(i + 1) / (double)countDot,
                    .5 - (dots[i + 1] / lowerK)));

                geometryGroup.Children.Add(line);
            }

            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = geometryGroup;

            geometryDrawing.Pen = new Pen(Brushes.Blue, 0.01);

            drawingGroup.Children.Add(geometryDrawing);
        }
        private void MarkerFun()
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            for (int i = 0; i <= 10; i++)
            {
                FormattedText formattedText = new FormattedText(
                String.Format("{0,7:F}", 8 - i),
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                0.05,
                Brushes.Black);
                formattedText.SetFontWeight(FontWeights.Bold);
                Geometry geometry = formattedText.BuildGeometry(new Point(-.7, (-0.45) + i * 0.1));
                geometryGroup.Children.Add(geometry);
            }
                
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = geometryGroup;
            geometryDrawing.Brush = Brushes.LightGray;
            geometryDrawing.Pen = new Pen(Brushes.Gray, 0.003);
            drawingGroup.Children.Add(geometryDrawing);
        }

        public Graphic()
        {
            InitializeComponent();

            DataFill();

            BackgroundFun();
            GridFun();
            Sinful();
            MarkerFun();

            imaje.Source = new DrawingImage(drawingGroup);
        }


    }
}
