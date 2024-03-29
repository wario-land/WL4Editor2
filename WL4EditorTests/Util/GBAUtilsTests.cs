using System.Collections.Immutable;
using WL4EditorCore.Util;

namespace WL4EditorTests.Util
{
    [TestClass]
    public class GBAUtilsTests : TestBase
    {
        private static readonly byte[] TestData = StringToByteArray("8001020304050607");

        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(ImmutableArray.Create(TestData));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mocks.MockRomDataProvider.Invocations.Clear();
            TestClassSynchronizationLock.ReleaseMutex();
        }

        [TestMethod]
        [DataRow(0u, 0x8001u)]
        [DataRow(1u, 0x0102u)]
        [DataRow(2u, 0x0203u)]
        [DataRow(3u, 0x0304u)]
        [DataRow(4u, 0x0405u)]
        [DataRow(5u, 0x0506u)]
        [DataRow(6u, 0x0607u)]
        public void Test_GetShortValue_Valid(uint offset, uint expected)
        {
            var actual = GBAUtils.GetShortValue(offset);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0u, 0x80010203u)]
        [DataRow(1u, 0x01020304u)]
        [DataRow(2u, 0x02030405u)]
        [DataRow(3u, 0x03040506u)]
        [DataRow(4u, 0x04050607u)]
        public void Test_GetIntValue_Valid(uint offset, uint expected)
        {
            var actual = GBAUtils.GetIntValue(offset);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0u, 0x00010203u)]
        public void Test_GetPointerValue_Valid(uint offset, uint expected)
        {
            var actual = GBAUtils.GetPointer(offset);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_GetShortValue_Invalid_OffsetOOB() => GBAUtils.GetShortValue(7);

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_GetIntValue_Invalid_OffsetOOB() => GBAUtils.GetIntValue(5);
    }
}