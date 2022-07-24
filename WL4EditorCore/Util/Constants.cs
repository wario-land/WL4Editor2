namespace WL4EditorCore.Util
{
    public class Constants
    {
        // Level enums
        public enum LevelDifficulty
        {
            NormalDifficulty = 0,
            HardDifficulty   = 1,
            SHardDifficulty  = 2
        };

        public enum Passage
        {
            EntryPassage    = 0,
            EmeraldPassage  = 1,
            RubyPassage     = 2,
            TopazPassage    = 3,
            SapphirePassage = 4,
            GoldenPassage   = 5
        };

        public enum Stage
        {
            FirstLevel  = 0,
            SecondLevel = 1,
            ThirdLevel  = 2,
            FourthLevel = 3,
            BossLevel   = 4
        };

        // Room enums
        public enum CameraControlType
        {
            FixedY              = 1,
            NoLimit             = 2,
            HasControlAttrs     = 3,
            VerticallySeperated = 4
        };

        public enum LayerSpecialEffect
        {
            NoEffect = 0,
            Layer3WaterEffect1 = 1,
            Layer3WaterEffect2 = 2,
            Layer0FogEffect = 3,
            Layer3FireEffect1 = 4,
            Layer3FireEffect2 = 5,
            Layer3HDMAUpperHalf = 6,
            Layer3HDMALowerHalf = 7,
            AlphaFireEffect1 = 8,
            AlphaFireEffect2 = 9
        }

        public enum LayerScrollingType
        {
            NoScrolling       = 0,
            HSpeedHalf        = 1,
            VSpeedHalf        = 2,
            HVSpeedHalf       = 3,
            HSyncVSpeedHalf   = 4,
            VSyncHSpeedHalf   = 5,
            HVSync            = 6,
            TopHalfAutoscroll = 7
        }
        
        public enum LayerPriority
        {
            Order_0123    = 0,
            Order_1023    = 1,
            Order_1023_SP = 2,
            Order_1203_SP = 3
        }

        public enum AlphaBlending
        {
            NoAlphaBlending = 0,
            EVA7_EVB16      = 1,
            EVA10_EVB16     = 2,
            EVA13_EVB16     = 3,
            EVA16_EVB16     = 4,
            EVA16_EVB0      = 5,
            EVA13_EVB3      = 6,
            EVA10_EVB6      = 7,
            EVA7_EVB9       = 8,
            EVA5_EVB11      = 9,
            EVA3_EVB13      = 10,
            EVA0_EVB16      = 11
        }

        // Layer enums
        public enum LayerMappingType
        {
            LayerDisabled = 0x00,
            LayerMap16    = 0x10,
            LayerTile8x8  = 0x20
        };

        // Addresses
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

        // Limits
        public const int TilesetCount = 92;
    }
}