using System;

internal static class GlobalPos
{
    internal static readonly Pos EMPTY_POS = new Pos();
    internal const float MAX_SECONDS = 59.98f;
    internal const double EARTH_RADIUS_METER = 6371.0 * 1000.0;

    internal static void endpointRad(double lonX, double latY, double distance, double angle, ref double endLonX, ref double endLatY)
    {
        endLatY = Math.Asin(Math.Sin(latY) * Math.Cos(distance) + Math.Cos(latY) * Math.Sin(distance) * Math.Cos(angle));

        double dlon = Math.Atan2(Math.Sin(angle) * Math.Sin(distance) * Math.Cos(latY), Math.Cos(distance) - Math.Sin(latY) * Math.Sin(endLatY));
        endLonX = Math.IEEERemainder(lonX - dlon + Math.PI, 2 * Math.PI) - Math.PI;
    }

    /* Degree to rad */
    internal static double toRadians(double deg)
    {
        return (Math.PI / 180) * deg;
    }

    /* Rad to degree */
    internal static double toDegrees(double deg)
    {
        return deg * (180.0 / Math.PI);
    }

    /* NM to rad (longitude or latitude) */
    private static double nmToRad(double value)
    {
        return (value > double.MaxValue / 2) ? value : (Math.PI / (180.0 * 60.0) * value);
    }

    /* meter to rad (longitude or latitude) */
    internal static double meterToRad(double value)
    {
        return (value > double.MaxValue / 2) ? value : nmToRad(meterToNm(value));
    }

    /* Distance from meters to nautical miles */
    internal static double meterToNm(double value)
    {
        return (value > double.MaxValue / 2) ? value : (value / 1852.216);
    }

    /* Normalizes a number to an arbitrary range by assuming the range wraps around when going below min or above max */
    internal static double normalizeRange(double value, double start, double end)
    {
        var width = end - start;
        var offsetValue = value - start; // Make value relative to 0

        // Reset back to start of original range by adding start
        return (offsetValue - ((offsetValue / width) * width)) + start;
    }

    /* Normalize lonx to -180 < lonx < 180 */
    internal static double normalizeLonXDeg(double lonX)
    {
        return normalizeRange(lonX, -180.0, 180.0);
    }

    /* Normalize laty to -90 < laty < 90 */
    internal static double normalizeLatYDeg(double latY)
    {
        return normalizeRange(latY, -90.0, 90.0);
    }

}

///*****************************************************************************
//* Copyright 2015-2019 Alexander Barthel alex@littlenavmap.org
//*
//* This program is free software: you can redistribute it and/or modify
//* it under the terms of the GNU General Public License as published by
//* the Free Software Foundation, either version 3 of the License, or
//* (at your option) any later version.
//*
//* This program is distributed in the hope that it will be useful,
//* but WITHOUT ANY WARRANTY; without even the implied warranty of
//* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//* GNU General Public License for more details.
//*
//* You should have received a copy of the GNU General Public License
//* along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************/

///*****************************************************************************
//* Copyright 2015-2019 Alexander Barthel alex@littlenavmap.org
//*
//* This program is free software: you can redistribute it and/or modify
//* it under the terms of the GNU General Public License as published by
//* the Free Software Foundation, either version 3 of the License, or
//* (at your option) any later version.
//*
//* This program is distributed in the hope that it will be useful,
//* but WITHOUT ANY WARRANTY; without even the implied warranty of
//* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//* GNU General Public License for more details.
//*
//* You should have received a copy of the GNU General Public License
//* along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************/

public enum CrossTrackStatus
{
    INVALID, // No distance found
    ALONG_TRACK, // Point is along track
    BEFORE_START, // Point is before start - distance is point to start
    AFTER_END // Point is after end - distance is point to end
}

/* Result for point to line distance measurement. All distances in meter. */
public class LineDistance
{
    public CrossTrackStatus status = CrossTrackStatus.INVALID;

