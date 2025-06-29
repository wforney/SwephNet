namespace Sweph.Net;

/// <summary>
/// Settings for the Sweph.Net library.
/// </summary>
public class SwephNetSettings
{
    /// <summary>
    /// Name of the configuration section for Sweph.Net settings.
    /// </summary>
    public const string SectionName = "SwephNet";

    /// <summary>
    /// If the above define INCLUDE_CODE_FOR_DPSI_DEPS_IAU1980 is FALSE or the software does not
    /// find the earth orientation files (see above) in the ephemeris path, then SEFLG_JPLHOR will
    /// run as SEFLG_JPLHOR_APPROX. The following define APPROXIMATE_HORIZONS_ASTRODIENST defines
    /// the handling of SEFLG_JPLHOR_APPROX. With this flag, planetary positions are always
    /// calculated using a recent precession/nutation model. If APPROXIMATE_HORIZONS_ASTRODIENST is
    /// FALSE, then the frame bias as recommended by IERS Conventions 2003 and 2010 is *not*
    /// applied. Instead, dpsi_bias and deps_bias are added to nutation. This procedure is found in
    /// some older astronomical software. Equatorial apparent positions will be close to JPL
    /// Horizons (within a few mas) beetween 1962 and current years. Ecl. longitude will be good,
    /// latitude bad. If APPROXIMATE_HORIZONS_ASTRODIENST is TRUE, the approximation of JPL Horizons
    /// is even better. Frame bias matrix is applied with some correction to RA and another
    /// correction is added to epsilon.
    /// </summary>
    public bool ApproximateHorizonsAstrodienst { get; set; } = true;

    /// <summary>
    /// You can set the latter false if you do not want to compile the code required to reproduce
    /// JPL Horizons. Keep it TRUE in order to reproduce JPL Horizons following IERS Conventions
    /// 1996 (1992), p. 22. Call swe_calc_ut() with iflag|SEFLG_JPLHOR. This options runs only, if
    /// the files DPSI_DEPS_IAU1980_FILE_EOPC04 and DPSI_DEPS_IAU1980_FILE_FINALS are in the
    /// ephemeris path.
    /// </summary>
    public bool IncludeCodeForDpsiDepsIAU1980 { get; set; } = true;

    /// <summary>
    /// The latter, if combined with SEFLG_JPLHOR provides good agreement with JPL Horizons for 1800
    /// - today. However, Horizons uses correct dpsi and deps only after 20-jan-1962. For all dates
    /// before that it uses dpsi and deps of 20-jan-1962, which provides a continuous ephemeris, but
    /// does not make sense otherwise. Before 1800, even this option does not provide agreement with
    /// Horizons, because Horizons uses a different precession model (Owen 1986) before 1800, which
    /// is not included in the Swiss Ephemeris. If this macro is FALSE then the program defaults to
    /// SEFLG_JPLHOR_APPROX outside the time range of correction data dpsi and deps. Note that this
    /// will result in a non-continuous ephemeris near 20-jan-1962 and current years.
    /// </summary>
    /// <remarks>Horizons method before 20-jan-1962</remarks>
    public bool UseHorizonsMethodBefore1980 { get; set; } = true;

    /// <summary>
    /// Precession coefficients for remote past and future
    /// </summary>
    public PrecessionCoefficients UsePrecessionCoefficient { get; set; } = PrecessionCoefficients.Vondrak2011;

    /// <summary>
    /// IAU precession 1976 or 2003 for recent centuries.
    /// </summary>
    public PrecessionIAU UsePrecessionIAU { get; set; } = PrecessionIAU.None;
}