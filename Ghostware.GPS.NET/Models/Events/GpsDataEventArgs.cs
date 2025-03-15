using System;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.GpsdModels;
using Ghostware.NMEAParser.NMEAMessages;

namespace Ghostware.GPS.NET.Models.Events
{
    public class GpsDataEventArgs : EventArgs
    {
        public GpsCoordinateSystem CoordinateSystem { get; set; } = GpsCoordinateSystem.GeoEtrs89;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Speed { get; set; }

        public GpsLocation Location { get; set; }
       
        public GpsDataEventArgs(GpsLocation gpsLocation)
        {
            Latitude = gpsLocation.Latitude;
            Longitude = gpsLocation.Longitude;
            Speed = gpsLocation.Speed;

            Location = gpsLocation;
        }

        public GpsDataEventArgs(GprmcMessage gpsLocation)
        {
            Latitude = gpsLocation.Latitude;
            Longitude = gpsLocation.Longitude;
            Speed = gpsLocation.Speed;
        }

        public GpsDataEventArgs(double latitude, double longitude, double speed = 0.0d)
        {
            Latitude = latitude;
            Longitude = longitude;
            Speed = speed;
        }

        public override string ToString()
        {
            return $"Latitude: {Latitude} - Longitude: {Longitude} - Speed: {Speed}";
        }
    }
}
