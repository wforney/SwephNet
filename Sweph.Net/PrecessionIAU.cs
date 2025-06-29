namespace Sweph.Net;

/// <summary>
/// IAU precession 1976 or 2003 for recent centuries.
/// </summary>
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Already shipped.")]
public enum PrecessionIAU
{
    /// <summary>
    /// None
    /// </summary>
    None,

    /// <summary>
    /// The IAU precession 1976
    /// </summary>
    IAU_1976,

    /// <summary>
    /// The IAU precession 2000
    /// </summary>
    IAU_2000,
    /// <summary>
    /// The IAU precession model P03
    /// </summary>
    IAU_2006
}
