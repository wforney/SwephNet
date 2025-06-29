using Sweph.Net.Services;
using System.Collections.Immutable;

namespace Sweph.Net.Chronology;

/// <summary>
/// Holds the Delta T values for Julian Day calculations.
/// </summary>
/// <param name="fileService">
/// The file service used to read Delta T values from an external file.
/// </param>
public class JulianDayDeltaT(IFileService fileService)
{
    private const int EndDT2 = 1600;

    /// <summary>
    /// The start year for the Delta T table.
    /// </summary>
    private const int StartDT = 1620;

    private const int StartDT2 = -1000;
    private const int StepDT2 = 100;

    /// <summary> Table for -1000 through 1600, from Morrison &amp; Stephenson (2004). </summary>
    private static readonly ImmutableArray<short> TableDT2 =
        [
        // -1000
        25400,
        // -900
        23700,
        // -800
        22000,
        // -700
        21000,
        // -600
        19040,
        // -500
        17190,
        // -400
        15530,
        // -300
        14080,
        // -200
        12790,
        // -100
        11640,
        // 0
        10580,
        // 100
        9600,
        // 200
        8640,
        // 300
        7680,
        // 400
        6700,
        // 500
        5710,
        // 600
        4740,
        // 700
        3810,
        // 800
        2960,
        // 900
        2200,
        // 1000
        1570,
        // 1100
        1090,
        // 1200
        740,
        // 1300
        490,
        // 1400
        320,
        // 1500
        200,
        // 1600
        120,
        ];

