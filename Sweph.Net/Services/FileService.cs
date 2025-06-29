using Sweph.Net.Chronology;
using Sweph.Net.Planets;
using Sweph.Net.Properties;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Sweph.Net.Services;

/// <summary>
/// Provides file loading functionality for the Swiss Ephemeris library.
/// </summary>
public partial class FileService : IFileService
{
    private const string AsteroidFileName = "seasnam.txt";
    private const string DeltaTFileName = "swe_deltat.txt";
    private const string DeltaTFileNameAlt = "sedeltat.txt";
    private const string FictitiousFileName = "seorbel.txt";
    private const int FictitiousGeo = 1;

    /// <inheritdoc/>
    public async Task<string?> FindAsteroidNameAsync(int id, CancellationToken cancellationToken = default)
    {
        var lines = File.ReadLinesAsync(AsteroidFileName, cancellationToken);
        if (!await lines.AnyAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
        {
            // If the file is empty or does not exist, return null
            return null;
        }

        // Regex to match asteroid lines in the format: "id name"
        Regex reg = AsteroidLineRegex();

        await foreach (string line in lines.ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            string trimmedLine = line.TrimStart(' ', '\t', '(', '[', '{');
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
            {
                continue;
            }

            Match match = reg.Match(trimmedLine);
            if (!match.Success)
            {
                continue;
            }

            int planetId = int.Parse(match.Groups[1].Value, CultureInfo.CurrentCulture);
            if (planetId == id)
            {
                return match.Groups[2].Value;
            }
        }

        return null;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<DeltaT> GetDeltaTRecordsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var lines = File.ReadLinesAsync(DeltaTFileName, cancellationToken);
        if (!await lines.AnyAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
        {
            // If the file is empty or does not exist, try the alternative file
            lines = File.ReadLinesAsync(DeltaTFileNameAlt, cancellationToken);
        }

        if (!await lines.AnyAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
        {
            // If the alternative file is also empty or does not exist, return an empty sequence
            yield break;
        }

        Regex reg = DeltaTRecordLineRegex();

        await foreach (string line in lines.ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            string trimmedLine = line.Trim(' ', '\t');
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
            {
                continue;
            }

            Match match = reg.Match(line);
            if (match.Success)
            {
                if (!int.TryParse(match.Groups[1].Value, out int year))
                {
                    continue;
                }

                if (!double.TryParse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                {
                    continue;
                }

                yield return new DeltaT(year, value);
            }
        }
    }

    /// <inheritdoc/>
    public async Task<(OsculatingElement? OsculatingElement, int? fict_ifl)> FindElementAsync(int idPlanet, double julianDay, int fictIfl, CancellationToken cancellationToken = default)
    {
        var lines = File.ReadLinesAsync(FictitiousFileName, Encoding.UTF8, cancellationToken);
        if (!await lines.AnyAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
        {
            // If the file is empty or does not exist, return null
            return (null, fictIfl);
        }

        OsculatingElement? result = null;
        int lineCount = 0;
        int planetNumber = -1;

        await foreach (string line in lines.ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            lineCount++;

            var trimmedLine = line.Trim(' ', '\t');
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                continue;
            }

            // Remove comments ending the line
            int iTmp = line.IndexOf('#', StringComparison.Ordinal);
            if (iTmp >= 0)
            {
                trimmedLine = trimmedLine[..iTmp];
            }

            // Split parts
            var parts = line.Split([','], StringSplitOptions.RemoveEmptyEntries);

            // serri = C.sprintf("error in file %s, line %7.0f:", SwissEph.SE_FICTFILE, (double)iline);
            if (parts.Length < 9)
            {
                throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorNineElementsRequired);
            }

            planetNumber++;
            if (planetNumber != idPlanet)
            {
                continue;
            }

            // epoch of elements
            string sp = parts[0].ToUpperInvariant();
            double epoch = true switch
            {
                bool when sp.StartsWith("J2000", StringComparison.OrdinalIgnoreCase) => JulianDay.J2000,
                bool when sp.StartsWith("B1950", StringComparison.OrdinalIgnoreCase) => JulianDay.B1950,
                bool when sp.StartsWith("J1900", StringComparison.OrdinalIgnoreCase) => JulianDay.J1900,
                bool when sp.StartsWith('J') || sp.StartsWith('B') => throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorInvalidEpoch),
                _ => double.Parse(sp, CultureInfo.InvariantCulture)
            };

            var tt = julianDay - epoch;

            // equinox
            sp = parts[1].TrimStart(' ', '\t').ToUpperInvariant();
            double equinox = true switch
            {
                bool when sp.StartsWith("J2000", StringComparison.OrdinalIgnoreCase) => JulianDay.J2000,
                bool when sp.StartsWith("B1950", StringComparison.OrdinalIgnoreCase) => JulianDay.B1950,
                bool when sp.StartsWith("J1900", StringComparison.OrdinalIgnoreCase) => JulianDay.J1900,
                bool when sp.StartsWith("JDATE", StringComparison.OrdinalIgnoreCase) => julianDay,
                bool when sp.StartsWith('J') || sp.StartsWith('B') => throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorInvalidEquinox),
                _ => double.Parse(sp, CultureInfo.InvariantCulture)
            };

            // mean anomaly t0
            var retc = CheckTTerms(tt, parts[2], out double dTmp);
            if (retc < 0)
            {
                throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorMeanAnomalyValueInvalid);
            }

            var meanAnomaly = Context.DegNorm(dTmp);

            // if mean anomaly has t terms (which happens with fictitious planet Vulcan),
            // we set epoch = tjd, so that no motion will be added anymore equinox = tjd
            if (retc == 1)
            {
                epoch = julianDay;
            }

            meanAnomaly *= Context.DegreesToRadians;

            // semi-axis
            retc = CheckTTerms(tt, parts[3], out dTmp);
            if (dTmp <= 0 || retc < 0)
            {
                throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorSemiAxisValueInvalid);
            }

            double semiAxis = dTmp;

            // eccentricity
            retc = CheckTTerms(tt, parts[4], out dTmp);
            if (dTmp >= 1 || dTmp < 0 || retc < 0)
            {
                throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorEccentricityValueInvalid);
            }

