using System.Globalization;

namespace Sweph.Net.Planets;

/// <summary>
/// Planet number
/// </summary>
public readonly record struct Planet(int Id)
{
    #region Planets

    /// <summary>
    /// Sun
    /// </summary>
    public static readonly Planet Sun = 0;

    /// <summary>
    /// Moon
    /// </summary>
    public static readonly Planet Moon = 1;

    /// <summary>
    /// Mercury
    /// </summary>
    public static readonly Planet Mercury = 2;

    /// <summary>
    /// Venus
    /// </summary>
    public static readonly Planet Venus = 3;

    /// <summary>
    /// Mars
    /// </summary>
    public static readonly Planet Mars = 4;

    /// <summary>
    /// Jupiter
    /// </summary>
    public static readonly Planet Jupiter = 5;

    /// <summary>
    /// Saturn
    /// </summary>
    public static readonly Planet Saturn = 6;

    /// <summary>
    /// Uranus
    /// </summary>
    public static readonly Planet Uranus = 7;

    /// <summary>
    /// Neptune
    /// </summary>
    public static readonly Planet Neptune = 8;

    /// <summary>
    /// Pluto
    /// </summary>
    public static readonly Planet Pluto = 9;

    /// <summary>
    /// MeanNode
    /// </summary>
    public static readonly Planet MeanNode = 10;

    /// <summary>
    /// TrueNode
    /// </summary>
    public static readonly Planet TrueNode = 11;

    /// <summary>
    /// MeanApog
    /// </summary>
    public static readonly Planet MeanApog = 12;

    /// <summary>
    /// OscuApog
    /// </summary>
    public static readonly Planet OscuApog = 13;

    /// <summary>
    /// Earth
    /// </summary>
    public static readonly Planet Earth = 14;

    /// <summary>
    /// Chiron
    /// </summary>
    public static readonly Planet Chiron = 15;

    /// <summary>
    /// Pholus
    /// </summary>
    public static readonly Planet Pholus = 16;

    /// <summary>
    /// Ceres
    /// </summary>
    public static readonly Planet Ceres = 17;

    /// <summary>
    /// Pallas
    /// </summary>
    public static readonly Planet Pallas = 18;

    /// <summary>
    /// Juno
    /// </summary>
    public static readonly Planet Juno = 19;

    /// <summary>
    /// Vesta
    /// </summary>
    public static readonly Planet Vesta = 20;

    /// <summary>
    /// IntpApog
    /// </summary>
    public static readonly Planet IntpApog = 21;

    /// <summary>
    /// IntpPerg
    /// </summary>
    public static readonly Planet IntpPerg = 22;

    #endregion Planets

    #region Specials

    /// <summary>
    /// Ecliptic/Nutation
    /// </summary>
    public static readonly Planet EclipticNutation = -1;

    /// <summary>
    /// Fixed star
    /// </summary>
    public static readonly Planet FixedStar = -10;

    #endregion Specials

    #region Fictitious

    #region Hamburger or Uranian "planets"

    /// <summary>
    /// The planet Cupido
    /// </summary>
    public static readonly Planet Cupido = 40;

    /// <summary>
    /// The planet Hades
    /// </summary>
    public static readonly Planet Hades = 41;

    /// <summary>
    /// The planet Zeus
    /// </summary>
    public static readonly Planet Zeus = 42;

    /// <summary>
    /// The planet Kronos
    /// </summary>
    public static readonly Planet Kronos = 43;

    /// <summary>
    /// The planet Apollon
    /// </summary>
    public static readonly Planet Apollon = 44;

    /// <summary>
    /// The planet Admetos
    /// </summary>
    public static readonly Planet Admetos = 45;

    /// <summary>
    /// The planet Vulkanus
    /// </summary>
    public static readonly Planet Vulkanus = 46;

    /// <summary>
    /// The planet Poseidon
    /// </summary>
    public static readonly Planet Poseidon = 47;

    #endregion Hamburger or Uranian "planets"

    #region Other fictitious bodies

    /// <summary>
    /// The planet Isis
    /// </summary>
    public static readonly Planet Isis = 48;

    /// <summary>
    /// The planet Nibiru
    /// </summary>
    public static readonly Planet Nibiru = 49;

    /// <summary>
    /// The planet Harrington
    /// </summary>
    public static readonly Planet Harrington = 50;

    /// <summary>
    /// The planet Neptune Leverrier
    /// </summary>
    public static readonly Planet NeptuneLeverrier = 51;

    /// <summary>
    /// The planet Neptune Adams
    /// </summary>
    public static readonly Planet NeptuneAdams = 52;

    /// <summary>
    /// The planet Pluto Lowell
    /// </summary>
    public static readonly Planet PlutoLowell = 53;

    /// <summary>
    /// The planet Pluto Pickering
    /// </summary>
    public static readonly Planet PlutoPickering = 54;

    /// <summary>
    /// The planet Vulcan
    /// </summary>
    public static readonly Planet Vulcan = 55;

    /// <summary>
    /// The planet White Moon
    /// </summary>
    public static readonly Planet WhiteMoon = 56;

    /// <summary>
    /// The planet Proserpina
    /// </summary>
    public static readonly Planet Proserpina = 57;

    /// <summary>
    /// The planet Waldemath
    /// </summary>
    public static readonly Planet Waldemath = 58;

    #endregion Other fictitious bodies

    #endregion Fictitious

    #region Asteroids

    /// <summary>
    /// The asteroid Ceres
    /// </summary>
    public static readonly Planet AsteroidCeres = FirstAsteroid + 1;

    /// <summary>
    /// The asteroid Pallas
    /// </summary>
    public static readonly Planet AsteroidPallas = FirstAsteroid + 2;

    /// <summary>
    /// The asteroid Juno
    /// </summary>
    public static readonly Planet AsteroidJuno = FirstAsteroid + 3;

    /// <summary>
    /// The asteroid Vesta
    /// </summary>
    public static readonly Planet AsteroidVesta = FirstAsteroid + 4;

    /// <summary>
    /// The asteroid Chiron
    /// </summary>
    public static readonly Planet AsteroidChiron = FirstAsteroid + 2060;

    /// <summary>
    /// The asteroid Pholus
    /// </summary>
    public static readonly Planet AsteroidPholus = FirstAsteroid + 5145;

    /// <summary>
    /// Pluto as Asteroid
    /// </summary>
    public static readonly Planet AsteroidPluto = 134340;

    #endregion Asteroids

    /// <summary>
    /// Last id of 'planet'
    /// </summary>
    public const int LastPlanet = 23;

    /// <summary>
    /// First id of fictitious
    /// </summary>
    public const int FirstFictitious = 40;

    /// <summary>
    /// First id of comets
    /// </summary>
    public const int FirstComet = 1000;

    /// <summary>
    /// First id of asteroid
    /// </summary>
    public const int FirstAsteroid = 10000;

    /// <summary>
    /// Create a new planet as asteroid
    /// </summary>
    /// <param name="id">Id of the asteroid</param>
    /// <returns>The asteroid</returns>
    public static Planet AsAsteroid(int id) => new(FirstAsteroid + id);

    /// <inheritdoc/>
    public override string ToString() => Id.ToString(CultureInfo.CurrentCulture);

    /// <summary>
    /// Implicit casting SwePlanet to Int32
    /// </summary>
    public static implicit operator int(Planet planet) => planet.Id;

    /// <summary>
    /// Implicit casting Int32 to SwePlanet
    /// </summary>
    public static implicit operator Planet(int id) => new(id);

    /// <summary>
    /// This id is a planet ?
    /// </summary>
    public bool IsPlanet => Id is >= 0 and <= LastPlanet;

    /// <summary>
    /// This id is a fictitious ?
    /// </summary>
    public bool IsFictitious => Id is >= FirstFictitious and < FirstComet;

    /// <summary>
    /// This id is a comet ?
    /// </summary>
    public bool IsComet => Id is >= FirstComet and < FirstAsteroid;

    /// <summary>
    /// This id is an asteroid ?
    /// </summary>
    public bool IsAsteroid => Id is >= FirstAsteroid;

    /// <summary>
    /// Convert this planet to an Int32.
    /// </summary>
    /// <returns>The integer representation of the planet's id.</returns>
    public int ToInt32() => Id;

    /// <summary>
    /// Create a new planet from an Int32 id.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>A new instance of <see cref="Planet"/> with the specified id.</returns>
    public static Planet FromInt32(int id) =>
        id < 0
            ? throw new ArgumentOutOfRangeException(nameof(id), "Planet id must be non-negative.")
            : new Planet(id);
}
