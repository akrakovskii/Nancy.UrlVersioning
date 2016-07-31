namespace Nancy.UrlVersioning
{
    /// <summary>
    /// Represents a factory that is used by <see cref="VersionNode"/> to resolve version information
    /// </summary>
    public interface IVersionInfoFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IVersionInfo"/>
        /// </summary>
        /// <param name="value">Version info in string form</param>
        /// <returns>New instance of <see cref="IVersionInfo"/></returns>
        IVersionInfo CreateOrDefault(string value);
    }
}
