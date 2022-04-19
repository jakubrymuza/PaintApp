using System.Linq;
using System.Windows.Media;
using System.Reflection;


namespace WPF_Lab
{
    public static class ColorLoader
    {
        public static void LoadColors(MainWindow window)
        {
            var props = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var colorInfos = props.Select(prop =>
            {
                var color = (Color)prop.GetValue(null, null);

                return new ColorInfo()
                {
                    Name = prop.Name,
                    Rgb = color,
                    RgbInfo = $"R:{ color.R} G:{ color.G}, B:{ color.B}"
                };
            });

            colorInfos = colorInfos.Where((col) => col.Name != "Transparent");

            foreach (var ci in colorInfos)
                window.colors.Add(ci.Rgb);

            window.DataContext = colorInfos;
        }
    }
}