            double eccentricity = dTmp;

            // perihelion argument
            retc = CheckTTerms(tt, parts[5], out dTmp);
            if (retc < 0)
            {
                throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorPerihelionArgumentValueInvalid);
            }

            double perihelion = Context.DegNorm(dTmp);
            perihelion *= Context.DegreesToRadians;

            // node
            retc = CheckTTerms(tt, parts[6], out dTmp);
            if (retc < 0)
            {
                throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorNodeValueInvalid);
            }

            var ascendingNode = Context.DegNorm(dTmp);
            ascendingNode *= Context.DegreesToRadians;

            // Inclination
            retc = CheckTTerms(tt, parts[7], out dTmp);
            if (retc < 0)
            {
                throw new SwephNetException(Resources.Error_ReadingFile, FictitiousFileName, lineCount, Resources.Fictitious_ErrorInclinationValueInvalid);
            }

            var inclination = Context.DegNorm(dTmp);
            inclination *= Context.DegreesToRadians;

            // planet name
            var name = parts[8].Trim(' ', '\t');

            // geocentric
            if (parts.Length > 9)
            {
                parts[9] = parts[9].ToUpperInvariant();
                if (parts[9].Contains("GEO", StringComparison.OrdinalIgnoreCase))
                {
                    fictIfl |= FictitiousGeo;
                }
            }

            // create the result
            result = new OsculatingElement(
                name,
                epoch,
                equinox,
                meanAnomaly,
                semiAxis,
                eccentricity,
                perihelion,
                ascendingNode,
                inclination);

            break;
        }


        if (result is null)
        {
            var format = CompositeFormat.Parse(Resources.Fictitious_ErrorElementsNotFound);
            throw new SwephNetException(
                Resources.Error_ReadingFile,
                FictitiousFileName,
                lineCount,
                string.Format(CultureInfo.CurrentCulture, format, idPlanet));
        }

        return (result, fictIfl);
    }

    [GeneratedRegex(@"^[\s\(\{\[]*(\d+)[\s\)\}\]]*(.+)$")]
    private static partial Regex AsteroidLineRegex();

    private static int CheckTTerms(double t, string sinp, out double doutp)
    {
        int z;
        int retc = 0;
        double[] tt = new double[5];
        double fac;

        tt[0] = t / 36525;
        tt[1] = tt[0];
        tt[2] = tt[1] * tt[1];
        tt[3] = tt[2] * tt[1];
        tt[4] = tt[3] * tt[1];
        if (sinp.Contains('+', StringComparison.Ordinal) || sinp.Contains('-', StringComparison.Ordinal))
        {
            retc = 1; // with additional terms
        }

        string? sp = sinp;
        doutp = 0;
        fac = 1;
        z = 0;
        while (true)
        {
            sp = sp?.TrimStart(' ', '\t');
            if (string.IsNullOrWhiteSpace(sp) || sp.StartsWith('+') || sp.StartsWith('-'))
            {
                if (z > 0)
                {
                    doutp += fac;
                }

                int isgn = 1;
                if (sp is not null && sp.StartsWith('-'))
                {
                    isgn = -1;
                }

                fac = 1 * isgn;
                if (string.IsNullOrWhiteSpace(sp))
                {
                    return retc;
                }

                sp = sp[1..];
            }
            else
            {
                sp = sp.TrimStart('*', ' ', '\t');
                if (sp is not null && sp.StartsWith('t'))
                {
                    /* a T */
                    sp = sp[1..];
                    if (sp is not null && (sp.StartsWith('+') || sp.StartsWith('-')))
                    {
                        fac *= tt[0];
                    }
                    else
                    {
                        if (!int.TryParse(sp, out int i))
                        {
                            i = 0;
                        }

                        if (i <= 4 && i >= 0)
                        {
                            fac *= tt[i];
                        }
                    }
                }
                else
                {
                    /* a number */
                    int cnt = 0;
                    while (cnt < (sp?.Length ?? 0) && sp is not null && "0123456789.".Contains(sp[cnt++], StringComparison.Ordinal))
                    {
                        ;
                    }

                    string? sval = cnt < 0 ? sp : sp?[..cnt];
                    if ((double.TryParse(sval, CultureInfo.CurrentCulture, out var val) && val != 0) || (sp?.StartsWith('0') ?? false))
                    {
                        fac *= val;
                    }
                }

                if (sp is not null)
                {
                    sp = sp.TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.');
                }
            }

            z++;
        }

        //return retc;	// there have been additional terms
    }

    [GeneratedRegex(@"^(\d{4})\s+(\d+\.\d+)$")]
    private static partial Regex DeltaTRecordLineRegex();
}
