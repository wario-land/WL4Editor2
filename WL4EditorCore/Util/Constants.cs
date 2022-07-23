namespace WL4EditorCore.Util
{
    public class Constants
    {
        public enum LevelDifficulty
        {
            NormalDifficulty = 0,
            HardDifficulty = 1,
            SHardDifficulty = 2
        };

        public enum Passage
        {
            EntryPassage = 0,
            EmeraldPassage = 1,
            RubyPassage = 2,
            TopazPassage = 3,
            SapphirePassage = 4,
            GoldenPassage = 5
        };

        public enum Stage
        {
            FirstLevel = 0,
            SecondLevel = 1,
            ThirdLevel = 2,
            FourthLevel = 3,
            BossLevel = 4
        };

        public const int TilesetDataTable = 0x3F2298;
        public const int LevelHeaderTable = 0x639068;
        public const int LevelHeaderIndexTable = 0x6391C4;
        public const int LevelNameENPointerTable = 0x63A3AC;
        public const int LevelNameJPPointerTable = 0x63A31C;
        public const int DoorTable = 0x78F21C;
        public const int RoomDataTable = 0x78F280;
        public const int CameraControlPointerTable = 0x78F540;
        public const int EntitySetInfoPointerTable = 0x78EF78;
        public const int EntityTilesetPointerTable = 0x78EBF0;
        public const int EntityPalettePointerTable = 0x78EDB4;
        public const int EntityTilesetLengthTable = 0x3B2C90;
        public const int AnimatedTileIdTableSwitchOff = 0x3F8098;
        public const int AnimatedTileIdTableSwitchOn = 0x3F91D8;
        public const int AnimatedTileSwitchInfoTable = 0x3F8C18;

        public const int CameraRecordSentinel = 0x3F9D58;
        public const int SpritesBasicElementTiles = 0x400AE8; // 0x3000 bytes in length
        public const int BGLayerDefaultPtr = 0x58DA7C;
        public const int NormalLayerDefaultPtr = 0x3F2263;
        public const int ToxicLandfillDustyLayer0Ptr = 0x601854;
        public const int FieryCavernDustyLayer0Ptr = 0x60D934;
        public const int UniversalSpritesPalette = 0x556DDC;
        public const int UniversalSpritesPalette2 = 0x400A68;
        public const int TreasureBoxGFXTiles = 0x352CF0;
        public const int CreditsTiles = 0x789FCC;

        public const int FreeSpaceStart = 0x78F970;
    }
}
