using System.Text;
using WL4EditorCore.Exception;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Util;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Component
{
    public class Level : ILevel
    {
        public Passage Passage { get; private set; }
        public Stage Stage { get; private set; }
        public uint LevelID { get; private set; }
        public string? LevelNameEN { get; private set; }
        public string? LevelNameJP { get; private set; }
        public uint HardTimerSeconds { get; private set; }
        public uint NormalTimerSeconds { get; private set; }
        public uint SuperHardTimerSeconds { get; private set; }
        public IList<IRoom> Rooms { get; private set; } = new List<IRoom>();
        public IList<IDoor> Doors { get; private set; } = new List<IDoor>();

        // Text data used in conversion between strings and the data format used by WL4
        private readonly string _textData = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.&あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんぁぃぅぇぉゃゅょっがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンァィゥェォャュョッガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポヴ'、。—~…!?()「」『』[]℃-";

        /// <summary>
        /// Construct a level object from specified passage and stage.
        /// </summary>
        /// <param name="passage">The passage from which to construct a level.</param>
        /// <param name="stage">The stage from which to construct a level.</param>
        /// <returns>The Level object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the passage or stage are outside the range of possible values.</exception>
        /// <exception cref="ArgumentException">If the passage and stage are within valid range, but their combination references an invalid stage.</exception>
        public Level(Passage passage, Stage stage)
        {
            ValidatePassageAndStage(passage, stage);
            this.Passage = passage;
            this.Stage = stage;
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Level.<ctor>)");

            int levelHeaderIndex = (int)GBAUtils.GetIntValue(LevelHeaderIndexTable + (int)passage * 24 + (int)stage * 4);
            int levelHeaderPointer = LevelHeaderTable + levelHeaderIndex * 12;

            this.LevelID = data[levelHeaderPointer];
            InitializeLevelNames(passage, stage);
            InitializeLevelTimers(levelHeaderPointer);
            InitializeDoors();
            InitializeRooms(levelHeaderPointer);
        }

        private static void ValidatePassageAndStage(Passage passage, Stage stage)
        {
            if (!Enum.IsDefined(passage))
            {
                throw new ArgumentOutOfRangeException($"Passage out of range: {passage}");
            }
            if (!Enum.IsDefined(stage))
            {
                throw new ArgumentOutOfRangeException($"Stage out of range: {stage}");
            }
            bool[][] _valid = new bool[][]
            {
                new bool[] { true, false, true , false, true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, false, false, false, true }
            };
            if (!_valid[(int)passage][(int)stage])
            {
                throw new ArgumentException($"Invalid passage/stage selection. Passage: {passage} Stage: {stage}");
            }
        }

        private void InitializeLevelNames(Passage passage, Stage stage)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Level.InitializeLevelNames)");
            Func<int, string> GetLevelName = (offset) =>
            {
                StringBuilder sb = new();
                for (int i = 0; i < 26; ++i)
                {
                    var c = data[offset + i];
                    sb.Append(c < _textData.Length ? _textData[c] : ' ');
                }
                return sb.ToString().Trim();
            };
            var offsetEN = GBAUtils.GetPointer(LevelNameENPointerTable + (int)passage * 24 + (int)stage * 4);
            this.LevelNameEN = GetLevelName(offsetEN);
            var offsetJP = GBAUtils.GetPointer(LevelNameJPPointerTable + (int)passage * 24 + (int)stage * 4);
            this.LevelNameJP = GetLevelName(offsetJP);
        }

        private void InitializeLevelTimers(int levelHeaderPointer)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Level.InitializeLevelTimers)");
            Func<int, uint> GetTimerSeconds = (offset) =>
            {
                var a = data[offset];
                var b = data[offset + 1];
                var c = data[offset + 2];
                return (uint)(a * 60 + b * 10 + c);
            };
            this.HardTimerSeconds = GetTimerSeconds(levelHeaderPointer + 3);
            this.NormalTimerSeconds = GetTimerSeconds(levelHeaderPointer + 6);
            this.SuperHardTimerSeconds = GetTimerSeconds(levelHeaderPointer + 9);
        }

        private void InitializeDoors()
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Level.InitializeDoors)");
            var doorStartAddress = GBAUtils.GetPointer(DoorTable + (int)this.LevelID * 4);
            var currentDoorIndex = 0;
            int doorPointer;
            while (data[doorPointer = doorStartAddress + currentDoorIndex * 12] != 0)
            {
                this.Doors.Add(Singleton.Instance.DoorFactory.CreateDoor(doorPointer));
                ++currentDoorIndex;
            }
            if(currentDoorIndex == 0)
            {
                throw new DataException($"Level ({Passage}, {Stage}) has bad Door data (0 doors)");
            }
        }

        private void InitializeRooms(int levelHeaderPointer)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Level.InitializeLevelTimers)");
            int roomCount = data[levelHeaderPointer + 1];
            var roomTableAddress = GBAUtils.GetPointer(RoomDataTable + (int)this.LevelID * 4);
            for (int i = 0; i < roomCount; ++i)
            {
                int roomDataAddress = roomTableAddress + i * 0x2C;
                this.Rooms.Add(Singleton.Instance.RoomFactory.CreateRoom(roomDataAddress, i, this.LevelID));
            }
        }
    }
}