    /// <summary>
    /// The Delta T table from 1620.
    /// </summary>
    private static double[] s_tableDT =
        [
        /* 1620.0 thru 1659.0 */
        124.00, 119.00, 115.00, 110.00, 106.00, 102.00, 98.00, 95.00, 91.00, 88.00,
        85.00, 82.00, 79.00, 77.00, 74.00, 72.00, 70.00, 67.00, 65.00, 63.00,
        62.00, 60.00, 58.00, 57.00, 55.00, 54.00, 53.00, 51.00, 50.00, 49.00,
        48.00, 47.00, 46.00, 45.00, 44.00, 43.00, 42.00, 41.00, 40.00, 38.00,
        /* 1660.0 thru 1699.0 */
        37.00, 36.00, 35.00, 34.00, 33.00, 32.00, 31.00, 30.00, 28.00, 27.00,
        26.00, 25.00, 24.00, 23.00, 22.00, 21.00, 20.00, 19.00, 18.00, 17.00,
        16.00, 15.00, 14.00, 14.00, 13.00, 12.00, 12.00, 11.00, 11.00, 10.00,
        10.00, 10.00, 9.00, 9.00, 9.00, 9.00, 9.00, 9.00, 9.00, 9.00,
        /* 1700.0 thru 1739.0 */
        9.00, 9.00, 9.00, 9.00, 9.00, 9.00, 9.00, 9.00, 10.00, 10.00,
        10.00, 10.00, 10.00, 10.00, 10.00, 10.00, 10.00, 11.00, 11.00, 11.00,
        11.00, 11.00, 11.00, 11.00, 11.00, 11.00, 11.00, 11.00, 11.00, 11.00,
        11.00, 11.00, 11.00, 11.00, 12.00, 12.00, 12.00, 12.00, 12.00, 12.00,
        /* 1740.0 thru 1779.0 */
        12.00, 12.00, 12.00, 12.00, 13.00, 13.00, 13.00, 13.00, 13.00, 13.00,
        13.00, 14.00, 14.00, 14.00, 14.00, 14.00, 14.00, 14.00, 15.00, 15.00,
        15.00, 15.00, 15.00, 15.00, 15.00, 16.00, 16.00, 16.00, 16.00, 16.00,
        16.00, 16.00, 16.00, 16.00, 16.00, 17.00, 17.00, 17.00, 17.00, 17.00,
        /* 1780.0 thru 1799.0 */
        17.00, 17.00, 17.00, 17.00, 17.00, 17.00, 17.00, 17.00, 17.00, 17.00,
        17.00, 17.00, 16.00, 16.00, 16.00, 16.00, 15.00, 15.00, 14.00, 14.00,
        /* 1800.0 thru 1819.0 */
        13.70, 13.40, 13.10, 12.90, 12.70, 12.60, 12.50, 12.50, 12.50, 12.50,
        12.50, 12.50, 12.50, 12.50, 12.50, 12.50, 12.50, 12.40, 12.30, 12.20,
        /* 1820.0 thru 1859.0 */
        12.00, 11.70, 11.40, 11.10, 10.60, 10.20, 9.60, 9.10, 8.60, 8.00,
        7.50, 7.00, 6.60, 6.30, 6.00, 5.80, 5.70, 5.60, 5.60, 5.60,
        5.70, 5.80, 5.90, 6.10, 6.20, 6.30, 6.50, 6.60, 6.80, 6.90,
        7.10, 7.20, 7.30, 7.40, 7.50, 7.60, 7.70, 7.70, 7.80, 7.80,
        /* 1860.0 thru 1899.0 */
        7.88, 7.82, 7.54, 6.97, 6.40, 6.02, 5.41, 4.10, 2.92, 1.82,
        1.61, .10, -1.02, -1.28, -2.69, -3.24, -3.64, -4.54, -4.71, -5.11,
        -5.40, -5.42, -5.20, -5.46, -5.46, -5.79, -5.63, -5.64, -5.80, -5.66,
        -5.87, -6.01, -6.19, -6.64, -6.44, -6.47, -6.09, -5.76, -4.66, -3.74,
        /* 1900.0 thru 1939.0 */
        -2.72, -1.54, -.02, 1.24, 2.64, 3.86, 5.37, 6.14, 7.75, 9.13,
        10.46, 11.53, 13.36, 14.65, 16.01, 17.20, 18.24, 19.06, 20.25, 20.95,
        21.16, 22.25, 22.41, 23.03, 23.49, 23.62, 23.86, 24.49, 24.34, 24.08,
        24.02, 24.00, 23.87, 23.95, 23.86, 23.93, 23.73, 23.92, 23.96, 24.02,
        /* 1940.0 thru 1979.0 */
        24.33, 24.83, 25.30, 25.70, 26.24, 26.77, 27.28, 27.78, 28.25, 28.71,
        29.15, 29.57, 29.97, 30.36, 30.72, 31.07, 31.35, 31.68, 32.18, 32.68,
        33.15, 33.59, 34.00, 34.47, 35.03, 35.73, 36.54, 37.43, 38.29, 39.20,
        40.18, 41.17, 42.23, 43.37, 44.49, 45.48, 46.46, 47.52, 48.53, 49.59,
        /* 1980.0 thru 1999.0 */
        50.54, 51.38, 52.17, 52.96, 53.79, 54.34, 54.87, 55.32, 55.82, 56.30,
        56.86, 57.57, 58.31, 59.12, 59.98, 60.78, 61.63, 62.30, 62.97, 63.47,
        /* 2000.0 thru 2009.0 */
        63.83, 64.09, 64.30, 64.47, 64.57, 64.69, 64.85, 65.15, 65.46, 65.78,
        /* 2010.0 thru 2013.0 */
        66.07, 66.32, 66.60, 66.907,
        /* Extrapolated values, 2014 - 2017 */
        67.267,67.90, 68.40, 69.00, 69.50, 70.00,
        ];

    private bool _initialized;

