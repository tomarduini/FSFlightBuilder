namespace FSFlightBuilder.Entities
{
    internal static class CommUtil
    {

        public static string commTypeToStr(string type)
        {
            switch (type)
            {

                case "MC":
                    return "MULTICOM";

                case "UC":
                    return "UNICOM";

                case "G":
                    return "GROUND";

                case "T":
                    return "TOWER";

                case "C":
                    return "CLEARANCE";

                case "A":
                    return "APPROACH";

                case "D":
                    return "DEPARTURE";

                case "CTR":
                    return "CENTER";

                case "CPT":
                    return "PRETAXI";

                case "RCD":
                    return "REMOTE";

                default:
                    return type;
            }
        }
    }
}
