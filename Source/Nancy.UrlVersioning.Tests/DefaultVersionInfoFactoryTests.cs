using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nancy.UrlVersioning.Tests
{
    [TestClass]
    public class DefaultVersionInfoFactoryTests
    {
        private readonly DefaultVersionInfoFactory _factory = new DefaultVersionInfoFactory();

        [TestMethod]
        public void Can_Parse_Version_With_Major_Only()
        {
            _factory.CreateOrDefault("v1").VersionShouldBe(1, 0);
        }

        [TestMethod]
        public void Can_Parse_Version_With_Major_And_Minor()
        {
            _factory.CreateOrDefault("v1.2").VersionShouldBe(1, 2);
        }

        [TestMethod]
        public void Can_Parse_Version_With_UpperCase_V()
        {
            _factory.CreateOrDefault("V2.5").VersionShouldBe(2, 5);
        }

        [TestMethod]
        public void Can_Parse_Version_With_LowerCase_V()
        {
            _factory.CreateOrDefault("v2.5").VersionShouldBe(2, 5);
        }

        [TestMethod]
        public void Fail_Parse_Version_Without_V()
        {
            _factory.CreateOrDefault("1.2").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_Non_Numeric_String()
        {
            _factory.CreateOrDefault("abc").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_Version_String_Started_With_Dot()
        {
            _factory.CreateOrDefault("v.").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_Version_String_Started_With_Dot2()
        {
            _factory.CreateOrDefault("v.2").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_Version_String_With_Dot_And_Without_Minor()
        {
            _factory.CreateOrDefault("v1.").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_String_With_V_Only()
        {
            _factory.CreateOrDefault("v").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_String_With_Major_And_Non_Numeric_Minor()
        {
            _factory.CreateOrDefault("v1.abc").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_Version_String_With_Major_Overflow()
        {
            _factory.CreateOrDefault("v1324324324324324242442345433455.2").Should().BeNull();
        }

        [TestMethod]
        public void Fail_Parse_Version_String_With_Minor_Overflow()
        {
            _factory.CreateOrDefault("v1.26456546353958734598345734958435734895").Should().BeNull();
        }
    }

    internal static class VersionAssertExtensions
    {
        public static void VersionShouldBe(this IVersionInfo info, int major, int minor)
        {
            info.Should().Be(new VersionInfo(major, minor));
        }
    }
}
