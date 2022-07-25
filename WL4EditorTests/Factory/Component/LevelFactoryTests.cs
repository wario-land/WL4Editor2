using Moq;
using System.Collections.Immutable;
using WL4EditorCore.Factory.Component;
using WL4EditorCore.Interfaces.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorTests.Factory.Component
{
    [TestClass]
    public class LevelFactoryTests : TestBase
    {
        private static ImmutableArray<byte> testData;

        #region Test Setup
        [ClassInitialize]
        public static void ClassInit(TestContext tc)
        {
            testData = ImmutableArray.Create(TestData.ConstructTestLevelData());
        }

        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(testData);
            Mocks.MockRoomFactory.Setup(a => a.CreateRoom(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<uint>())).Returns(new Mock<IRoom>().Object);
            Mocks.MockDoorFactory.Setup(a => a.CreateDoor(It.IsAny<int>())).Returns(new Mock<IDoor>().Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mocks.MockRomDataProvider.Invocations.Clear();
            Mocks.MockRoomFactory.Invocations.Clear();
            Mocks.MockDoorFactory.Invocations.Clear();
            TestClassSynchronizationLock.ReleaseMutex();
        }
        #endregion

        #region Valid Tests
        [TestMethod]
        [Description("Test valid creation of a Level object")]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel)]
        public void Test_CreateLevel_Valid(Passage passage, Stage stage)
        {
            var level = new LevelFactory().CreateLevel(passage, stage);
            Assert.IsNotNull(level);
            Assert.AreEqual(passage, level.Passage);
            Assert.AreEqual(stage, level.Stage);
        }
        #endregion
    }
}
