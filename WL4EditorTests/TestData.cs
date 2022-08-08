using static WL4EditorCore.Util.Constants;

namespace WL4EditorTests
{
    internal class TestData : TestBase
    {
        // Construct test ROM data that will load 2 levels
        public static byte[] ConstructTestLevelData()
        {
            var data = new byte[FreeSpaceStart];

            // Level (0, 0) - 1 room, 1 door
            var levelHeaderIndex1 = StringToByteArray("00000000");
            levelHeaderIndex1.CopyTo(data, LevelHeaderIndexTable);
            var levelHeader1 = StringToByteArray("00 01 0A 070102 080304 090506");
            levelHeader1.CopyTo(data, LevelHeaderTable);

            var nameENPointer1 = StringToByteArray("08000000");
            nameENPointer1.CopyTo(data, LevelNameENPointerTable);
            var nameEN1 = StringToByteArray("FFFFFFFFFFFFFF 1D283637 FF 152839282F FF 01 FFFFFFFFFFFFFF"); // Test Level 1
            nameEN1.CopyTo(data, 0);
            var nameJPPointer1 = StringToByteArray("0800001A");
            nameJPPointer1.CopyTo(data, LevelNameJPPointerTable);
            var nameJP1 = StringToByteArray("FFFFFFFFFFFFFFFF A29CA3 FF B9D9B8 FF 01 FFFFFFFFFFFFFFFFFF"); // テスト レベル 1
            nameJP1.CopyTo(data, 0x1A);

            // Level (3, 2) - 3 rooms, 3 doors
            var levelHeaderIndex2 = StringToByteArray("00000001");
            levelHeaderIndex2.CopyTo(data, LevelHeaderIndexTable + 80);
            var levelHeader2 = StringToByteArray("01 03 0A 040107 050208 060309");
            levelHeader2.CopyTo(data, LevelHeaderTable + 12);

            var nameENPointer2 = StringToByteArray("08000034");
            nameENPointer2.CopyTo(data, LevelNameENPointerTable + 80);
            var nameEN2 = StringToByteArray("FFFFFFFFFFFFFF 1D283637 FF 152839282F FF 02 FFFFFFFFFFFFFF"); // Test Level 2
            nameEN2.CopyTo(data, 0x34);
            var nameJPPointer2 = StringToByteArray("0800004E");
            nameJPPointer2.CopyTo(data, LevelNameJPPointerTable + 80);
            var nameJP2 = StringToByteArray("FFFFFFFFFFFFFFFF A29CA3 FF B9D9B8 FF 02 FFFFFFFFFFFFFFFFFF"); // テスト レベル 2
            nameJP2.CopyTo(data, 0x4E);

            // Room data
            var roomPointers = StringToByteArray("0800000008000000"); // dummy addresses since rooms are not loaded for the Level unit tests
            roomPointers.CopyTo(data, RoomDataTable);

            // Door data
            var doorPointer1 = StringToByteArray("08000068");
            doorPointer1.CopyTo(data, DoorTable);
            var doorData1 = StringToByteArray("010000000000000000000000 000000000000000000000000"); // dummy data since door implementation details are not tested in Level unit tests
            doorData1.CopyTo(data, 0x68);
            var doorPointer2 = StringToByteArray("08000080");
            doorPointer2.CopyTo(data, DoorTable + 4);
            var doorData2 = StringToByteArray("010000000000000000000000 020000000000000000000000 020000000000000000000000 000000000000000000000000");
            doorData2.CopyTo(data, 0x80);

            return data;
        }

        // Construct test ROM data that will load a room
        public static byte[] ConstructTestRoomData()
        {
            var data = new byte[FreeSpaceStart];

            // Room (0, 0, 0) - 1 room in level, no camera control
            var roomHeader1 = StringToByteArray("02 10101020 000000 08598EEC 085991DC 08599454 085FA6D0 010107 00 085991D0 08599448 08599600 00FF 0100");
            roomHeader1.CopyTo(data, 31);

            // Room (1, 1, 1) - 2 rooms in level, camera control boxes in both rooms, 2 camera control boxes in 2nd room
            var roomHeader2 = StringToByteArray("03 10102000 000000 08598FEC 085992DC 08599554 085FA7D0 03020E 00 085992D0 08599548 08599700 0111 0080");
            roomHeader2.CopyTo(data, 1000);
            var pointerToLevelCameraPointerTable = StringToByteArray("08000100");
            pointerToLevelCameraPointerTable.CopyTo(data, CameraControlPointerTable + 4);
            var cameraControlPointerTable = StringToByteArray("08000000 0800000B 083F9D58");
            cameraControlPointerTable.CopyTo(data, 0x100);
            var cameraControlData = StringToByteArray("0102 021011121314150506 031112131415160607");
            cameraControlData.CopyTo(data, 0xB);

            return data;
        }
    }
}
