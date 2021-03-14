using System;

namespace Mapsui.Providers.ArcGIS.Image
{
    public class TimeInfo
    {
        public string startTimeField { get; set; }
        public string endTimeField { get; set; }
        public long[] timeExtent { get; set; }
        public TimeReference timeReference { get; set; }

        public DateTime? StartDate
        {
            get
            {
                return timeExtent.Length == 2 ? ConvertUnixTimeStamp(timeExtent[0]) : ConvertUnixTimeStamp(0);
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return timeExtent.Length == 2 ? ConvertUnixTimeStamp(timeExtent[1]) : ConvertUnixTimeStamp(0);
            }
        }

        public static DateTime? ConvertUnixTimeStamp(long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(unixTimeStamp);
        }
    }
}
