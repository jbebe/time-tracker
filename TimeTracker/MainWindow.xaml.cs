using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IniParser;
using IniParser.Model;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserConfig UserConfig { get; set; }

        public VisualConfig VisualConfig { get; set; }

        private Timer TimelineTimer { get; set; }

        public ClockHand ClockHand { get; set; }

        public int ActiveWidth => 
            VisualConfig.ScreenWidth - VisualConfig.MarginLeft - VisualConfig.MarginRight;

        public decimal WorktimeAmount =>
            UserConfig.WorkTimeEnd.Ticks - UserConfig.WorkTimeStart.Ticks;

        public MainWindow()
        {
            InitializeComponent();

            // Custom
            LoadConfiguration();
            InitVisuals();
        }

        private void LoadConfiguration()
        {
            UserConfig = UserConfig.FromIni("config.ini");
            VisualConfig = new VisualConfig
            {
                ScreenWidth = (int)SystemParameters.PrimaryScreenWidth,
                ScreenHeight = (int)SystemParameters.PrimaryScreenHeight,
                MarginLeft = 50,
                TaskbarDimenions = Helpers.GetTaskbarDimensions(),
            };
        }

        private void InitVisuals()
        {
            // Set up window size
            this.Top = UserConfig.VerticalPosition ?? VisualConfig.ScreenHeight - (this.Height + VisualConfig.TaskbarDimenions.Height);
            this.Left = 0;
            this.Width = VisualConfig.ScreenWidth;

            // Set up timeline values
            elTimeline.Children.Clear();
            UserConfig
                .GetTimelineSteps()
                .ForEach(x =>
                {
                    var tb = new TextBlock
                    {
                        Style = FindResource("TimelineTextStyle") as Style,
                        Text = new DateTime(x.Ticks, DateTimeKind.Local).ToString("HH:mm"),
                    };
                    // POS_LEFT / (SCREEN_WIDTH - MARGIN_LEFT - MARGIN-RIGHT) = TIME_X / (WorkEnd - WorkStart)
                    decimal left = ((x.Ticks * ActiveWidth) / WorktimeAmount) - ActiveWidth + VisualConfig.MarginLeft;
                    Canvas.SetLeft(tb, (int)left);

                    elTimeline.Children.Add(tb);
                });

            // Set up current time pointer
            ClockHand = new ClockHand();
            Canvas.SetLeft(ClockHand, VisualConfig.MarginLeft);
            elTimeline.Children.Add(ClockHand);
            TimelineTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
            TimelineTimer.Elapsed += TimelineTimer_Elapsed;
            TimelineTimer.Start();
        }

        private void TimelineTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            var x = now.TimeOfDay;

            decimal left;
            if (x < UserConfig.WorkTimeStart)
                left = VisualConfig.MarginLeft / 2;
            else if (x > UserConfig.WorkTimeEnd)
                left = ActiveWidth + VisualConfig.MarginLeft / 2;
            else 
             left = ((x.Ticks * ActiveWidth) / WorktimeAmount) - ActiveWidth + VisualConfig.MarginLeft;

            Dispatcher.Invoke(() => Canvas.SetLeft(ClockHand, (int)left));
        }
    }
}
