using System.Collections.Immutable;
using WL4EditorCore.Factory.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorTests.Factory.Component
{
    [TestClass]
    public class LevelFactoryTests : TestBase
    {
        private static readonly byte[] TestData = StringToByteArray("0001020304050607");
        private readonly LevelFactory LevelFactory = new();

        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(ImmutableArray.Create(TestData));
            // TODO setup mocking for DoorFactory and RoomFactory once they are implemented
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mocks.MockRomDataProvider.Invocations.Clear();
            TestClassSynchronizationLock.ReleaseMutex();
        }

        [TestMethod]
        [Description("Test valid creation of a Level object")]
        [DataRow(0, 0)]
        public void Test_CreateLevel_Valid(Passage passage, Stage stage)
        {
            var level = LevelFactory.CreateLevel(passage, stage);
            Assert.IsNotNull(level);

            // TODO implement
        }

        [TestMethod]
        [Description("Test to make sure that exception is thrown if the passage or stage are out of range of their enums")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [DataRow(-1, 0)]
        [DataRow(6, 0)]
        [DataRow(0, -1)]
        [DataRow(0, 5)]
        public void Test_CreateLevel_Invalid_IndexOOB(int passage, int stage)
        {
            LevelFactory.CreateLevel((Passage)passage, (Stage)stage);
        }

        [TestMethod]
        [Description("Test to make sure that exception is thrown if the passage and stage combo is invalid")]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow(Passage.EntryPassage, Stage.SecondLevel)]
        [DataRow(Passage.EntryPassage, Stage.FourthLevel)]
        [DataRow(Passage.GoldenPassage, Stage.SecondLevel)]
        [DataRow(Passage.GoldenPassage, Stage.ThirdLevel)]
        [DataRow(Passage.GoldenPassage, Stage.FourthLevel)]
        public void Test_CreateLevel_Invalid_BadStage(Passage passage, Stage stage)
        {
            LevelFactory.CreateLevel(passage, stage);
        }
    }
}