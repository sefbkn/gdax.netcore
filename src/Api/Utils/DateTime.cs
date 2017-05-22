using System;

namespace Boukenken.Gdax
{
    public static class DateTimeUtilities
    {
        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}