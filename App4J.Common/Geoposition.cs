namespace PrayPal.Common
{
    public class Geoposition
    {
        public Geoposition(double longitude, double latitude, double? altitude)
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
        }

        public double Longitude { get; }
        public double Latitude { get; }
        public double? Altitude { get; }
    }
}