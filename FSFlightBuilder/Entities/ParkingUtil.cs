namespace FSFlightBuilder.Entities
{
    internal static class ParkingUtil
    {

        public static string parkingTypeToStr(string type)
        {
            switch (type)
            {
                case "UNKOWN":
                    return "UNKNOWN";

                case "RGA":
                    return "Ramp GA";

                case "RGAS":
                    return "Ramp GA Small";

                case "RGAM":
                    return "Ramp GA Medium";

                case "RGAL":
                    return "Ramp GA Large";

                case "RC":
                    return "Ramp Cargo";

                case "RMC":
                    return "Ramp Military Cargo";

                case "RMCB":
                    return "Ramp Military Combat";

                case "GS":
                    return "Gate Small";

                case "GM":
                    return "Gate Medium";

                case "GH":
                    return "Gate Heavy";

                case "DGA":
                    return "GA Dock";

                case "FUEL":
                    return "FUEL";

                case "V":
                    return "Vehicles";

                case "RE":
                    return "Ramp GA Extra";

                case "GE":
                    return "Gate Extra";
            }
            //qWarning().nospace().noquote() << "Invalid parking type " << type;
            return "INVALID";
        }
        public static string parkingNameToStr(string abbr)
        {
            switch (abbr)
            {
                case "NONE":
                    return "NO PARKING";

                case "P":
                    return "Parking";

                case "NP":
                    return "North Parking";

                case "NEP":
                    return "North-East Parking";

                case "EP":
                    return "East Parking";

                case "SEP":
                    return "South-East Parking";

                case "SP":
                    return "South Parking";

                case "SWP":
                    return "South-West Parking";

                case "WP":
                    return "West Parking";

                case "NWP":
                    return "North-West Parking";

                case "G":
                    return "Gate";

                case "D":
                    return "Dock";

                case "GA":
                    return "Gate A";

                case "GB":
                    return "Gate B";

                case "GC":
                    return "Gate C";

                case "GD":
                    return "Gate D";

                case "GE":
                    return "Gate E";

                case "GF":
                    return "Gate F";

                case "GG":
                    return "Gate G";

                case "GH":
                    return "Gate H";

                case "GI":
                    return "Gate I";

                case "GJ":
                    return "Gate J";

                case "GK":
                    return "Gate K";

                case "GL":
                    return "Gate L";

                case "GM":
                    return "Gate M";

                case "GN":
                    return "Gate N";

                case "GO":
                    return "Gate O";

                case "GP":
                    return "Gate P";

                case "GQ":
                    return "Gate Q";

                case "GR":
                    return "Gate R";

                case "GS":
                    return "Gate S";

                case "GT":
                    return "Gate T";

                case "GU":
                    return "Gate U";

                case "GV":
                    return "Gate V";

                case "GW":
                    return "Gate W";

                case "GX":
                    return "Gate X";

                case "GY":
                    return "Gate Y";

                case "GZ":
                    return "Gate Z";

            }
            //qWarning().nospace().noquote() << "Invalid parking name " << type;
            return "INVALID";
        }

        public static string parkingNameToStrRev(string name)
        {
            switch (name)
            {
                case "NO PARKING":
                    return "NONE";

                case "Parking":
                    return "P";

                case "North Parking":
                    return "NP";

                case "North-East Parking":
                    return "NEP";

                case "East Parking":
                    return "EP";

                case "South-East Parking":
                    return "SEP";

                case "South Parking":
                    return "SP";

                case "South-West Parking":
                    return "SWP";

                case "West Parking":
                    return "WP";

                case "North-West Parking":
                    return "NWP";

                case "Gate":
                    return "G";

                case "Dock":
                    return "D";

                case "Gate A":
                    return "GA";

                case "Gate B":
                    return "GB";

                case "Gate C":
                    return "GC";

                case "Gate D":
                    return "GD";

                case "Gate E":
                    return "GE";

                case "Gate F":
                    return "GF";

                case "Gate G":
                    return "GG";

                case "Gate H":
                    return "GH";

                case "Gate I":
                    return "GI";

                case "Gate J":
                    return "GJ";

                case "Gate K":
                    return "GK";

                case "Gate L":
                    return "GL";

                case "Gate M":
                    return "GM";

                case "Gate N":
                    return "GN";

                case "Gate O":
                    return "GO";

                case "Gate P":
                    return "GP";

                case "Gate Q":
                    return "GQ";

                case "Gate R":
                    return "GR";

                case "Gate S":
                    return "GS";

                case "Gate T":
                    return "GT";

                case "Gate U":
                    return "GU";

                case "Gate V":
                    return "GV";

                case "Gate W":
                    return "GW";

                case "Gate X":
                    return "GX";

                case "Gate Y":
                    return "GY";

                case "Gate Z":
                    return "GZ";

            }
            //qWarning().nospace().noquote() << "Invalid parking name " << type;
            return "INVALID";
        }
    }
}
