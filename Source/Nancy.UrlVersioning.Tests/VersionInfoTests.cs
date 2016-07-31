using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nancy.UrlVersioning.Tests
{
    [TestClass]
    public class VersionInfoTests
    {
        [TestMethod]
        public void Can_Compare_Versions_Equal()
        {
            new VersionInfo(1, 2).CompareTo(new VersionInfo(1, 2)).Should().Be(0);
        }

        [TestMethod]
        public void Can_Compare_Versions_Minor_Greater()
        {
            new VersionInfo(1, 2).CompareTo(new VersionInfo(1, 1)).Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void Can_Compare_Versions_Minor_Lower()
        {
            new VersionInfo(1, 2).CompareTo(new VersionInfo(1, 3)).Should().BeLessThan(0);
        }

        [TestMethod]
        public void Can_Compare_Versions_Major_Greater()
        {
            new VersionInfo(2, 1).CompareTo(new VersionInfo(1, 1)).Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void Can_Compare_Versions_Major_Lower()
        {
            new VersionInfo(1, 1).CompareTo(new VersionInfo(2, 1)).Should().BeLessThan(0);
        }
    }
}
