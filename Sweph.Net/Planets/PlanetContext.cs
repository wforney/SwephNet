using Microsoft.Extensions.Logging;
using Sweph.Net.Chronology;
using Sweph.Net.Properties;
using Sweph.Net.Services;
using System.Globalization;
using System.Text;

namespace Sweph.Net.Planets;

/// <summary>
/// PlanetContext is a class that manages the names and properties of planets, asteroids, and fictitious bodies.
/// </summary>
public partial class PlanetContext(ILogger<PlanetContext> logger)
{
    private Dictionary<int, string> _bufferNames = [];

    // Suffixes planets locales names
    private static readonly string[] PlanetNames =
        [
        "Sun",
        "Moon",
        "Mercury",
        "Venus",
        "Mars",
        "Jupiter",
        "Saturn",
        "Uranus",
        "Neptune",
        "Pluto",
        "MeanNode",
        "TrueNode",
        "MeanApogee",
        "OscuApogee",
        "Earth",
        "Chiron",
        "Pholus",
        "Ceres",
        "Pallas",
        "Juno",
        "Vesta",
        "IntpApogee",
        "IntpPerigee"
        ];

    private static readonly string[] PlanetFictitiousNames =
        [
        "Cupido",
        "Hades",
        "Zeus",
        "Kronos",
        "Apollon",
        "Admetos",
        "Vulkanus",
        "Poseidon",
        "Isis-Transpluto",
        "Nibiru",
        "Harrington",
        "Leverrier",
        "Adams",
        "Lowell",
        "Pickering"
        ];

    #region Osculating elements

    private static double[][] plan_oscu_elem_neely = [
          [JulianDay.J1900, JulianDay.J1900, 163.7409, 40.99837, 0.00460, 171.4333, 129.8325, 1.0833],/* Cupido Neely */
          [JulianDay.J1900, JulianDay.J1900,  27.6496, 50.66744, 0.00245, 148.1796, 161.3339, 1.0500],/* Hades Neely */
          [JulianDay.J1900, JulianDay.J1900, 165.1232, 59.21436, 0.00120, 299.0440,   0.0000, 0.0000],/* Zeus Neely */
          [JulianDay.J1900, JulianDay.J1900, 169.0193, 64.81960, 0.00305, 208.8801,   0.0000, 0.0000],/* Kronos Neely */
          [JulianDay.J1900, JulianDay.J1900, 138.0533, 70.29949, 0.00000,   0.0000,   0.0000, 0.0000],/* Apollon Neely */
          [JulianDay.J1900, JulianDay.J1900, 351.3350, 73.62765, 0.00000,   0.0000,   0.0000, 0.0000],/* Admetos Neely */
          [JulianDay.J1900, JulianDay.J1900,  55.8983, 77.25568, 0.00000,   0.0000,   0.0000, 0.0000],/* Vulcanus Neely */
          [JulianDay.J1900, JulianDay.J1900, 165.5163, 83.66907, 0.00000,   0.0000,   0.0000, 0.0000],/* Poseidon Neely */
          /* Isis-Transpluto; elements from "Die Sterne" 3/1952, p. 70ff.
           * Strubell does not give an equinox. 1945 is taken to best reproduce
           * ASTRON ephemeris. (This is a strange choice, though.)
           * The epoch is 1772.76. The year is understood to have 366 days.
           * The fraction is counted from 1 Jan. 1772 */
          [2368547.66, 2431456.5, 0.0, 77.775, 0.3, 0.7, 0, 0],
          /* Nibiru, elements from Christian Woeltge, Hannover */
          [1856113.380954, 1856113.380954, 0.0, 234.8921, 0.981092, 103.966, -44.567, 158.708],
          /* Harrington, elements from Astronomical Journal 96(4), Oct. 1988 */
          new double[] {2374696.5, JulianDay.J2000, 0.0, 101.2, 0.411, 208.5, 275.4, 32.4},
          /* Leverrier's Neptune,
            according to W.G. Hoyt, "Planets X and Pluto", Tucson 1980, p. 63 */
          [2395662.5, 2395662.5, 34.05, 36.15, 0.10761, 284.75, 0, 0],
          /* Adam's Neptune */
          [2395662.5, 2395662.5, 24.28, 37.25, 0.12062, 299.11, 0, 0],
          /* Lowell's Pluto */
          [2425977.5, 2425977.5, 281, 43.0, 0.202, 204.9, 0, 0],
          /* Pickering's Pluto */
          [2425977.5, 2425977.5, 48.95, 55.1, 0.31, 280.1, 100, 15], /**/
        ];

