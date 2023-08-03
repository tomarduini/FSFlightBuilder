namespace FSFlightBuilder.Data.Models;
public enum NdbType
{
    COMPASS_POINT = 0,
    MH = 1,
    H = 2,
    HH = 3
}

/*
 * Nondirectional beacon. Top level record.
 */
public class Ndb : NavBase
{
    private NdbType type;

    public Ndb(NavDatabaseOptions options, BinaryStream bs) : base(options, bs)
    {
        type = (nav.NdbType)bs.readShort();
        frequency = bs.readInt() / 10;
        position = new BglPosition(bs, true, 1000.0f);
        range = bs.readFloat();
        magVar = GlobalMembersConverter.adjustMagvar(bs.readFloat(), true);
        ident = GlobalMembersConverter.intToIcao(bs.readUInt());

        uint regionFlags = bs.readUInt();
        region = GlobalMembersConverter.intToIcao(regionFlags & 0x7ff, true);
        airportIdent = GlobalMembersConverter.intToIcao((regionFlags >> 11) & 0x1fffff, true);

        // Read only name subrecord
        if (bs.tellg() < startOffset + size)
        {
            Record r = new Record(options, bs);
            uint id = r.getId();

            rec.NdbRecordType t = (rec.NdbRecordType)(id & 0x00ff);
            if (checkSubRecord(r))
            {
                return;
            }

            switch (t)
            {
                case rec.NdbRecordType.NDB_NAME:
                    //name = bs.readString(r.getSize() - Record.SIZE);
                    name = bs.readString(r.getSize() - r.SIZE, Encoding.Default);
                    break;
                default:
                    //qWarning().nospace().noquote() << "Unexpected record type in NDB record 0x" << hex << t << dec << " for ident " << ident;
                    break;
            }
            r.seekToEnd();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public atools.fs.bgl.nav.NdbType getType()
    {
        return type;
    }

    public static string ndbTypeToStr(NdbType type)
    {
        switch (type)
        {
            case nav.NdbType.COMPASS_POINT:
                return "CP";

            case nav.NdbType.MH:
                return "MH";

            case nav.NdbType.H:
                return "H";

            case nav.NdbType.HH:
                return "HH";
        }
        //qWarning().nospace().noquote() << "Invalid NDB type " << type;
        return "INVALID";
    }

    // private static QDebug operator << (QDebug @out, Ndb record)
    // {
    //QDebugStateSaver saver = new QDebugStateSaver(@out);

    //@out.nospace().noquote() << (NavBase)record << " Ndb[type " << Ndb.ndbTypeToStr(record.type) << "]";
    //return new QDebug(@out);
    // }

}
