using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace WPF_Lab
{
    public class ShapeDrawer
    {
        private Shape shape;
        private Canvas canvas;
        private MainWindow window;
        private Point startingPoint;

        public ShapeDrawer(Shape shape, Canvas canvas, MainWindow window)
        {
            this.shape = shape;
            this.canvas = canvas;
            this.window = window;
        }

        public void AddShape()
        {
            Mouse.OverrideCursor = Cursors.Cross;
            canvas.MouseLeftButtonDown += DrawShape;
            canvas.MouseLeftButtonUp += EndDrawing;
        }

        private void DrawShape(object sender, MouseButtonEventArgs e)
        {
            ShapeCreate(shape, new Random(), window);

            startingPoint = e.GetPosition(canvas);

            Canvas.SetLeft(shape, startingPoint.X);
            Canvas.SetTop(shape, startingPoint.Y);

            shape.Height = 0;
            shape.Width = 0;


            Mouse.Capture(canvas);

            canvas.MouseMove += ResizeDrawing;

            e.Handled = true;
        }

        public static void InitizalizeTransforms(Shape shape)
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new TranslateTransform());
            transformGroup.Children.Add(new RotateTransform(0, shape.Width / 2, shape.Height / 2));
            shape.RenderTransform = transformGroup;
        }

        private void EndDrawing(object sender, MouseButtonEventArgs e)
        {

            Mouse.OverrideCursor = null;
            canvas.MouseMove -= ResizeDrawing;
            canvas.MouseLeftButtonDown -= DrawShape;
            canvas.MouseLeftButtonUp -= EndDrawing;

            InitizalizeTransforms(shape);

            Mouse.Capture(null);
            e.Handled = true;
        }

        private void ResizeDrawing(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(canvas).Y < startingPoint.Y)
                Canvas.SetTop(shape, e.GetPosition(canvas).Y);

            if (e.GetPosition(canvas).X < startingPoint.X)
                Canvas.SetLeft(shape, e.GetPosition(canvas).X);

            shape.Height = Math.Abs(e.GetPosition(canvas).Y - startingPoint.Y);
            shape.Width = Math.Abs(e.GetPosition(canvas).X - startingPoint.X);

        }


        public static void ApplyGlow(Shape s)
        {
            DropShadowEffect glow = new DropShadowEffect();
            glow.BlurRadius = 50;
            glow.Direction = 270;
            glow.Color = Colors.White;
            s.Effect = glow;
        }

        private const int minShapeSize = 100;
        private const int maxShapeSize = 300;
        public static void AddRandomShape(Canvas c, Random rand, MainWindow window)
        {
            Shape s;

            if (rand.Next(0, 2) == 0)
                s = new Ellipse();
            else s = new Rectangle();

            s.Height = rand.Next(minShapeSize, maxShapeSize);
            s.Width = rand.Next(minShapeSize, maxShapeSize);
            Canvas.SetLeft(s, rand.Next((int)c.ActualWidth - (int)s.Width));
            Canvas.SetTop(s, rand.Next((int)c.ActualHeight - (int)s.Height));

            ShapeCreate(s, rand, window);

            c.Children.Add(s);

            InitizalizeTransforms(s);
        }

        public static SolidColorBrush GetRandomBrush(Random rand, MainWindow window) => new SolidColorBrush(window.colors[rand.Next(window.colors.Count - 1)]);

        public static void ShapeCreate(Shape s, Random rand, MainWindow window)
        {

            s.Fill = GetRandomBrush(rand, window);
            s.MouseRightButtonDown += window.shapeSelector.Shape_MouseRightButtonDown;
            s.Cursor = Cursors.Hand;
            s.MouseLeftButtonDown += window.Shape_MouseLeftButtonDown;

        }


    }
}
