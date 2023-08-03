using System;

namespace FSFlightBuilder.Components
{
    /// <summary>
    /// Safe Convertsion utility
    /// </summary>
    public static class AWDConvert
    {
        public static string ToString(object input)
        {
            return input?.ToString() ?? string.Empty;
        }

        public static decimal ToDecimal(object input)
        {
            decimal returnValue;
            if (input != null && decimal.TryParse(input.ToString(), out returnValue))
                return returnValue >= 0 ? returnValue : 0;
            return 0;
        }

        public static int ToInt32(object input)
        {
            if (input != null)
            {
                int returnValue;
                if (int.TryParse(input.ToString(), out returnValue))
                {
                    return returnValue;
                }
            }
            return 0;
        }

        public static double ToDouble(object input)
        {
            if (input != null)
            {
                double returnValue;
                if (double.TryParse(input.ToString(), out returnValue))
                {
                    return returnValue;
                }
            }
            return 0;
        }

        public static bool ToBoolean(object input)
        {
            if (input != null)
            {
                bool returnValue;
                if (bool.TryParse(input.ToString(), out returnValue))
                {
                    return returnValue;
                }
            }
            return false;
        }

        public static DateTime ToDateTime(object input)
        {
            if (input != null)
            {
                DateTime returnValue;
                if (DateTime.TryParse(input.ToString(), out returnValue))
                {
                    return returnValue;
                }
            }
            return DateTime.MinValue;
        }

    }
}
