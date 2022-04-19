using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;


namespace WPF_Lab
{
    public class CanvasSaver
    {
        private Canvas canvas;

        public CanvasSaver(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SaveCanvas()
        {
            SaveFileDialog save = new SaveFileDialog();

            save.Filter = "PNG file(*.png)|*.png";

            if (save.ShowDialog() == true)
            {
                RenderTargetBitmap bitmap = GetBitmap();
                BitmapFrame frame = BitmapFrame.Create(bitmap);
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(frame);

                using (var stream = File.Create(save.FileName))
                {
                    encoder.Save(stream);
                }

            }
        }
        private RenderTargetBitmap GetBitmap()
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96;

            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, PixelFormats.Default);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            bitmap.Render(dv);

            return bitmap;
        }
    }
}
