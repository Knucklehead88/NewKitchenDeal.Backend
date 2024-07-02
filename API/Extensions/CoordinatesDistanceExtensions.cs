using Core.Entities.Identity;

namespace API.Extensions
{
    public static class CoordinatesDistanceExtensions
    {
        public static double DistanceTo(this Location location, double targetLongitude, double targetLatitude)
        {
            return DistanceTo(location, targetLongitude, targetLatitude, UnitOfLength.Kilometers);
        }

        public static double DistanceTo(this Location location, double targetLongitude, double targetLatitude, UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * location.Latitude / 180;
            var targetRad = Math.PI * targetLatitude / 180;
            var theta = location.Longitude - targetLongitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
    }

    public class UnitOfLength
    {
        public static UnitOfLength Kilometers = new(1.609344);
        public static UnitOfLength NauticalMiles = new(0.8684);
        public static UnitOfLength Miles = new(1);

        private readonly double _fromMilesFactor;

        private UnitOfLength(double fromMilesFactor)
        {
            _fromMilesFactor = fromMilesFactor;
        }

        public double ConvertFromMiles(double input)
        {
            return input * _fromMilesFactor;
        }
    }
}
