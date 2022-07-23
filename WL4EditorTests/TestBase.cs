using Moq;
using System.Text.RegularExpressions;
using WL4EditorCore;
using WL4EditorCore.Interfaces;
using WL4EditorCore.Interfaces.Factory;
using WL4EditorCore.Interfaces.Util;

namespace WL4EditorTests
{
    [TestClass]
    public class TestBase
    {
        protected const string TestDataDirectory = "..\\..\\..\\TestData";
        protected static readonly MockSingleton Mocks = new();
        protected static readonly Mutex TestClassSynchronizationLock = new();

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext tc)
        {
            Singleton.InitSingleton(Mocks);
        }

        protected class MockSingleton : ISingleton
        {
            public Mock<ILayerFactory> MockLayerFactory = new();
            public Mock<IDoorFactory> MockDoorFactory = new();
            public Mock<ILevelFactory> MockLevelFactory = new();
            public Mock<IMap16TileFactory> MockMap16TileFactory = new();
            public Mock<IRomDataProvider> MockRomDataProvider = new();
            public Mock<IRoomFactory> MockRoomFactory = new();
            public Mock<ITile8x8Factory> MockTile8x8Factory = new();
            public IDoorFactory DoorFactory { get => MockDoorFactory.Object; }
            public ILayerFactory LayerFactory { get => MockLayerFactory.Object; }

            public ILevelFactory LevelFactory { get => MockLevelFactory.Object; }

            public IMap16TileFactory Map16TileFactory { get => MockMap16TileFactory.Object; }

            public IRomDataProvider RomDataProvider { get => MockRomDataProvider.Object; }

            public IRoomFactory RoomFactory { get => MockRoomFactory.Object; }

            public ITile8x8Factory Tile8x8Factory { get => MockTile8x8Factory.Object; }
        }

        protected static byte[] StringToByteArray(string input)
        {
            input = input.Replace(" ", string.Empty);
            var pattern = "^([0-9A-F]{2})*$";
            if (!Regex.IsMatch(input, pattern))
            {
                throw new Exception($"Incorrectly formatted test. Test data does not match pattern {pattern}");
            }
            var result = new byte[input.Length / 2];
            for (int i = 0; i < input.Length / 2; ++i)
            {
                result[i] = byte.Parse(input.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return result;
        }
    }
}