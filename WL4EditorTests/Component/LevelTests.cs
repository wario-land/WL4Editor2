using Moq;
using System.Collections.Immutable;
using WL4EditorCore.Component;
using WL4EditorCore.Interfaces.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorTests.Component
{
    [TestClass]
    public class LevelTests : TestBase
    {
        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
            var testData = ImmutableArray.Create(TestData.ConstructTestLevelData());
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(testData);
            Mocks.MockRoomFactory.Setup(a => a.CreateRoom(It.IsAny<int>())).Returns(new Mock<IRoom>().Object);
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

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel)]
        public void Test_Level_Init_Valid_Passage(Passage passage, Stage stage)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(passage, level.Passage);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel)]
        public void Test_Level_Init_Valid_Stage(Passage passage, Stage stage)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(stage, level.Stage);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, 0u)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, 1u)]
        public void Test_Level_Init_Valid_LevelID(Passage passage, Stage stage, uint expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.LevelID);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, "Test Level 1")]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, "Test Level 2")]
        public void Test_Level_Init_Valid_LevelNameEN(Passage passage, Stage stage, string expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.LevelNameEN);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, "テスト レベル 1")]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, "テスト レベル 2")]
        public void Test_Level_Init_Valid_LevelNameJP(Passage passage, Stage stage, string expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.LevelNameJP);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, 432u)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, 257u)]
        public void Test_Level_Init_Valid_HardTimerSeconds(Passage passage, Stage stage, uint expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.HardTimerSeconds);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, 514u)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, 328u)]
        public void Test_Level_Init_Valid_NormalTimerSeconds(Passage passage, Stage stage, uint expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.NormalTimerSeconds);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, 596u)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, 399u)]
        public void Test_Level_Init_Valid_SuperHardTimerSeconds(Passage passage, Stage stage, uint expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.SuperHardTimerSeconds);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, 1)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, 3)]
        public void Test_Level_Init_Valid_RoomCount(Passage passage, Stage stage, int expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.Rooms.Count);
        }

        [TestMethod]
        [DataRow(Passage.EntryPassage, Stage.FirstLevel, 1)]
        [DataRow(Passage.TopazPassage, Stage.ThirdLevel, 3)]
        public void Test_Level_Init_Valid_DoorCount(Passage passage, Stage stage, int expected)
        {
            var level = new Level(passage, stage);
            Assert.AreEqual(expected, level.Doors.Count);
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
            new Level((Passage)passage, (Stage)stage);
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
            new Level(passage, stage);
        }
    }
}
