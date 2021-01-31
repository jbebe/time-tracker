using IniParser;
using System;
using System.Collections.Generic;
using System.Globalization;
using static TimeTracker.Helpers;

namespace TimeTracker
{
    public class UserConfig
    {
        public TimeSpan WorkTimeStart { get; set; }

        public TimeSpan WorkTimeEnd { get; set; }

        public string TimelineFormat { get; set; }

        public TimeSpan TimelineResolution { get; set; }

        public int? VerticalPosition { get; set; }

        public string DatabasePath { get; set; }

        public static UserConfig FromIni(string iniPath)
        {
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(iniPath);
            var workTimeStart = data["Work"]["WorkTimeStart"];
            var workTimeEnd = data["Work"]["WorkTimeEnd"];
            var timelineFormat = data["Visual"]["TimelineFormat"];
            var timelineResolution = data["Visual"]["TimelineResolution"];
            var filledVerticalPosition = int.TryParse(data["Visual"]["VerticalPosition"], out var verticalPosition);
            var databasePath = data["IO"]["DatabasePath"];

            return new UserConfig
            {
                WorkTimeStart = DateTime.ParseExact(workTimeStart, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay,
                WorkTimeEnd = DateTime.ParseExact(workTimeEnd, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay,
                TimelineFormat = timelineFormat,
                TimelineResolution = TimeSpan.FromSeconds(double.Parse(timelineResolution)),
                VerticalPosition = filledVerticalPosition ? verticalPosition : null as int?,
                DatabasePath = databasePath,
            };
        }

        public List<TimeSpan> GetTimelineSteps()
        {
            var result = new List<TimeSpan>();
            var currentDate = WorkTimeStart;
            while (currentDate <= WorkTimeEnd)
            {
                result.Add(currentDate);
                currentDate += TimelineResolution;
            }

            return result;
        }
    }

    public class VisualConfig
    {
        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        public int MarginLeft { get; set; }

        public int MarginRight { get => MarginLeft; }

        public TaskbarDimenions TaskbarDimenions { get; set; }
    }
}
