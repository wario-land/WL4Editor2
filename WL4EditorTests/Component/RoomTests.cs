using Moq;
using System.Collections.Immutable;
using WL4EditorCore.Component;
using WL4EditorCore.Exception;
using WL4EditorCore.Interfaces.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorTests.Component
{
    [TestClass]
    public class RoomTests : TestBase
    {
        private static ImmutableArray<byte> testData;
        private IList<MethodInvocation> Invocations = new List<MethodInvocation>();

        #region Test Setup
        [ClassInitialize]
        public static void ClassInit(TestContext tc)
        {
            testData = ImmutableArray.Create(TestData.ConstructTestRoomData());
        }

        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(testData);
            Mocks.MockLayerFactory.Setup(a => a.CreateLayer(It.IsAny<int>(), It.IsAny<LayerMappingType>()))
                .Callback<int, LayerMappingType>((a, b) => Invocations.Add(new MethodInvocation("CreateLayer", a, b)))
                .Returns(new Mock<ILayer>().Object);
            Mocks.MockCameraControlFactory.Setup(a => a.CreateCameraControl(It.IsAny<int>()))
                .Callback<int>(a => Invocations.Add(new MethodInvocation("CreateCameraControl", a)))
                .Returns(new Mock<ICameraControl>().Object);
            Mocks.MockEntitySetFactory.Setup(a => a.CreateEntitySet(It.IsAny<int>()))
                .Callback<int>(a => Invocations.Add(new MethodInvocation("CreateEntitySet", a)))
                .Returns(new Mock<IEntitySet>().Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mocks.MockRomDataProvider.Invocations.Clear();
            Mocks.MockLayerFactory.Invocations.Clear();
            Mocks.MockCameraControlFactory.Invocations.Clear();
            Mocks.MockEntitySetFactory.Invocations.Clear();
            TestClassSynchronizationLock.ReleaseMutex();
        }
        #endregion

        #region Valid Tests
        [TestMethod]
        [DataRow(31, 0, 0u, (byte)2)]
        [DataRow(1000, 1, 1u, (byte)3)]
        public void Test_Room_Init_Valid_TilesetID(int roomDataAddress, int roomIndex, uint levelID, byte expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.TilesetID);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, (ushort)0x100)]
        [DataRow(1000, 1, 1u, (ushort)0x80)]
        public void Test_Room_Init_Valid_BGMVolume(int roomDataAddress, int roomIndex, uint levelID, ushort expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.BGMVolume);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, (byte)0xFF)]
        [DataRow(1000, 1, 1u, (byte)0x11)]
        public void Test_Room_Init_Valid_WaterDistanceFromTop(int roomDataAddress, int roomIndex, uint levelID, byte expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.WaterDistanceFromTop);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, CameraControlType.FixedY)]
        [DataRow(1000, 1, 1u, CameraControlType.HasControlAttributes)]
        public void Test_Room_Init_Valid_CameraControlType(int roomDataAddress, int roomIndex, uint levelID, CameraControlType expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.CameraControlType);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, LayerSpecialEffect.Layer3WaterEffect1)]
        [DataRow(1000, 1, 1u, LayerSpecialEffect.Layer3WaterEffect2)]
        public void Test_Room_Init_Valid_LayerSpecialEffect(int roomDataAddress, int roomIndex, uint levelID, LayerSpecialEffect expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.LayerSpecialEffect);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, LayerPriority.Order_1203_SP)]
        [DataRow(1000, 1, 1u, LayerPriority.Order_1023_SP)]
        public void Test_Room_Init_Valid_LayerPriority(int roomDataAddress, int roomIndex, uint levelID, LayerPriority expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.LayerPriority);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, AlphaBlending.NoAlphaBlending)]
        [DataRow(1000, 1, 1u, AlphaBlending.EVA10_EVB16)]
        public void Test_Room_Init_Valid_AlphaBlending(int roomDataAddress, int roomIndex, uint levelID, AlphaBlending expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.AlphaBlending);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, LayerScrollingType.NoScrolling)]
        [DataRow(1000, 1, 1u, LayerScrollingType.HSpeedHalf)]
        public void Test_Room_Init_Valid_LayerScrollingType(int roomDataAddress, int roomIndex, uint levelID, LayerScrollingType expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.LayerScrollingType);
        }

        [TestMethod]
        [DataRow(31, 0, 0u)]
        [DataRow(1000, 1, 1u)]
        public void Test_Room_Init_Valid_LayersCount(int roomDataAddress, int roomIndex, uint levelID)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(4, room.Layers.Count);
        }

        [TestMethod]
        [DataRow(31, 0, 0u, 0)]
        [DataRow(1000, 1, 1u, 2)]
        public void Test_Room_Init_Valid_CameraControlsCount(int roomDataAddress, int roomIndex, uint levelID, int expected)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(expected, room.CameraControls.Count);
        }

        [TestMethod]
        [DataRow(31, 0, 0u)]
        [DataRow(1000, 1, 1u)]
        public void Test_Room_Init_Valid_CameraControlsCount(int roomDataAddress, int roomIndex, uint levelID)
        {
            var room = new Room(roomDataAddress, roomIndex, levelID);
            Assert.AreEqual(3, room.EntitySets.Count);
        }

        private static readonly MethodInvocation[][] CallbackData =
        {
            new MethodInvocation[]
            {
                new MethodInvocation("CreateLayer", 0x598EEC, LayerMappingType.LayerMap16),
                new MethodInvocation("CreateLayer", 0x5991DC, LayerMappingType.LayerMap16),
                new MethodInvocation("CreateLayer", 0x599454, LayerMappingType.LayerMap16),
                new MethodInvocation("CreateLayer", 0x5FA6D0, LayerMappingType.LayerTile8x8),
                new MethodInvocation("CreateEntitySet", 0x5991D0),
                new MethodInvocation("CreateEntitySet", 0x599448),
                new MethodInvocation("CreateEntitySet", 0x599600)
            },
            new MethodInvocation[]
            {
                new MethodInvocation("CreateLayer", 0x598FEC, LayerMappingType.LayerMap16),
                new MethodInvocation("CreateLayer", 0x5992DC, LayerMappingType.LayerMap16),
                new MethodInvocation("CreateLayer", 0x599554, LayerMappingType.LayerTile8x8),
                new MethodInvocation("CreateLayer", 0x5FA7D0, LayerMappingType.LayerDisabled),
                new MethodInvocation("CreateCameraControl", 0xD),
                new MethodInvocation("CreateCameraControl", 0x16),
                new MethodInvocation("CreateEntitySet", 0x5992D0),
                new MethodInvocation("CreateEntitySet", 0x599548),
                new MethodInvocation("CreateEntitySet", 0x599700)
            }
        };

        [TestMethod]
        [Description("Test to make sure the correct invocations are made to construct member objects")]
        [DataRow(31, 0, 0u, 0)]
        [DataRow(1000, 1, 1u, 1)]
        public void Test_CreateRoom_Valid_Invocations(int roomDataAddress, int roomIndex, uint levelID, int callbackDataIndex)
        {
            _ = new Room(roomDataAddress, roomIndex, levelID);
            var expected = CallbackData[callbackDataIndex];
            var actual = this.Invocations;
            Assert.AreEqual(expected.Length, actual.Count);
            Array.ForEach(expected, a => Assert.IsTrue(actual.Contains(a, a), $"Expected invocation \"{a}\" did not occur"));
        }
        #endregion

        #region Invalid Tests
        [TestMethod]
        [Description("Camera control failure: No camera controls found which correspond to room")]
        [ExpectedException(typeof(DataException))]
        public void Test_CreateRoom_Invalid_CameraControlAttributes()
        {
            new Room(1000, 4, 1u);
        }
        #endregion
    }
}
