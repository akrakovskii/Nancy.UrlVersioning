using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Nancy.UrlVersioning
{
    /// <summary>
    /// Concreate implementation of <see cref="IVersionInfoFactory"/> interfaces.
    /// Creates a new instance of <see cref="VersionInfo"/> if input string is in valid format or return null.
    /// </summary>
    public class DefaultVersionInfoFactory: IVersionInfoFactory
    {
        private const string MajorKey = "major";
        private const string MinorKey = "minor";

        private static readonly Regex VersionRegex = new Regex(@"^[vV](?<major>\d+)(?:\.(?=\d+))?(?<minor>\d+)?$",
            RegexOptions.Compiled);

        /// <summary>
        /// Creates a new instance of <see cref="VersionInfo"/> if input string is in valid format or return null.
        /// </summary>
        /// <param name="value">Version info in format v{major}.{minor} or v{major}</param>
        /// <returns>New instance of <see cref="VersionInfo"/> or null</returns>
        public IVersionInfo CreateOrDefault(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var match = VersionRegex.Match(value);

            if (!match.Success)
                return null;

            int major;
            int minor = 0;

            if (!Int32.TryParse(match.Groups[MajorKey].Value, NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out major))
                return null;

            if (match.Groups[MinorKey].Success &&
                !Int32.TryParse(match.Groups[MinorKey].Value, NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out minor))
                return null;

            return new VersionInfo(major, minor);
        }
    }
}
