namespace FSFlightBuilder.Entities
{
    internal static class RunwayUtil
    {

        public static string surfaceTypeToStr(string type)
        {
            switch (type)
            {
                case "C":
                    return "CONCRETE";
                case "G":
                    return "GRASS";
                case "W":
                    return "WATER";
                case "A":
                    return "ASPHALT";
                case "CE":
                    return "CEMENT";
                case "CL":
                    return "CLAY";
                case "SN":
                    return "SNOW";
                case "I":
                    return "ICE";
                case "D":
                    return "DIRT";
                case "CR":
                    return "CORAL";
                case "GR":
                    return "GRAVEL";
                case "OT":
                    return "OIL_TREATED";
                case "SM":
                    return "STEEL_MATS";
                case "B":
                    return "BITUMINOUS";
                case "BR":
                    return "BRICK";
                case "M":
                    return "MACADAM";
                case "PL":
                    return "PLANKS";
                case "S":
                    return "SAND";
                case "SH":
                    return "SHALE";
                case "T":
                    return "TARMAC";
                case "TR":
                    return "TRANSPARENT";
                case "UNKNOWN":
                    return "UNDEFINED";
            }
            return "UNDEFINED";
        }

        public static string surfaceTypeToStrRev(string type)
        {
            switch (type)
            {
                case "CONCRETE":
                    return "C";
                case "GRASS":
                case "ERASE_GRASS":
                    return "G";
                case "WATER":
                    return "W";
                case "ASPHALT":
                    return "A";
                case "CEMENT":
                    return "CE";
                case "CLAY":
                    return "CL";
                case "SNOW":
                    return "SN";
                case "ICE":
                    return "I";
                case "DIRT":
                    return "D";
                case "CORAL":
                    return "CR";
                case "GRAVEL":
                    return "GR";
                case "PAINT":
                case "OIL_TREATED":
                    return "OT";
                case "STEEL_MATS":
                    return "SM";
                case "BITUMINOUS":
                    return "B";
                case "BRICK":
                    return "BR";
                case "MACADAM":
                    return "M";
                case "PLANKS":
                    return "PL";
                case "SAND":
                    return "S";
                case "SHALE":
                    return "SH";
                case "TARMAC":
                    return "T";
                case "TRANSPARENT":
                    return "TR";
                case "UNDEFINED":
                    return "UNKNOWN";
            }
            return "UNKNOWN";
        }
    }
}
