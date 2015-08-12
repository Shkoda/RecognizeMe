namespace GlobalPlay.Tools
{
    using System;
    using UnityEngine;

    public class TimeUtil
    {
        private static TimeSpan diffWithServer;

        public static String FormatTime(TimeSpan time)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (uint) time.TotalHours, time.Minutes, time.Seconds);
        }

        public static void SetServerTime(long serverTime)
        {
            DateTime time = UtcDateTimeFromJavaUtcTime(serverTime);
            diffWithServer = time - DateTime.UtcNow;
            Debug.Log(string.Format("Diff with server time = {0}", diffWithServer));
        }

        private static DateTime UtcDateTimeFromJavaUtcTime(long time)
        {
            if (time < 1000)
            {
                // The game will fail after this date
                return new DateTime(3000, 01, 01);
            }

            var utcDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            utcDateTime = utcDateTime.AddSeconds(Math.Round(time/1000d));
            return utcDateTime;
        }

        public static DateTime ToClientUtcTime(long serverTime)
        {
            return SyncToClientTime(UtcDateTimeFromJavaUtcTime(serverTime));
        }

        private static DateTime SyncToClientTime(DateTime time)
        {
            return time - diffWithServer;
        }
    }
}