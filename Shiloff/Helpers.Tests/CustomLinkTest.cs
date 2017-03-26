using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Tests
{
    [TestClass]
    public class CustomLinkTest
    {

        private class Test
        {
            public string Property1 { get; set; }
            public List<object> Property2 { get; set; }
            public string Sort { get; set; }
            public string Ignore => "Ignore";
        }

        [TestMethod]
        public void ExtendedHtmlHelper_CreateCustomLink_Success()
        {
            //arrange

            //act
            var result = ExtendedHtmlHelper.CustomLink("link",
                () => new Test()
                {
                    Property1 = "prop1",
                    Property2 = new List<object> {"p21", 123},
                    Sort = "SortDesc"
                });

            //assert
            Assert.AreEqual("link?property1=prop1&property2=p21&property2=123&sort=sortdesc", result);
        }
    }
}
