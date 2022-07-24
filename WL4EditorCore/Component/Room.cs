using WL4EditorCore.Exception;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Util;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Component
{
    public class Room : IRoom
    {
        public byte TilesetID { get; private set; }
        public uint EntityTableHard { get; private set; }
        public uint EntityTableNormal { get; private set; }
        public uint EntityTableSuperHard { get; private set; }
        public ushort BGMVolume { get; private set; }
        public byte WaterDistanceFromTop { get; private set; }
        public CameraControlType CameraControlType { get; private set; }
        public LayerSpecialEffect LayerSpecialEffect { get; private set; }
        public LayerPriority LayerPriority { get; private set; }
        public AlphaBlending AlphaBlending { get; private set; }
        public LayerScrollingType LayerScrollingType { get; private set; }
        public ITileset? Tileset { get; private set; }
        public IList<ILayer> Layers { get; private set; } = new List<ILayer>();

        /// <summary>
        /// Construct an instance of Room.
        /// </summary>
        /// <param name="roomDataAddress">Address of the room header</param>
        public Room(int roomDataAddress)
        {
            InitializeProperties(roomDataAddress);
            InitializeLayers(roomDataAddress);
            InitializeCameraData(roomDataAddress);
            InitializeEntitySets(roomDataAddress);
        }

        private void InitializeProperties(int roomDataAddress)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Room.InitializeProperties)");

            this.TilesetID = data[roomDataAddress];
            this.EntityTableHard = GBAUtils.GetIntValue(roomDataAddress + 28);
            this.EntityTableNormal = GBAUtils.GetIntValue(roomDataAddress + 32);
            this.EntityTableSuperHard = GBAUtils.GetIntValue(roomDataAddress + 36);
            this.BGMVolume = GBAUtils.GetShortValue(roomDataAddress + 42);
            this.WaterDistanceFromTop = data[roomDataAddress + 41];

            this.CameraControlType = (CameraControlType)data[roomDataAddress + 24];
            this.LayerSpecialEffect = (LayerSpecialEffect)data[roomDataAddress + 25];
            var priorityAndAlphaAttribute = data[roomDataAddress + 26];
            this.LayerPriority = (LayerPriority)(priorityAndAlphaAttribute & 3);
            this.AlphaBlending = (AlphaBlending)((priorityAndAlphaAttribute >> 2) - 1);
            this.LayerScrollingType = (LayerScrollingType)data[roomDataAddress + 40];

            this.Tileset = Singleton.Instance.TilesetFactory.CreateTileset(this.TilesetID);
        }

        private void InitializeLayers(int roomDataAddress)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Room.InitializeLayers)");

            for (int i = 0; i < 4; ++i)
            {
                var mappingType = (LayerMappingType)(data[roomDataAddress + i + 1] & 0x30);
                var layerDataAddress = GBAUtils.GetPointer(roomDataAddress + i * 4 + 8);
                this.Layers.Add(Singleton.Instance.LayerFactory.CreateLayer(layerDataAddress, mappingType));
            }
        }

        private void InitializeCameraData(int roomDataAddress)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Room.InitializeCameraData)");

            // TODO
        }

        private void InitializeEntitySets(int roomDataAddress)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Room.InitializeEntitySets)");

            // TODO
        }
    }
}
