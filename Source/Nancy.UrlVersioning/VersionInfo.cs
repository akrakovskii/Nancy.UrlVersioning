using System;

namespace Nancy.UrlVersioning
{
    /// <summary>
    /// Concrete implementation of <see cref="IVersionInfo"/> interface, stores major and minor version numbers
    /// </summary>
    public class VersionInfo : IVersionInfo, IEquatable<VersionInfo>, IComparable<VersionInfo>, IComparable<IVersionInfo>
    {
        private readonly int _major;
        private readonly int _minor;

        public int Major { get { return _major; } }

        public int Minor { get { return _minor; } }

        public VersionInfo(int major, int minor)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException("major");

            if (minor < 0)
                throw new ArgumentOutOfRangeException("minor");

            _major = major;
            _minor = minor;
        }

        public bool Equals(VersionInfo other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VersionInfo);
        }

        public bool Equals(IVersionInfo other)
        {
            return Equals(other as VersionInfo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_major * 397) ^ _minor;
            }
        }

        public int CompareTo(VersionInfo other)
        {
            if (ReferenceEquals(other, null))
                return 1;

            var diff = _major - other.Major;
            return diff == 0 ? _minor - other.Minor : diff;
        }

        public int CompareTo(IVersionInfo other)
        {
            return CompareTo(other as VersionInfo);
        }

        public override string ToString()
        {
            return string.Format("Major: {0} Minor: {1}", _major, _minor);
        }
    }
}
