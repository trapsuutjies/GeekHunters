using NUnit.Framework;
using GeekHunterASP;
using System.Web.UI.WebControls;

namespace GeekHunterASP.Tests
{
    [TestFixture()]
    public class _DefaultTests
    {
        [Test()]
        public void SanitizeNamesTest()
        {
            _Default page = new _Default();
            TextBox testBox = new TextBox();
            testBox.Text = "l33t sp33k";
            page.SanitizeNames(testBox, null);
            Assert.AreEqual("lt spk", testBox.Text);
        }
    }
}