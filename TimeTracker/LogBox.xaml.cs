using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace TimeTracker
{
    // Adorners must subclass the abstract base class Adorner.
    public class SimpleCircleAdorner : Adorner
    {
        // Be sure to call the base class constructor.
        public SimpleCircleAdorner(UIElement adornedElement)
          : base(adornedElement)
        {
        }

        // A common way to implement an adorner's rendering behavior is to override the OnRender
        // method, which is called by the layout system as part of a rendering pass.
        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;

            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }
    }

    /// <summary>
    /// Interaction logic for LogBox.xaml
    /// </summary>
    public partial class LogBox : UserControl
    {
        public LogEntity LogEntity { get; set; }

        public LogBox()
        {
            InitializeComponent();

            /*Id.Text = LogEntity.Id;
            Time.Text = $"{LogEntity.Time.Hours}:{LogEntity.Time.Minutes}";
            Comment.Text = LogEntity.Comment;*/

            /*var myAdornerLayer = AdornerLayer.GetAdornerLayer(this);
            myAdornerLayer.Add(new SimpleCircleAdorner(this));*/
        }

        public void Update()
        {
            el_Id.Text = LogEntity.Id;
            el_From.Text = $"{LogEntity.From.Hours}:{LogEntity.From.Minutes}";
            el_Comment.Text = LogEntity.Comment;
        }
    }

    public class LogEntity
    {
        public string Id { get; set; }

        public TimeSpan From { get; set; }

        public TimeSpan To { get; set; }

        public string Comment { get; set; }
    }
}