    /* All distances in meter */
    public float distance; // Along track distance from point to end of line or linestring -  Along track distance from point to start of line or linestring - /* Cross track distance - Positive means right of course,  negative means left of course.
    public float distanceFrom1;
    public float distanceFrom2;
    //*  Can also contain distance to nearest point depending on status */
}

/*
 * Geographic position class. Calculations based on
 * http://williams.best.vwh.net/avform.htm
 */
public class Pos
{
    const string OVERFLOW_60_TEST = "%1";
    const string OVERFLOW_60_TEST_TEXT = "60";

    private static double INVALID_VALUE = double.MaxValue;

    private double altitude;
    private double lonX = INVALID_VALUE;
    private double latY = INVALID_VALUE;

    public Pos()
    {
        this.lonX = INVALID_VALUE;
        this.latY = INVALID_VALUE;
        this.altitude = 0.0f;
    }

    public Pos(int lonXDeg, int lonXMin, float lonXSec, bool west, int latYDeg, int latYMin, float latYSec, bool south, float alt = 0.0f)
    {
        lonX = (lonXDeg + lonXMin / 60.0f + lonXSec / 3600.0f) * (west ? -1.0f : 1.0f);
        latY = (latYDeg + latYMin / 60.0f + latYSec / 3600.0f) * (south ? -1.0f : 1.0f);
        altitude = alt;
    }

    public Pos(float longitudeX, float latitudeY, float alt = 0.0f)
    {
        this.lonX = longitudeX;
        this.latY = latitudeY;
        this.altitude = alt;
    }
    public Pos(double longitudeX, double latitudeY, double alt = 0.0)
    {
        lonX = (float)longitudeX;
        latY = (float)latitudeY;
        altitude = (float)alt;
    }

    public double getLatY()
    {
        return latY;
    }

    public int getLatYDeg()
    {
        return deg(latY);
    }

    public int getLatYMin()
    {
        return min(latY);
    }

    public double getLatYSec()
    {
        return sec(latY);
    }

    public double getLonX()
    {
        return lonX;
    }

    public int getLonXDeg()
    {
        return deg(lonX);
    }

    public int getLonXMin()
    {
        return min(lonX);
    }

    public double getLonXSec()
    {
        return sec(lonX);
    }

    public double getAltitude()
    {
        return altitude;
    }

    /* Normalize this position to -180 < lonx < 180 and -90 < laty < 90 and return reference */
    public Pos normalize()
    {
        if (isValid())
        {
            lonX = GlobalPos.normalizeLonXDeg(lonX);
            latY = GlobalPos.normalizeLatYDeg(latY);
        }
        return this;
    }

    //  /* return endpoint at distance and angle */
    public Pos endpoint(double distanceMeter, double angleDeg)
    {
        if (!isValid())
        {
            return GlobalPos.EMPTY_POS;
        }

        if (distanceMeter == 0.0f)
        {
            return this;
        }

        double lon = 0;
        double lat = 0;

        GlobalPos.endpointRad(GlobalPos.toRadians((double)lonX), GlobalPos.toRadians((double)latY), GlobalPos.meterToRad((double)distanceMeter), GlobalPos.toRadians(-(double)angleDeg + 360.0), ref lon, ref lat);

        return new Pos((float)GlobalPos.toDegrees(lon), (float)GlobalPos.toDegrees(lat));
    }

    /* false if position is not initialized */
    public bool isValid()
    {
        // Use half value to get around calculations
        return lonX < INVALID_VALUE / 2.0f && latY < INVALID_VALUE / 2.0f;
    }

    /* Return true if close to any pole */
    public bool isPole()
    {
        return isValid() && (latY > 89.0f || latY < -89.0f);
    }

    public void setLonX(double value)
    {
        lonX = value;
    }

    public void setLatY(double value)
    {
        latY = value;
    }

    public void setAltitude(double value)
    {
        altitude = value;
    }