    private static double[][] plan_oscu_elem_no_neely = [
          [JulianDay.J1900, JulianDay.J1900, 104.5959, 40.99837,  0, 0, 0, 0], /* Cupido   */
          [JulianDay.J1900, JulianDay.J1900, 337.4517, 50.667443, 0, 0, 0, 0], /* Hades    */
          [JulianDay.J1900, JulianDay.J1900, 104.0904, 59.214362, 0, 0, 0, 0], /* Zeus     */
          [JulianDay.J1900, JulianDay.J1900,  17.7346, 64.816896, 0, 0, 0, 0], /* Kronos   */
          [JulianDay.J1900, JulianDay.J1900, 138.0354, 70.361652, 0, 0, 0, 0], /* Apollon  */
          [JulianDay.J1900, JulianDay.J1900,  -8.678,  73.736476, 0, 0, 0, 0], /* Admetos  */
          [JulianDay.J1900, JulianDay.J1900,  55.9826, 77.445895, 0, 0, 0, 0], /* Vulkanus */
          [JulianDay.J1900, JulianDay.J1900, 165.3595, 83.493733, 0, 0, 0, 0], /* Poseidon */
          /* Isis-Transpluto; elements from "Die Sterne" 3/1952, p. 70ff.
           * Strubell does not give an equinox. 1945 is taken to best reproduce
           * ASTRON ephemeris. (This is a strange choice, though.)
           * The epoch is 1772.76. The year is understood to have 366 days.
           * The fraction is counted from 1 Jan. 1772 */
          [2368547.66, 2431456.5, 0.0, 77.775, 0.3, 0.7, 0, 0],
          /* Nibiru, elements from Christian Woeltge, Hannover */
          [1856113.380954, 1856113.380954, 0.0, 234.8921, 0.981092, 103.966, -44.567, 158.708],
          /* Harrington, elements from Astronomical Journal 96(4), Oct. 1988 */
          new double[] {2374696.5, JulianDay.J2000, 0.0, 101.2, 0.411, 208.5, 275.4, 32.4},
          /* Leverrier's Neptune,
            according to W.G. Hoyt, "Planets X and Pluto", Tucson 1980, p. 63 */
          [2395662.5, 2395662.5, 34.05, 36.15, 0.10761, 284.75, 0, 0],
          /* Adam's Neptune */
          [2395662.5, 2395662.5, 24.28, 37.25, 0.12062, 299.11, 0, 0],
          /* Lowell's Pluto */
          [2425977.5, 2425977.5, 281, 43.0, 0.202, 204.9, 0, 0],
          /* Pickering's Pluto */
          [2425977.5, 2425977.5, 48.95, 55.1, 0.31, 280.1, 100, 15], /**/
        ];

    private double[][] plan_oscu_elem { get { return UseNeely ? plan_oscu_elem_neely : plan_oscu_elem_no_neely; } }

    /// <summary>
    /// Read an osculating element for a planet and a julian day
    /// </summary>
    /// <param name="idPlanet">
    /// Id of the planet for which the osculating element is requested.
    /// </param>
    /// <param name="jd">
    /// Julian day for which the osculating element is requested.
    /// </param>
    /// <param name="fictitiousPlanetFile">
    /// If the planet is fictitious, this parameter is used to specify the fictitious planet file.
    /// </param>
    /// <returns>
    /// A tuple containing the osculating element and an optional fictitious planet file index.
    /// </returns>
    protected async Task<(OsculatingElement? result, int? fict_ifl)> ReadElementAsync(int idPlanet, double jd, int fictitiousPlanetFile)
    {
        OsculatingElement? result;
        int? fict_iflo;
        (result, fict_iflo) = await FileService.FindElementAsync(idPlanet, jd, fictitiousPlanetFile).ConfigureAwait(false);

        // If file or planet not found, use built-in bodies
        if (result is null)
        {
            var planOscu = plan_oscu_elem;
            if (idPlanet >= planOscu.Length)
            {
                throw new SwephNetException(Resources.Fictitious_ErrorNoElements, idPlanet);
            }

            var plan = planOscu[idPlanet];
            result = new OsculatingElement()
            {
                Epoch = plan[0],
                Equinox = plan[1],
                MeanAnomaly = plan[2] * Context.DegreesToRadians,
                SemiAxis = plan[3],
                Eccentricity = plan[4],
                Perihelion = plan[5] * Context.DegreesToRadians,
                AscendingNode = plan[6] * Context.DegreesToRadians,
                Inclination = plan[7] * Context.DegreesToRadians,
                Name = Resources.ResourceManager.GetString(
                    string.Format(CultureInfo.CurrentCulture, "FictitiousName_{0}", PlanetFictitiousNames[idPlanet]), CultureInfo.CurrentCulture)
                    ?? throw new ArgumentNullException(nameof(OsculatingElement.Name))
            };
        }

        return (result, fict_iflo);
    }

    #endregion

