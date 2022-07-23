using System.Collections.Immutable;
using WL4EditorCore.Util;

namespace WL4EditorTests.Util
{
    [TestClass]
    public class RomDataProviderTests : TestBase
    {
        private static readonly RomDataProvider Provider = new();

        [TestMethod]
        [Description("Test a simple case of reading a file and verifying its contents match the expected payload")]
        public void Test_Init_Valid()
        {
            var dataFile = $"{TestDataDirectory}\\Compression\\Compressed\\60A1D9.bin";
            var expected = ImmutableArray.Create(StringToByteArray("028400FF0000028400630000"));
            Provider.LoadDatafromFile(dataFile);
            CollectionAssert.AreEqual(expected, Provider.Data());
        }

        [TestMethod]
        [Description("Test to make sure that FileNotFoundException is thrown if the file does not exist")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Test_Init_Invalid()
        {
            Provider.LoadDatafromFile("gggggggggggg");
        }
    }
}