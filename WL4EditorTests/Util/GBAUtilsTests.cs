using WL4EditorCore.Util;

namespace WL4EditorTests.Util
{
    [TestClass]
    public class GBAUtilsTests
    {
        private static byte[] TestData = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };

        [TestMethod]
        [DataRow(0u, 0x0001u)]
        [DataRow(1u, 0x0102u)]
        [DataRow(2u, 0x0203u)]
        [DataRow(3u, 0x0304u)]
        [DataRow(4u, 0x0405u)]
        [DataRow(5u, 0x0506u)]
        [DataRow(6u, 0x0607u)]
        public void Test_GetShortValue_Valid(uint offset, uint expected)
        {
            var actual = GBAUtils.GetShortValue(TestData, offset);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0u, 0x00010203u)]
        [DataRow(1u, 0x01020304u)]
        [DataRow(2u, 0x02030405u)]
        [DataRow(3u, 0x03040506u)]
        [DataRow(4u, 0x04050607u)]
        public void Test_GetIntValue_Valid(uint offset, uint expected)
        {
            var actual = GBAUtils.GetIntValue(TestData, offset);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_GetShortValue_Invalid_NullData() => GBAUtils.GetShortValue(null, 0);

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_GetShortValue_Invalid_OffsetOOB() => GBAUtils.GetShortValue(TestData, 7);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_GetIntValue_Invalid_NullData() => GBAUtils.GetIntValue(null, 0);

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_GetIntValue_Invalid_OffsetOOB() => GBAUtils.GetIntValue(TestData, 5);
    }
}