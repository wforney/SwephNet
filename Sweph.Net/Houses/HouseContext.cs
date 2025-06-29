using Sweph.Net.Chronology;
using Sweph.Net.Geography;
using System.Globalization;

namespace Sweph.Net.Houses;

/// <summary>
/// Represents the context for house calculations, providing methods to retrieve house system names,
/// </summary>
public sealed class HouseContext
{
    /// <summary>
    /// Returns the name of an house system
    /// </summary>
    public static string GetHouseSystemName(HouseSystem hs)
    {
        return hs switch
        {
            HouseSystem.Koch => "Koch",
            HouseSystem.Porphyrius => "Porphyrius",
            HouseSystem.Regiomontanus => "Regiomontanus",
            HouseSystem.Campanus => "Campanus",
            HouseSystem.Equal => "Equal",
            HouseSystem.VehlowEqual => "Vehlow equal",
            HouseSystem.WholeSign => "Whole sign",
            HouseSystem.MeridianSystem => "Axial rotation system / Meridian system / Zariel",
            HouseSystem.Horizon => "Azimuthal / Horizontal system",
            HouseSystem.PolichPage => "Polich/Page (\"topocentric\" system)",
            HouseSystem.Alcabitus => "Alcabitus",
            HouseSystem.Morinus => "Morinus",
            HouseSystem.KrusinskiPisa => "Krusinski-Pisa",
            HouseSystem.GauquelinSector => "Gauquelin sector",
            HouseSystem.APC => "APC houses",
            _ => "Placidus",
        };
    }

    /// <summary>
    /// Convert an house system from a char
    /// </summary>
    public static HouseSystem HouseSystemFromChar(char c)
    {
        return char.ToUpper(c, CultureInfo.CurrentCulture) switch
        {
            'A' or 'E' => HouseSystem.Equal,
            'B' => HouseSystem.Alcabitus,
            'C' => HouseSystem.Campanus,
            'G' => HouseSystem.GauquelinSector,
            'H' => HouseSystem.Horizon,
            'M' => HouseSystem.Morinus,
            'O' => HouseSystem.Porphyrius,
            'R' => HouseSystem.Regiomontanus,
            'T' => HouseSystem.PolichPage,
            'U' => HouseSystem.KrusinskiPisa,
            'V' => HouseSystem.VehlowEqual,
            'W' => HouseSystem.WholeSign,
            'X' => HouseSystem.MeridianSystem,
            'Y' => HouseSystem.APC,
            _ => HouseSystem.Placidus,
        };
    }

    /// <summary>
    /// Convert a char to an house system
    /// </summary>
    public static char HouseSystemToChar(HouseSystem hs)
    {
        return hs switch
        {
            HouseSystem.Koch => 'K',
            HouseSystem.Porphyrius => 'O',
            HouseSystem.Regiomontanus => 'R',
            HouseSystem.Campanus => 'C',
            HouseSystem.Equal => 'E',
            HouseSystem.VehlowEqual => 'V',
            HouseSystem.WholeSign => 'W',
            HouseSystem.MeridianSystem => 'X',
            HouseSystem.Horizon => 'H',
            HouseSystem.PolichPage => 'T',
            HouseSystem.Alcabitus => 'B',
            HouseSystem.Morinus => 'M',
            HouseSystem.KrusinskiPisa => 'U',
            HouseSystem.GauquelinSector => 'G',
            HouseSystem.APC => 'Y',
            _ => 'P',
        };
    }

    /// <summary>
    /// Calculate houses positions
    /// </summary>
    /// <param name="day"></param>
    /// <param name="position"></param>
    /// <param name="hsys"></param>
    /// <returns></returns>
    public static HouseResult Houses(JulianDay day, GeoPosition position, HouseSystem hsys)
    {
        throw new NotImplementedException();
        // int i, retc = 0; double armc, eps; double[] nutlo = new double[2];

        //EphemerisTime jde = new(day);
        //double eps = Context.Epsiln(jde, 0) * Context.RadiansToDegrees;

        //            SE.SwephLib.swi_nutation(tjde, 0, nutlo);
        //            for (i = 0; i < 2; i++)
        //                nutlo[i] *= SwissEph.RADTODEG;
        //            armc = SE.swe_degnorm(SE.swe_sidtime0(tjd_ut, eps + nutlo[1], nutlo[0]) * 15 + geolon);
        //#if TRACE
        //            //swi_open_trace(NULL);
        //            //if (swi_trace_count <= TRACE_COUNT_MAX) {
        //            //    if (swi_fp_trace_c != NULL) {
        //            //        fputs("\n/*SWE_HOUSES*/\n", swi_fp_trace_c);
        //            //        fprintf(swi_fp_trace_c, "#if 0\n");
        //            //        fprintf(swi_fp_trace_c, "  tjd = %.9f;", tjd_ut);
        //            //        fprintf(swi_fp_trace_c, " geolon = %.9f;", geolon);
        //            //        fprintf(swi_fp_trace_c, " geolat = %.9f;", geolat);
        //            //        fprintf(swi_fp_trace_c, " hsys = %d;\n", hsys);
        //            //        fprintf(swi_fp_trace_c, "  retc = swe_houses(tjd, geolat, geolon, hsys, cusp, ascmc);\n");
        //            //        fprintf(swi_fp_trace_c, "  /* swe_houses calls swe_houses_armc as follows: */\n");
        //            //        fprintf(swi_fp_trace_c, "#endif\n");
        //            //        fflush(swi_fp_trace_c);
        //            //    }
        //            //}
        //#endif
        //            retc = swe_houses_armc(armc, geolat, eps + nutlo[1], hsys, cusp, ascmc);
        //            return retc;
        //        }
    }
}
