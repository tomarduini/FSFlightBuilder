namespace FSFlightBuilder.Data.Models;

public enum VorFlags
{
    // bit 0: if 0 then DME only, otherwise 1 for ILS
    // bit 2: backcourse (0 = false, 1 = true)
    // bit 3: glideslope present
    // bit 4: DME present
    // bit 5: NAV true

    FLAGS_DME_ONLY = 1 << 0,
    FLAGS_BC = 1 << 1,
    FLAGS_GS = 1 << 2,
    FLAGS_DME = 1 << 3,
    FLAGS_NAV = 1 << 4
}

//class Dme;

/*
 * VHF omnidirectional range - VOR/DME/VORDME record
 */
public class Vor : NavBase
{
    private nav.IlsVorType type;
    //private bool dmeOnly;
    private Dme dme = null;

    public Vor(NavDatabaseOptions options, BinaryStream bs) : base(options, bs)
    {
        type = (nav.IlsVorType)bs.readUByte();
        int flags = bs.readUByte();

        //dmeOnly = (flags & FLAGS_DME_ONLY) == 0;
        // TODO compare flags with record presence
        // hasDme = (flags & FLAGS_DME) == FLAGS_DME;
        // hasNav = (flags & FLAGS_NAV) == FLAGS_NAV;

        position = new BglPosition(bs, true, 1000.0f);
        frequency = bs.readInt() / 1000;
        range = bs.readFloat();
        magVar = GlobalMembersConverter.adjustMagvar(bs.readFloat(), true);

        ident = GlobalMembersConverter.intToIcao(bs.readUInt());

        uint regionFlags = bs.readUInt();
        region = GlobalMembersConverter.intToIcao(regionFlags & 0x7ff, true);

        // TODO report wiki error ap ident is never set
        airportIdent = GlobalMembersConverter.intToIcao((regionFlags >> 11) & 0x1fffff, true);

        while (bs.tellg() < startOffset + size)
        {
            Record r = new Record(options, bs);
            //rec.IlsVorRecordType t = r.getId<rec.IlsVorRecordType>();
            rec.IlsVorRecordType t = (rec.IlsVorRecordType)r.getId();
            if (checkSubRecord(r))
            {
                return;
            }

            switch (t)
            {
                case rec.IlsVorRecordType.ILS_VOR_NAME:
                    //name = bs.readString(r.getSize() - Record.SIZE);
                    name = bs.readString(r.getSize() - 6, Encoding.Default);
                    //name = bs.readString(r.getSize() - r.SIZE);
                    break;
                case rec.IlsVorRecordType.DME:
                    r.seekToStart();
                    dme = new Dme(options, bs);
                    break;
                case rec.IlsVorRecordType.LOCALIZER:
                case rec.IlsVorRecordType.GLIDESLOPE:
                    break;
                    //      default:
                    //        qWarning().nospace().noquote() << "Unexpected record type in VOR record 0x" << hex << t << dec <<
                    //        " for ident " << ident;
            }
            r.seekToEnd();
        }
    }

    public override void Dispose()
    {
        if (dme != null)
        {
            dme.Dispose();
        }
        base.Dispose();
    }

    /*
     * @return get the DME record for this VOR if available - otherwise null
     */
    public atools.fs.bgl.Dme getDme()
    {
        return dme;
    }

    /*
     * @return true if only DME
     */
    //public bool isDmeOnly()
    //{
    //    return dmeOnly;
    //}

    /*
     * @return VOR type which also indicates the range
     */
    public atools.fs.bgl.nav.IlsVorType getType()
    {
        return type;
    }

    //   private static QDebug operator << (QDebug @out, Vor record)
    //   {
    // 	QDebugStateSaver saver = new QDebugStateSaver(@out);

    // 	@out.nospace().noquote() << (NavBase)record << " Vor[" << "type " << IlsVor.ilsVorTypeToStr(record.type) << ", dmeOnly " << record.dmeOnly;
    // 	if (record.dme != null)
    // 	{
    // 	  @out << ", " << record.dme;
    // 	}
    // 	@out << "]";
    // 	return new QDebug(@out);
    //   }

}