    private double sec(double value)
    {
        double min = (value - (int)value) * 60.0f;
        double seconds = (min - (int)min) * 60.0f;

        // Avoid 60 seconds due to rounding up when converting to text
        if (doesOverflow60(seconds))
        {
            return 0.0f;
        }
        else
        {
            return seconds;
        }
    }

    private int min(double value)
    {
        double min = (value - (int)value) * 60.0f;
        double seconds = (min - (int)min) * 60.0f;
        int minutes = (int)min;

        // Avoid 60 seconds due to rounding up when converting to text
        if (doesOverflow60(seconds))
        {
            minutes += (minutes > 0 ? 1 : -1);
        }
        if (Math.Abs(minutes) >= 60)
        {
            minutes = 0;
        }

        return minutes;
    }

    private int deg(double value)
    {
        double min = (value - (int)value) * 60.0f;
        double seconds = (min - (int)min) * 60.0f;

        int degrees = (int)value;
        int minutes = (int)min;

        // Avoid 60 seconds due to rounding up when converting to text
        if (doesOverflow60(seconds))
        {
            minutes += (minutes > 0 ? 1 : -1);
        }
        if (Math.Abs(minutes) >= 60)
        {
            degrees += (degrees > 0 ? 1 : -1);
        }

        return degrees;
    }

    /* Check if seconds or minutes value is rounded up to 60.00 when convertin to string */
    private bool doesOverflow60(double value)
    {
        // No locale specifics used here
        // String conversion is only option to catch system dependend rounding variances
        return Math.Round(value) >= 60;
    }

    internal static double Bearing(Pos pt1, Pos pt2)
    {
        return Bearing(pt1.getLatY(), pt1.getLonX(), pt2.getLatY(), pt2.getLonX());
    }

    internal static double Bearing(double fromLat, double fromLon, double toLat, double toLon)
    {
        double x = Math.Cos(DegreesToRadians(fromLat)) * Math.Sin(DegreesToRadians(toLat)) - Math.Sin(DegreesToRadians(fromLat)) * Math.Cos(DegreesToRadians(toLat)) * Math.Cos(DegreesToRadians(toLon - fromLon));
        double y = Math.Sin(DegreesToRadians(toLon - fromLon)) * Math.Cos(DegreesToRadians(toLat));

        // Math.Atan2 can return negative value, 0 <= output value < 2*PI expected 
        return (Math.Atan2(y, x) + Math.PI * 2) % (Math.PI * 2);
    }

    internal static double RhumbBearingTo(Pos pos, double lat, double lng)
    {
        double lat1 = DegreesToRadians(pos.getLatY());
        double lat2 = DegreesToRadians(lat);
        double dLon = DegreesToRadians(lng - pos.getLonX());

        double dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
        if (Math.Abs(dLon) > Math.PI) dLon = (dLon > 0) ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
        double brng = Math.Atan2(dLon, dPhi);

        return (RadianToDegree(brng) + 360) % 360;
    } // end RhumbBearingTo

    internal static double BearingTo(Pos pos, double lat, double lng)
    {
        return BearingTo(pos.getLatY(), pos.getLonX(), lat, lng);
    } // end BearingTo

    internal static double BearingTo(double fromLat, double fromLon, double lat, double lng)
    {
        double lat1 = DegreesToRadians(fromLat);
        double lat2 = DegreesToRadians(lat);
        double dLon = DegreesToRadians(lng) - DegreesToRadians(fromLon);

        double y = Math.Sin(dLon) * Math.Cos(lat2);
        double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
        double brng = Math.Atan2(y, x);

        return (RadianToDegree(brng) + 360) % 360;
    } // end BearingTo

    internal static double DegreesToRadians(double angle)
    {
        return angle * Math.PI / 180.0d;
    }

    internal static double RadianToDegree(double angle)
    {
        return angle * (180.0 / Math.PI);
    }
}