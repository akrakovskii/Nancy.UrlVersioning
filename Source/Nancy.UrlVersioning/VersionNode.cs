using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Nancy.Routing.Trie;
using Nancy.Routing.Trie.Nodes;

namespace Nancy.UrlVersioning
{
    /// <summary>
    /// Route trie node that handles version parameter
    /// </summary>
    public class VersionNode : TrieNode
    {
        private const string VersionParam = "ver";
        private const string VersionKey = "version";
        private const string ScoreKey = "score";
        private const string ParamKey = "param";

        private static readonly Regex VersionRegex =
            new Regex(@"\[(?<param>\w+\:)?(?<version>" + VersionParam + @"\(.+\))(?<score>\:\d+)?\]", RegexOptions.Compiled);

        private readonly List<IVersionInfo> _supportedVersions = new List<IVersionInfo>();
        private string _parameterName;
        private int _score;
        private readonly IVersionInfoFactory _versionFactory;

        public VersionNode(TrieNode parent, string segment, ITrieNodeFactory nodeFactory, IVersionInfoFactory versionFactory)
            : base(parent, segment, nodeFactory)
        {
            if (versionFactory == null)
                throw new ArgumentNullException("versionFactory");
            if (String.IsNullOrEmpty(segment))
                throw new ArgumentException("Value cannot be null or empty.", "segment");

            _versionFactory = versionFactory;
            ExtractParameters();
        }

        private void ExtractParameters()
        {
            var match = VersionRegex.Match(RouteDefinitionSegment);

            if (!match.Success)
                throw new FormatException(String.Format(
                    "Version segment has invalid format: {0}. Expected: [parameter:{1}(supported versions):score]",
                    RouteDefinitionSegment, VersionParam));

            _parameterName = match.Groups[ParamKey].Value.Trim(':');
            var scoreValue = match.Groups[ScoreKey].Value;

            if (match.Groups[ScoreKey].Success &&
                !Int32.TryParse(scoreValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out _score))
                throw new FormatException(
                    String.Format("Score parameter has invalid format: {0}. Integer value expected", scoreValue));

            ExtractSupportedVersions(match.Groups[VersionKey].Value);
        }

        private void ExtractSupportedVersions(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
                throw new ArgumentException("Value cannot be null or empty.", "parameter");

            var segments = parameter.Substring(VersionParam.Length).Trim('(', ')').Split(',');

            if (segments.Length == 0)
                throw new FormatException(
                    String.Format("Suported versions has invalid format: {0}. Expected: ver(versions, separated, by, commas)",
                        parameter));

            foreach (var segment in segments)
            {
                var info = _versionFactory.CreateOrDefault(segment.Trim());

                if (info == null)
                    throw new FormatException(String.Format("Version info has invalid format: {0}", segment));

                _supportedVersions.Add(info);
            }
        }

        /// <summary>
        /// Checks whether segment can be handled by this node
        /// </summary>
        /// <param name="segment">Route segment</param>
        /// <returns>True if segment can be processed, false otherwise</returns>
        public static bool Supports(string segment)
        {
            if (String.IsNullOrEmpty(segment))
                return false;

            return VersionRegex.Match(segment).Success;
        }

        /// <summary>
        /// Matches specified segment with current node
        /// </summary>
        /// <param name="segment">Path segment</param>
        /// <returns>Segment match result</returns>
        public override SegmentMatch Match(string segment)
        {
            var version = _versionFactory.CreateOrDefault(segment);

            if (version == null)
                return SegmentMatch.NoMatch;

            if (_supportedVersions.Contains(version))
            {
                var match = new SegmentMatch(true);

                if (!String.IsNullOrEmpty(_parameterName))
                {
                    match.CapturedParameters.Add(_parameterName, segment);
                }

                return match;
            }

            return SegmentMatch.NoMatch;
        }

        /// <summary>
        /// Score for this node
        /// </summary>
        public override int Score
        {
            get { return 100 + _score; }
        }
    }
}
