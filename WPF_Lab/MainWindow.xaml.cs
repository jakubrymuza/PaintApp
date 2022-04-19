using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using System.Threading;


namespace WPF_Lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int initialShapesCount = 4;
        public List<Color> colors;
        private Point clickPosition;      
        public SelectedData selectedData { get; set; }
        public ShapeSelector shapeSelector;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            selectedData = new SelectedData();
            shapeSelector = new ShapeSelector(selectedData, myCanvas);

            InitializeComponent();
            allColorInfos = colorsList.Items;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            colors = new List<Color>();
            ColorLoader.LoadColors(this);

            Random rand = new Random();
            for (int i = 0; i < initialShapesCount; ++i)
                ShapeDrawer.AddRandomShape(myCanvas, rand, this);

        }

        public void DeleteSelected(object sender, RoutedEventArgs e)
        {
            foreach (Shape s in shapeSelector.selectedShapes)
            {
                myCanvas.Children.Remove(s);
            }

            shapeSelector.selectedShapes.Clear();
        }


        private void SetRandomColors(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();

            foreach (Shape s in shapeSelector.selectedShapes)
            {
                s.Fill = ShapeDrawer.GetRandomBrush(rand, this);
            }
        }

        private void AddRectangle(object sender, RoutedEventArgs e)
        {
            AddShape(new Rectangle());
        }

        private void AddEllipse(object sender, RoutedEventArgs e)
        {
            AddShape(new Ellipse());
        }

        private void AddShape(Shape shape)
        {
            shapeSelector.DeselectAllShapes();
            ShapeDrawer drawer = new ShapeDrawer(shape, myCanvas, this);
            drawer.AddShape();
            myCanvas.Children.Add(shape);
        }

        private void ExportCanvas(object sender, RoutedEventArgs e)
        {
            CanvasSaver saver = new CanvasSaver(myCanvas);
            saver.SaveCanvas();

        }

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            if (Thread.CurrentThread.CurrentUICulture.Name == "pl-PL")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            else Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");


            // TODO
        }

        public static ItemCollection allColorInfos;

        public static ColorInfo FindColorInfo(Color color)
        {
            foreach (ColorInfo x in allColorInfos)
                if (color.Equals(x.Rgb))
                    return x;
            return null;
        }


        public void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.OverrideCursor == Cursors.Cross)
                return;

            if (!shapeSelector.selectedShapes.Contains((Shape)sender))
            {
                shapeSelector.DeselectAllShapes();
                shapeSelector.SelectShape((Shape)sender);
            }

            Mouse.OverrideCursor = Cursors.ScrollAll;
            myCanvas.MouseMove += MyCanvas_MouseMove;
            myCanvas.MouseLeftButtonUp += MyCanvas_MouseLeftButtonUp;
            clickPosition = e.GetPosition(myCanvas);

            e.Handled = true;
        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myCanvas.MouseLeftButtonUp -= MyCanvas_MouseLeftButtonUp;
            myCanvas.MouseMove -= MyCanvas_MouseMove;
            Mouse.OverrideCursor = null;

            foreach (Shape s in shapeSelector.selectedShapes)
                ApplyTranslateTransform(s);


            Mouse.Capture(null);
        }

        private void ApplyTranslateTransform(Shape s)
        {
            TranslateTransform transform = ((TransformGroup)s.RenderTransform).Children[0] as TranslateTransform;

            Canvas.SetLeft(s, Canvas.GetLeft(s) + transform.X);
            Canvas.SetTop(s, Canvas.GetTop(s) + transform.Y);
            transform.X = transform.Y = 0;
        }



        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Mouse.Capture(myCanvas);
            foreach (Shape shape in shapeSelector.selectedShapes)
            {

                Point currentPosition = e.GetPosition(myCanvas);

                TranslateTransform transform = ((TransformGroup)shape.RenderTransform).Children[0] as TranslateTransform;

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;
            }

        }


        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
                shapeSelector.DeselectAllShapes();
        }



    }









}
