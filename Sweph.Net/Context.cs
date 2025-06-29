using Microsoft.Extensions.Options;
using Sweph.Net.Chronology;
using Sweph.Net.Houses;
using Sweph.Net.Planets;

namespace Sweph.Net;

/// <summary>
/// Represents a context for accessing resources or services. Implements the <see cref="IDisposable"/>.
/// </summary>
/// <seealso cref="IDisposable"/>
public class Context(
    HouseContext houseContext,
    PlanetContext planetContext,
    IOptionsMonitor<SwephNetSettings> swephNetSettingsOptionsMonitor)
{
    /// <summary>
    /// Represents the conversion factor used to convert radians to arcseconds.
    /// </summary>
    /// <remarks>
    /// Multiply an angle in radians by this constant to obtain the equivalent angle in arcseconds.
    /// The value is derived from dividing (180 * 3600) by π.
    /// </remarks>
    public const double RadiansToArcSeconds = (180.0 * 3600.0) / Math.PI;

    /// <summary>
    /// Represents the conversion factor used to convert arcseconds to radians.
    /// </summary>
    /// <remarks>
    /// Multiply an angle in arcseconds by this constant to obtain the equivalent angle in radians.
    /// The value is derived from dividing π by (180 * 3600).
    /// </remarks>
    public const double ArcSecondsToRadians = Math.PI / (180.0 * 3600.0);

    /// <summary>
    /// Represents the conversion factor used to convert degrees to radians.
    /// </summary>
    /// <remarks>
    /// Multiply an angle in degrees by this constant to obtain the equivalent angle in radians.
    /// The value is derived from dividing π by 180.
    /// </remarks>
    public const double DegreesToRadians = Math.PI / 180.0;

    /// <summary>
    /// Represents the conversion factor used to convert radians to degrees.
    /// </summary>
    /// <remarks>
    /// Multiply an angle in radians by this constant to obtain the equivalent angle in degrees.
    /// The value is derived from dividing 180 by π.
    /// </remarks>
    public const double RadiansToDegrees = 180.0 / Math.PI;

    /// <summary>
    /// Gets the <see cref="HouseContext"/> for accessing house calculations and data.
    /// </summary>
    public HouseContext Houses { get; } = houseContext;

    /// <summary>
    /// Gets the <see cref="PlanetContext"/> for accessing planetary data and calculations.
    /// </summary>
    public PlanetContext Planets { get; } = planetContext;

    /// <summary>
    /// Reduce x modulo 360 degrees
    /// </summary>
    public static double DegNorm(double x)
    {
        double y = x % 360.0;
        if (Math.Abs(y) < 1e-13)
        {
            y = 0; // Alois fix 11-dec-1999
        }

        if (y < 0.0)
        {
            y += 360.0;
        }

        return y;
    }

    #region Precession and ecliptic obliquity

    /// <summary>
    /// A double representing the mathematical constant π (pi) times 2.
    /// </summary>
    private const double D2PI = Math.PI * 2.0;

    /// <summary>
    /// Number of data points for dcor_eps_jpl
    /// </summary>
    private const int NDCOR_EPS_JPL = 51;

    /// <summary>
    /// Number of periodic terms and polynomial terms for pre_peps()
    /// </summary>
    private const int NPER_PEPS = 10;

    /// <summary>
    /// Number of polynomial terms for pre_peps()
    /// </summary>
    private const int NPOL_PEPS = 4;

    /// <summary>
    /// Julian day for dcor_eps_jpl
    /// </summary>
    private static readonly double[] s_Dcor_eps_jpl = [
                36.726, 36.627, 36.595, 36.578, 36.640, 36.659, 36.731, 36.765,
            36.662, 36.555, 36.335, 36.321, 36.354, 36.227, 36.289, 36.348, 36.257, 36.163,
            35.979, 35.896, 35.842, 35.825, 35.912, 35.950, 36.093, 36.191, 36.009, 35.943,
            35.875, 35.771, 35.788, 35.753, 35.822, 35.866, 35.771, 35.732, 35.543, 35.498,
            35.449, 35.409, 35.497, 35.556, 35.672, 35.760, 35.596, 35.565, 35.510, 35.394,
            35.385, 35.375, 35.415,
        ];

    /// <summary>
    /// for pre_peps(): periodics
    /// </summary>
    private static readonly double[][] s_Peper = [
          [+409.90, +396.15, +537.22, +402.90, +417.15, +288.92, +4043.00, +306.00, +277.00, +203.00],
          [-6908.287473, -3198.706291, +1453.674527, -857.748557, +1173.231614, -156.981465, +371.836550, -216.619040, +193.691479, +11.891524],
          [+753.872780, -247.805823, +379.471484, -53.880558, -90.109153, -353.600190, -63.115353, -28.248187, +17.703387, +38.911307],
          [-2845.175469, +449.844989, -1255.915323, +886.736783, +418.887514, +997.912441, -240.979710, +76.541307, -36.788069, -170.964086],
          [-1704.720302, -862.308358, +447.832178, -889.571909, +190.402846, -56.564991, -296.222622, -75.859952, +67.473503, +3.014055],
        ];

    /// <summary>
    /// for pre_peps(): polynomials
    /// </summary>
    private static readonly double[][] s_Pepol = [
          [+8134.017132, +84028.206305],
          [+5043.0520035, +0.3624445],
          [-0.00710733, -0.00004039],
          [+0.000000271, -0.000000110]
        ];

    /// <summary>
    /// return dpre, deps
    /// </summary>
    internal static Tuple<double, double> LdpPeps(double tjd)
    {
        int i;
        double w, a, s, c;
        int npol = NPOL_PEPS;
        int nper = NPER_PEPS;
        double t = (tjd - JulianDay.J2000) / 36525.0;
        double p = 0;
        double q = 0;

        // periodic terms
        for (i = 0; i < nper; i++)
        {
            w = D2PI * t;
            a = w / s_Peper[0][i];
            s = Math.Sin(a);
            c = Math.Cos(a);
            p += (c * s_Peper[1][i]) + (s * s_Peper[3][i]);
            q += (c * s_Peper[2][i]) + (s * s_Peper[4][i]);
        }

        // polynomial terms
        w = 1;
        for (i = 0; i < npol; i++)
        {
            p += s_Pepol[i][0] * w;
            q += s_Pepol[i][1] * w;
            w *= t;
        }

        // both to radians
        p *= ArcSecondsToRadians;
        q *= ArcSecondsToRadians;

        // return
        return new Tuple<double, double>(p, q);
    }

    /// <summary>
    /// Obliquity of the ecliptic at Julian date jd
    /// </summary>
    /// <remarks>
    /// IAU Coefficients are from: J. H. Lieske, T. Lederle, W. Fricke, and B. Morando, "Expressions
    /// for the Precession Quantities Based upon the IAU (1976) System of Astronomical Constants,"
    /// Astronomy and Astrophysics 58, 1-16 (1977).
    ///
    /// Before or after 200 years from J2000, the formula used is from: J. Laskar, "Secular terms of
    /// classical planetary theories using the results of general theory," Astronomy and
    /// Astrophysics 157, 59070 (1986).
    ///
    /// Bretagnon, P. et al.: 2003, "Expressions for Precession Consistent with the IAU 2000A
    /// Model". A&amp;A 400,785 B03 84381.4088 -46.836051*t -1667*10-7*t2 +199911*10-8*t3
    /// -523*10-9*t4 -248*10-10*t5 -3*10-11*t6 C03 84381.406 -46.836769*t -1831*10-7*t2
    /// +20034*10-7*t3 -576*10-9*t4 -434*10-10*t5
    ///
    /// See precess and page B18 of the Astronomical Almanac.
    /// </remarks>
    internal double Epsiln(double jd, JPL.JplHorizonMode horizons)
    {
        SwephNetSettings options = swephNetSettingsOptionsMonitor.CurrentValue;
        double T = (jd - 2451545.0) / 36525.0;
        double eps;
        if ((horizons & JPL.JplHorizonMode.JplHorizons) != 0 && options.IncludeCodeForDpsiDepsIAU1980)
        {
            eps = ((((((1.813e-3 * T) - 5.9e-4) * T) - 46.8150) * T) + 84381.448) * DegreesToRadians / 3600;
        }
        else if ((horizons & JPL.JplHorizonMode.JplApproximate) != 0 && !options.ApproximateHorizonsAstrodienst)
        {
            eps = ((((((1.813e-3 * T) - 5.9e-4) * T) - 46.8150) * T) + 84381.448) * DegreesToRadians / 3600;
        }
        else if (options.UsePrecessionIAU == PrecessionIAU.IAU_1976 && Math.Abs(T) <= JulianDay.PrecessionIAU_1976_Centuries)
        {
            eps = ((((((1.813e-3 * T) - 5.9e-4) * T) - 46.8150) * T) + 84381.448) * DegreesToRadians / 3600;
        }
        else if (options.UsePrecessionIAU == PrecessionIAU.IAU_2000 && Math.Abs(T) <= JulianDay.PrecessionIAU_2000_Centuries)
        {
            eps = ((((((1.813e-3 * T) - 5.9e-4) * T) - 46.84024) * T) + 84381.406) * DegreesToRadians / 3600;
        }
        else if (options.UsePrecessionIAU == PrecessionIAU.IAU_2006 && Math.Abs(T) <= JulianDay.PrecessionIAU_2006_Centuries)
        {
            eps = ((((((((((-4.34e-8 * T) - 5.76e-7) * T) + 2.0034e-3) * T) - 1.831e-4) * T) - 46.836769) * T) + 84381.406) * DegreesToRadians / 3600.0;
        }
        else if (options.UsePrecessionCoefficient == PrecessionCoefficients.Bretagnon2003)
        {
            eps = ((((((((((((-3e-11 * T) - 2.48e-8) * T) - 5.23e-7) * T) + 1.99911e-3) * T) - 1.667e-4) * T) - 46.836051) * T) + 84381.40880) * DegreesToRadians / 3600.0;
        }
        else if (options.UsePrecessionCoefficient == PrecessionCoefficients.Simon1994)
        {
            eps = ((((((((((2.5e-8 * T) - 5.1e-7) * T) + 1.9989e-3) * T) - 1.52e-4) * T) - 46.80927) * T) + 84381.412) * DegreesToRadians / 3600.0;
        }
        else if (options.UsePrecessionCoefficient == PrecessionCoefficients.Williams1994)
        {
            eps = ((((((((-1.0e-6 * T) + 2.0e-3) * T) - 1.74e-4) * T) - 46.833960) * T) + 84381.409) * DegreesToRadians / 3600.0;/* */
        }
        else if (options.UsePrecessionCoefficient == PrecessionCoefficients.Laskar1986)
        {
            T /= 10.0;
            eps = (((((((((((((((((((2.45e-10 * T) + 5.79e-9) * T) + 2.787e-7) * T)
            + 7.12e-7) * T) - 3.905e-5) * T) - 2.4967e-3) * T)
            - 5.138e-3) * T) + 1.99925) * T) - 0.0155) * T) - 468.093) * T)
            + 84381.448;
            eps *= DegreesToRadians / 3600.0;
        }
        else
        {
            // Vondrak2011
            Tuple<double, double> tup = LdpPeps(jd);
            eps = tup.Item2;
            if ((horizons & JPL.JplHorizonMode.JplApproximate) != 0 && !options.ApproximateHorizonsAstrodienst)
            {
                double tofs = (jd - DCOR_EPS_JPL_TJD0) / 365.25;
                double dofs;
                if (tofs < 0)
                {
                    dofs = s_Dcor_eps_jpl[0];
                }
                else if (tofs >= NDCOR_EPS_JPL - 1)
                {
                    dofs = s_Dcor_eps_jpl[NDCOR_EPS_JPL - 1];
                }
                else
                {
                    double t0 = (int)tofs;
                    double t1 = t0 + 1;
                    //dofs = dcor_eps_jpl[(int)t0];
                    dofs = ((tofs - t0) * (s_Dcor_eps_jpl[(int)t0] - s_Dcor_eps_jpl[(int)t1])) + s_Dcor_eps_jpl[(int)t0];
                }

                dofs /= 1000.0 * 3600.0;
                eps += dofs * DegreesToRadians;
            }
        }
        return eps;
    }

    #endregion Precession and ecliptic obliquity

    #region Nutation in longitude and obliquity

    //internal double[] Nutation(double J, JPL.JplHorizonMode jplMode)
    //{
    //    var options = swephNetSettingsOptionsMonitor.CurrentValue;
    //    double[] result;
    //    if ((jplMode & JPL.JplHorizonMode.JplHorizons) != 0 && options.IncludeCodeForDpsiDepsIAU1980)
    //    {
    //        result = NutationIAU1980(J);
    //    }
    //    else if (NUT_IAU_1980)
    //    {
    //        result = NutationIAU1980(J);
    //    }
    //    else if (NUT_IAU_2000A || NUT_IAU_2000B)
    //    {
    //        result = NutationIAU2000ab(J);
    //        /*if ((iflag & SEFLG_JPLHOR_APPROX) && FRAME_BIAS_APPROX_HORIZONS) {*/
    //        if ((jplMode & JPL.JplHorizonMode.JplApproximate) != 0 && !options.ApproximateHorizonsAstrodienst)
    //        {
    //            result[0] += -41.7750 / 3600.0 / 1000.0 * DegreesToRadians;
    //            result[1] += -6.8192 / 3600.0 / 1000.0 * DegreesToRadians;
    //        }
    //    }
    //    if (options.IncludeCodeForDpsiDepsIAU1980)
    //    {
    //        if ((jplMode & JPL.JplHorizonMode.JplHorizons) != 0)
    //        {
    //            double n = (int)(swed.eop_tjd_end - swed.eop_tjd_beg + 0.000001);
    //            double J2 = J;
    //            if (J < swed.eop_tjd_beg_horizons)
    //            {
    //                J2 = swed.eop_tjd_beg_horizons;
    //            }
    //
    //            double dpsi = bessel(swed.dpsi, n + 1, J2 - swed.eop_tjd_beg);
    //            double deps = bessel(swed.deps, n + 1, J2 - swed.eop_tjd_beg);
    //            result[0] += dpsi / 3600.0 * DegreesToRadians;
    //            result[1] += deps / 3600.0 * DegreesToRadians;
    //        }
    //    }
    //
    //    return result;
    //}

    #endregion Nutation in longitude and obliquity
}