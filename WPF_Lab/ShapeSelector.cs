
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WPF_Lab
{
    public class ShapeSelector
    {
        public ObservableCollection<Shape> selectedShapes;
        private SelectedData selectedData;
        private Canvas canvas;

        public ShapeSelector(SelectedData selectedData, Canvas canvas)
        {
            selectedShapes = new ObservableCollection<Shape>();
            selectedShapes.CollectionChanged += SelectedShapesChanged;
            this.selectedData = selectedData;
            this.canvas = canvas;
        }

        private void SelectedShapesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (selectedShapes.Count > 0)
            {
                if (selectedData.CurrentShape != selectedShapes[selectedShapes.Count - 1])
                {
                    selectedData.IsSelected = true;
                    selectedData.CurrentShape = selectedShapes[selectedShapes.Count - 1];

                }

            }
            else
            {
                selectedData.IsSelected = false;
                selectedData.CurrentShape = null;
            }


        }

        public void Shape_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Shape shape = (Shape)sender;
            Canvas.SetZIndex(shape, 1);

            if (selectedShapes.Contains(shape))
                DeselectShape(shape);
            else SelectShape(shape);

        }

        public void DeselectShape(Shape shape)
        {
            selectedShapes.Remove(shape);
            Canvas.SetZIndex(shape, 0);
            shape.Effect = null;
        }

        public void SelectShape(Shape shape)
        {
            selectedShapes.Add(shape);
            ShapeDrawer.ApplyGlow(shape);
            Canvas.SetZIndex(shape, 1);
        }


        public void DeselectAllShapes()
        {
            while (selectedShapes.Count > 0)
                DeselectShape(selectedShapes.First());

        }
    }
}
