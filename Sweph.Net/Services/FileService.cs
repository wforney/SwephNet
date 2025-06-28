using Sweph.Net.Chronology;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Sweph.Net.Services;

/// <summary>
/// Provides file loading functionality for the Swiss Ephemeris library.
/// </summary>
internal partial class FileService
{
    /// <summary>
    /// Get delta t records as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IAsyncEnumerable`1&gt; representing the asynchronous operation.</returns>
    public static async IAsyncEnumerable<DeltaT> GetDeltaTRecordsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string[]? lines = await File.ReadAllLinesAsync("swe_deltat.txt", cancellationToken).ConfigureAwait(false);
        if (lines is null || lines.Length == 0)
        {
            lines = await File.ReadAllLinesAsync("sedeltat.txt", cancellationToken).ConfigureAwait(false);
        }

        if (lines is null || lines.Length == 0)
        {
            yield break;
        }

        Regex reg = DeltaTRecordLineRegex();

        foreach (string line in lines)
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

    [GeneratedRegex(@"^(\d{4})\s+(\d+\.\d+)$")]
    private static partial Regex DeltaTRecordLineRegex();
}
