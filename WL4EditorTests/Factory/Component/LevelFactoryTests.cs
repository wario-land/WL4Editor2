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
        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
            var testData = ImmutableArray.Create(TestData.ConstructTestLevelData());
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(testData);
            Mocks.MockRoomFactory.Setup(a => a.CreateRoom(It.IsAny<uint>())).Returns(new Mock<IRoom>().Object);
            Mocks.MockDoorFactory.Setup(a => a.CreateDoor(It.IsAny<uint>())).Returns(new Mock<IDoor>().Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mocks.MockRomDataProvider.Invocations.Clear();
            Mocks.MockRoomFactory.Invocations.Clear();
            Mocks.MockDoorFactory.Invocations.Clear();
            TestClassSynchronizationLock.ReleaseMutex();
        }

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
    }
}