    /// <summary>
    /// Gets the current tidal acceleration.
    /// </summary>
    public double TidalAcceleration { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the DeltaT calculation should use the Espenak Meeus calculation.
    /// </summary>
    public bool UseEspenakMeeus2006 { get; set; }

    /// <summary>
    /// <para>
    /// Astronomical Almanac table is corrected by adding the expression
    /// -0.000091 (ndot + 26)(year-1955)^2 seconds to entries prior to 1955 (AA page K8), where ndot
    /// is the secular tidal term in the mean motion of the Moon.
    /// </para>
    /// <para>
    /// Entries after 1955 are referred to atomic time standards and are not affected by errors in
    /// Lunar or planetary theory.
    /// </para>
    /// </summary>
    public double AdjustForTidacc(double ans, double y)
    {
        if (y < 1955.0)
        {
            double b = y - 1955.0;
            ans += -0.000091 * (TidalAcceleration + 26.0) * b * b;
        }

        return ans;
    }

    /// <summary>
    /// Calculates the Delta T value for a given Julian Day.
    /// </summary>
    /// <param name="jd">Julian Day in UT</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The Delta T value in days, which is the difference between</returns>
    /// <remarks>(ET - UT) in days</remarks>
    public async Task<double> DeltaTAsync(double jd, CancellationToken cancellationToken = default)
    {
        double asY = 2000.0 + (jd - JulianDay.J2000) / 365.25;
        double asYGreg = 2000.0 + (jd - JulianDay.J2000) / 365.2425;

        // Before 1633 AD and using UseEspenakMeeus2006 Polynomials by Espenak & Meeus 2006, derived
        // from Stephenson & Morrison 2004. Note, Espenak & Meeus use their formulae only from 2000
        // BC on. However, they use the long-term formula of Morrison & Stephenson, which can be
        // used even for the remoter past.
        if (UseEspenakMeeus2006 && jd < 2317746.13090277789)
        {
            return DeltatEspenakMeeus1620(jd);
        }

        // If the macro ESPENAK_MEEUS_2006 is FALSE: Before 1620, we follow Stephenson & Morrsion
        // 2004. For the tabulated values 1000 BC through 1600 AD, we use linear interpolation.
        if (asY < StartDT)
        {
            if (asY < EndDT2)
            {
                return DeltatMorrisonStephenson1600(jd);
            }
            else
            {
                // between 1600 and 1620: linear interpolation between end of table dt2 and start of
                // table dt
                int b = StartDT - EndDT2;
                int iy = (EndDT2 - StartDT2) / StepDT2;
                double dd = (asY - EndDT2) / b;
                double ans = TableDT2[iy] + dd * (s_tableDT[0] - TableDT2[iy]);
                ans = AdjustForTidacc(ans, asYGreg);
                return ans / 86400.0;
            }
        }
        else
        {
            // 1620 - today + a few years (tabend): Besselian interpolation from tabulated values in
            // table dt. See AA page K11.
            return await DeltaTAAAsync(jd, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Adjusts the Delta T value for tidal acceleration.
    /// </summary>
    /// <param name="tjd">The Julian Day to adjust the Delta T value for tidal acceleration.</param>
    /// <returns>The adjusted Delta T value in seconds.</returns>
    protected static double DeltatLongtermMorrisonStephenson(double tjd)
    {
        double ygreg = 2000.0 + (tjd - JulianDay.J2000) / 365.2425;
        double u = (ygreg - 1820) / 100.0;
        return -20 + 32 * u * u;
    }

    /// <summary>
    /// Adjusts the Delta T value for tidal acceleration.
    /// </summary>
    /// <param name="tjd">The Julian Day to adjust the Delta T value for tidal acceleration.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The adjusted Delta T value in seconds.</returns>
    protected async Task<double> DeltaTAAAsync(double tjd, CancellationToken cancellationToken = default)
    {
        double ans2, ans3;
        double p, b, b2, y, dd;
        double[] d = new double[6];
        int i, iy, k;
        int tabsiz = await InitializeAsync(cancellationToken).ConfigureAwait(false);
        int tabend = StartDT + tabsiz - 1;

        // Y = 2000.0 + (tjd - J2000)/365.25;
        y = 2000.0 + (tjd - JulianDay.J2000) / 365.2425;
        double ans;
        if (y <= tabend)
        {
            // Index into the table.
            p = Math.Floor(y);
            iy = (int)(p - StartDT);

            // Zeroth order estimate is value at start of year
            ans = s_tableDT[iy];
            k = iy + 1;
            if (k >= tabsiz)
            {
                goto done; // No data, can't go on
            }

            // The fraction of tabulation interval
            p = y - p;

            // First order interpolated value
            ans += p * (s_tableDT[k] - s_tableDT[iy]);
            if (iy - 1 < 0 || iy + 2 >= tabsiz)
            {
                goto done; // can't do second differences
            }

            // Make table of first differences
            k = iy - 2;
            for (i = 0; i < 5; i++)
            {
                d[i] = k < 0 || k + 1 >= tabsiz ? 0 : s_tableDT[k + 1] - s_tableDT[k];

                k += 1;
            }

            // Compute second differences
            for (i = 0; i < 4; i++)
            {
                d[i] = d[i + 1] - d[i];
            }

            b = 0.25 * p * (p - 1.0);
            ans += b * (d[1] + d[2]);
            if (iy + 2 >= tabsiz)
            {
                goto done;
            }

            // Compute third differences
            for (i = 0; i < 3; i++)
            {
                d[i] = d[i + 1] - d[i];
            }

            b = 2.0 * b / 3.0;
            ans += (p - 0.5) * b * d[1];
            if (iy - 2 < 0 || iy + 3 > tabsiz)
            {
                goto done;
            }

            // Compute fourth differences
            for (i = 0; i < 2; i++)
            {
                d[i] = d[i + 1] - d[i];
            }

            b = 0.125 * b * (p + 1.0) * (p - 2.0);
            ans += b * (d[0] + d[1]);
        done:
            ans = AdjustForTidacc(ans, y);
            return ans / 86400.0;
        }

        // today - : Formula Stephenson (1997; p. 507), with modification to avoid jump at end of AA
        // table, similar to what Meeus 1998 had suggested. Slow transition within 100 years.
        b = 0.01 * (y - 1820);
        ans = -20 + 31 * b * b;

        // slow transition from tabulated values to Stephenson formula:
        if (y <= tabend + 100)
        {
            b2 = 0.01 * (tabend - 1820);
            ans2 = -20 + 31 * b2 * b2;
            ans3 = s_tableDT[tabsiz - 1];
            dd = ans2 - ans3;
            ans += dd * (y - (tabend + 100)) * 0.01;
        }

        return ans / 86400.0;
    }

    /// <summary>
    /// Adjusts the Delta T value for tidal acceleration.
    /// </summary>
    /// <param name="tjd">The Julian Day to adjust the Delta T value for tidal acceleration.</param>
    /// <returns>The adjusted Delta T value in seconds.</returns>
    protected double DeltatEspenakMeeus1620(double tjd)
    {
        double ans = 0;
        double ygreg;
        double u;

        //* double Y = 2000.0 + (tjd - J2000)/365.25;
        ygreg = 2000.0 + (tjd - JulianDay.J2000) / 365.2425;
        if (ygreg < -500)
        {
            ans = DeltatLongtermMorrisonStephenson(tjd);
        }
        else if (ygreg < 500)
        {
            u = ygreg / 100.0;
            ans = (((((0.0090316521 * u + 0.022174192) * u - 0.1798452) * u - 5.952053) * u + 33.78311) * u - 1014.41) * u + 10583.6;
        }
        else if (ygreg < 1600)
        {
            u = (ygreg - 1000) / 100.0;
            ans = (((((0.0083572073 * u - 0.005050998) * u - 0.8503463) * u + 0.319781) * u + 71.23472) * u - 556.01) * u + 1574.2;
        }
        // TODO: Verify dead code
        else if (ygreg < 1700)
        {
            u = ygreg - 1600;
            ans = 120 - 0.9808 * u - 0.01532 * u * u + u * u * u / 7129.0;
        }
        else if (ygreg < 1800)
        {
            u = ygreg - 1700;
            ans = (((-u / 1174000.0 + 0.00013336) * u - 0.0059285) * u + 0.1603) * u + 8.83;
        }
        else if (ygreg < 1860)
        {
            u = ygreg - 1800;
            ans = ((((((0.000000000875 * u - 0.0000001699) * u + 0.0000121272) * u - 0.00037436) * u + 0.0041116) * u + 0.0068612) * u - 0.332447) * u + 13.72;
        }
        else if (ygreg < 1900)
        {
            u = ygreg - 1860;
            ans = ((((u / 233174.0 - 0.0004473624) * u + 0.01680668) * u - 0.251754) * u + 0.5737) * u + 7.62;
        }
        else if (ygreg < 1920)
        {
            u = ygreg - 1900;
            ans = (((-0.000197 * u + 0.0061966) * u - 0.0598939) * u + 1.494119) * u - 2.79;
        }
        else if (ygreg < 1941)
        {
            u = ygreg - 1920;
            ans = 21.20 + 0.84493 * u - 0.076100 * u * u + 0.0020936 * u * u * u;
        }
        else if (ygreg < 1961)
        {
            u = ygreg - 1950;
            ans = 29.07 + 0.407 * u - u * u / 233.0 + u * u * u / 2547.0;
        }
        else if (ygreg < 1986)
        {
            u = ygreg - 1975;
            ans = 45.45 + 1.067 * u - u * u / 260.0 - u * u * u / 718.0;
        }
        else if (ygreg < 2005)
        {
            u = ygreg - 2000;
            ans = ((((0.00002373599 * u + 0.000651814) * u + 0.0017275) * u - 0.060374) * u + 0.3345) * u + 63.86;
        }

        ans = AdjustForTidacc(ans, ygreg);
        ans /= 86400.0;
        return ans;
    }

    /// <summary>
    /// Adjusts the Delta T value for tidal acceleration.
    /// </summary>
    /// <param name="tjd">The Julian Day to adjust the Delta T value for tidal acceleration.</param>
    /// <returns>The adjusted Delta T value in seconds.</returns>
    protected double DeltatMorrisonStephenson1600(double tjd)
    {
        double ans = 0, ans2, ans3;
        double p, b, dd;
        double tjd0;
        int iy;
        double y = 2000.0 + (tjd - JulianDay.J2000) / 365.2425;

        // before -1000: formula by Stephenson&Morrison (2004; p. 335) but adjusted to fit the
        // starting point of table dt2.
        if (y < StartDT2)
        {
            //B = (Y - LTERM_EQUATION_YSTART) * 0.01;
            //ans = -20 + LTERM_EQUATION_COEFF * B * B;
            ans = DeltatLongtermMorrisonStephenson(tjd);
            ans = AdjustForTidacc(ans, y);

            // transition from formula to table over 100 years
            if (y >= StartDT2 - 100)
            {
                //* starting value of table dt2:
                ans2 = AdjustForTidacc(TableDT2[0], StartDT2);

                // value of formula at epoch TAB2_START B = (TAB2_START - LTERM_EQUATION_YSTART) *
                // 0.01; ans3 = -20 + LTERM_EQUATION_COEFF * B * B;
                tjd0 = (StartDT2 - 2000) * 365.2425 + JulianDay.J2000;
                ans3 = DeltatLongtermMorrisonStephenson(tjd0);
                ans3 = AdjustForTidacc(ans3, y);
                dd = ans3 - ans2;
                b = (y - (StartDT2 - 100)) * 0.01;

                // fit to starting point of table dt2.
                ans -= dd * b;
            }
        }

        // between -1000 and 1600: linear interpolation between values of table dt2
        // (Stephenson&Morrison 2004)
        if (y is >= StartDT2 and < EndDT2)
        {
            double yjul = 2000 + (tjd - 2451557.5) / 365.25;
            p = Math.Floor(yjul);
            iy = (int)((p - StartDT2) / StepDT2);
            dd = (yjul - (StartDT2 + StepDT2 * iy)) / StepDT2;
            ans = TableDT2[iy] + (TableDT2[iy + 1] - TableDT2[iy]) * dd;

            // correction for tidal acceleration used by our ephemeris
            ans = AdjustForTidacc(ans, y);
        }

        ans /= 86400.0;
        return ans;
    }

    /// <summary>
    /// Reads the delta t values from an external file.
    /// <para>record structure: year(whitespace)delta_t in 0.01 sec.</para>
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The size of the table.</returns>
    private async Task<int> InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_initialized)
        {
            return s_tableDT.Length;
        }

        TidalAcceleration = DeltaT.TidalDefault;

        DeltaT[] records = await fileService.GetDeltaTRecordsAsync(cancellationToken)
            .Where(r => r.Year is >= StartDT and < 2050) // We limit the table to 2050
            .OrderBy(r => r.Year)
            .ToArrayAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (records.Length != 0)
        {
            // Calculate the new table size
            int lastYear = records[^1].Year;
            int newSize = lastYear - StartDT + 1;

            // Resize the table
            if (newSize > s_tableDT.Length)
            {
                double[] dt = s_tableDT;
                s_tableDT = new double[newSize];
                Array.Copy(dt, 0, s_tableDT, 0, dt.Length);
            }

            // Update the table
            foreach (DeltaT rec in records)
            {
                int tabIndex = rec.Year - StartDT;
                s_tableDT[tabIndex] = rec.Value;
            }
        }

        _initialized = true;
        return s_tableDT.Length;
    }
}
