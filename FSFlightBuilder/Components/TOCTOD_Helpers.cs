using System;

namespace FSFlightBuilder.Components
{
    public static class TOCTOD_Helpers
    {
        //φ2 = asin(sin φ1 ⋅ cos δ + cos φ1 ⋅ sin δ ⋅ cos θ)

        //λ2 = λ1 + atan2(sin θ ⋅ sin δ ⋅ cos φ1, cos δ − sin φ1 ⋅ sin φ2)
        //where
        //φ is latitude, λ is longitude, θ is the bearing (clockwise from north), δ is the angular distance d/R; d being the distance travelled, R the earth’s radius

        public static GeoLocation FindPointAtDistanceFrom(GeoLocation startPoint, double initialBearing, double distanceKilometres)
        {
            const double radiusEarthKilometres = 6371.01;
            var initialBearingRadians = DegreesToRadians(initialBearing);
            var distRatio = distanceKilometres / radiusEarthKilometres; //angular distance
            var distRatioSine = Math.Sin(distRatio);
            var distRatioCosine = Math.Cos(distRatio);

            var startLatRad = DegreesToRadians(startPoint.Latitude);
            var startLonRad = DegreesToRadians(startPoint.Longitude);

            var startLatCos = Math.Cos(startLatRad);
            var startLatSin = Math.Sin(startLatRad);

            var lat2 =
                Math.Asin(startLatSin * distRatioCosine + startLatCos * distRatioSine * Math.Cos(initialBearingRadians));
            var lon2 = startLonRad + Math.Atan2(Math.Sin(initialBearingRadians) * distRatioSine * startLatCos,
                distRatioCosine - startLatSin * Math.Sin(lat2));

            return new GeoLocation
            {
                Latitude = RadiansToDegrees(lat2),
                Longitude = RadiansToDegrees(lon2)
            };
        }

        public struct GeoLocation
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        public static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }

        public static double BearingToDegrees(double bearing)
        {
            if (bearing > 360)
            {
                return BearingToDegrees(bearing - 360);
            }
            if (bearing < 0)
            {
                return BearingToDegrees(bearing + 360);
            }
            if (0 <= bearing && bearing <= 90)
            {
                return 90 - bearing;
            }
            if (90 < bearing && bearing <= 360)
            {
                return 360 - (bearing - 90);
            }
            return 0;
        }

        // Wind corrected
        public static int Groundspeed(double course, double trueAirspeed, double windDirection, double windSpeed)
        {
            double WTAngle = BearingToDegrees(course) - (BearingToDegrees(windDirection) + 180);
            return (int)Math.Round((trueAirspeed * Math.Cos(WCA(course, trueAirspeed, windDirection, windSpeed) * Math.PI / 180) + windSpeed * Math.Cos(WTAngle * Math.PI / 180)));
        }

        public static int WCA(double course, double trueAirspeed, double windDirection, double windSpeed)
        {
            double WTAngle = BearingToDegrees(course) - (BearingToDegrees(windDirection) + 180);
            return -(int)Math.Round((Math.Asin(windSpeed * Math.Sin(WTAngle * Math.PI / 180) / trueAirspeed) * (180 / Math.PI)));
        }

        public static int Heading(double course, double trueAirspeed, double windDirection, double windSpeed)
        {
            return (int)Math.Round(course + WCA(course, trueAirspeed, windDirection, windSpeed));
        }

        public static double DegreeBearing(GeoLocation start, GeoLocation finish)
        {
            var dLon = DegreesToRadians(finish.Longitude - start.Longitude);
            var dPhi = Math.Log(
                Math.Tan(DegreesToRadians(finish.Latitude) / 2 + Math.PI / 4) / Math.Tan(DegreesToRadians(start.Latitude) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (RadiansToDegrees(radians) + 360) % 360;
        }

        public static double Distance(GeoLocation start, GeoLocation finish, char unit = 'K')
        {
            var sCoord = new Location { Latitude = start.Latitude, Longitude = start.Longitude }; //new GeoCoordinate(start.Latitude, start.Longitude);
            var eCoord = new Location { Latitude = finish.Latitude, Longitude = finish.Longitude };

            var distanceInMeters = CalculateDistance(sCoord, eCoord); //.GetDistanceTo(eCoord);
            if (unit == 'K')
            {
                return distanceInMeters * 0.001; //KM
            }
            return distanceInMeters * 0.000539957; //NM
        }

        public static double Distance(double timeInMinutes, double speed, char unit = 'K')
        {
            var timeInHours = timeInMinutes / 60;
            if (unit == 'K')
            {
                speed = speed * 1.15078; //0.868976;
            }
            //Speed now in MPH
            //Distance in miles = time * speed
            var distInMPH = speed * timeInHours;

            //Return distance in Kilometers
            return distInMPH * 1.60934;
        }

        public static double TODDistance(double altitudeToDecend, double descentRate, double gs)
        {
            return (altitudeToDecend / descentRate) * (gs / 60);
        }

        public static double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