    /// <summary>
    /// Returns the name of a fictitious planet
    /// </summary>
    /// <param name="id">Id of the fictitious planet. 0 is the first fictitious id.</param>
    /// <returns>Name of the fictitious planet</returns>
    protected async Task<string> GetFictitiousNameAsync(int id)
    {
        try
        {
            int iDummy = 0;
            var (result, fict_ifl) = await ReadElementAsync(id, 0, iDummy).ConfigureAwait(false);
            if (result is not null)
            {
                return result.Value.Name;
            }
        }
        catch (Exception ex)
        {
            LogFictitiousNameNotFound(logger, id, ex);
        }
        return Resources.Fictitious_NameNotFound;
    }

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Error,
        Message = "Fictitious name not found for id {id}")]
    private static partial void LogFictitiousNameNotFound(ILogger logger, int id, Exception? exception);

    /// <summary>
    /// Returns the name of an asteroid
    /// </summary>
    /// <param name="asteroid">Id of the asteroid. 0 is the first asteroid id</param>
    /// <returns>Name of the asteroid</returns>
    protected static async Task<string> GetAsteroidNameAsync(int asteroid)
    {
        // TODO Check to implement this from a future 'File Data' ?????
        ///* if name is already available */
        //if (ipl == swed.fidat[SEI_FILE_ANY_AST].ipl[0])
        //    s = swed.fidat[SEI_FILE_ANY_AST].astnam;
        ///* else try to get it from ephemeris file */
        //else {
        //    var retc = sweph(J2000, ipl, SEI_FILE_ANY_AST, 0, null, NO_SAVE, xp, ref sdummy);
        //    if (retc != ERR && retc != NOT_AVAILABLE)
        //        s = swed.fidat[SEI_FILE_ANY_AST].astnam;
        //    else
        //        s = C.sprintf("%d: not found", ipl - SE_AST_OFFSET);
        var format = CompositeFormat.Parse(Resources.Asteroid_NameNotFound);
        string result = string.Format(CultureInfo.CurrentCulture, format, asteroid);
        //}

        // If there is a provisional designation only in ephemeris file,
        // we look for a name in seasnam.txt, which can be updated by
        // the user.
        // Some old ephemeris files return a '?' in the first position.
        // There are still a couple of unnamed bodies that got their
        // provisional designation before 1925, when the current method
        // of provisional designations was introduced. They have an 'A'
        // as the first character, e.g. A924 RC.

        // If name not found
        if (string.IsNullOrWhiteSpace(result) || "?0123456789".Contains(result[0].ToString(), StringComparison.Ordinal) || (result.Length > 1 && char.IsDigit(result[1])))
        {
            // Try to read from asteroid names file
            result = (await FileService.FindAsteroidNameAsync(asteroid).ConfigureAwait(false)) ?? result;
        }

        return result;
    }

    /// <summary>
    /// Returns the name of planet <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id of the planet</param>
    /// <returns>Name of the planet</returns>
    public async Task<string> GetPlanetNameAsync(Planet id)
    {
        // Update the id for the asteroid used like a planet
        if (id == Planet.AsteroidPluto)
        {
            id = Planet.Pluto;
        }

        if (id == Planet.AsteroidCeres)
        {
            id = Planet.Ceres;
        }

        if (id == Planet.AsteroidPallas)
        {
            id = Planet.Pallas;
        }

        if (id == Planet.AsteroidJuno)
        {
            id = Planet.Juno;
        }

        if (id == Planet.AsteroidVesta)
        {
            id = Planet.Vesta;
        }

        if (id == Planet.AsteroidChiron)
        {
            id = Planet.Chiron;
        }

        if (id == Planet.AsteroidPholus)
        {
            id = Planet.Pholus;
        }

        // Check the buffer
        if (_bufferNames.TryGetValue(id, out string? result))
        {
            return result;
        }

        // Planets names
        if (id >= 0 && id <= Planet.LastPlanet)
        {
            result = Resources.ResourceManager.GetString(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "PlanetName_{0}",
                    PlanetNames[id]),
                CultureInfo.CurrentCulture);
        }
        else
        {
            if (id.IsFictitious)
            {
                result = await GetFictitiousNameAsync(id - Planet.FirstFictitious).ConfigureAwait(false);
            }
            else if (id.IsAsteroid)
            {
                result = await GetAsteroidNameAsync(id - Planet.FirstAsteroid).ConfigureAwait(false);
            }
            else
            {    // If not found return the id
                result = id.Id.ToString(CultureInfo.InvariantCulture);
            }
        }

        // Save the result in the buffer
        if (string.IsNullOrWhiteSpace(result))
        {
            result = Resources.Planet_NameNotFound;
        }

        _bufferNames[id] = result;
        return result;
    }

    /// <summary>
    /// Use James Neely's revised elements of Uranian planets
    /// </summary>
    public bool UseNeely { get; set; } = true;

}
