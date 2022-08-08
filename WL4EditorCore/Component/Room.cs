using WL4EditorCore.Exception;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Util;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Component
{
    public class Room : IRoom
    {
        public byte TilesetID { get; private set; }
        public ushort BGMVolume { get; private set; }
        public byte WaterDistanceFromTop { get; private set; }
        public CameraControlType CameraControlType { get; private set; }
        public LayerSpecialEffect LayerSpecialEffect { get; private set; }
        public LayerPriority LayerPriority { get; private set; }
        public AlphaBlending AlphaBlending { get; private set; }
        public LayerScrollingType LayerScrollingType { get; private set; }
        public ITileset? Tileset { get; private set; }
        public IList<ILayer> Layers { get; private set; } = new List<ILayer>();
        public IList<ICameraControl> CameraControls { get; private set; } = new List<ICameraControl>();
        public IList<IEntitySet> EntitySets { get; private set; } = new List<IEntitySet>();

        /// <summary>
        /// Construct an instance of Room.
        /// </summary>
        /// <param name="roomDataAddress">Address of the room header</param>
        public Room(int roomDataAddress, int roomIndex, uint levelID)
        {
            InitializeProperties(roomDataAddress);
            InitializeLayers(roomDataAddress);
            InitializeCameraData(roomDataAddress, roomIndex, levelID);
            InitializeEntitySets(roomDataAddress);
        }

        private void InitializeProperties(int roomDataAddress)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Room.InitializeProperties)");

            this.TilesetID = data[roomDataAddress];
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

        private void InitializeCameraData(int roomDataAddress, int roomIndex, uint levelID)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Room.InitializeCameraData)");

            if(this.CameraControlType == CameraControlType.HasControlAttributes)
            {
                var levelCameraControlPointerTable = GBAUtils.GetPointer(CameraControlPointerTable + (int)levelID * 4);
                // Level camera control pointer table contains 1 pointer per room that uses HasControlAttributes,
                // up to 16 (the maximum possible room count for a level). The list is terminated with 0x3F9D58.
                for (int i = 0; i < 16; ++i)
                {
                    var cameraControlDataPointer = GBAUtils.GetPointer(levelCameraControlPointerTable + i * 4);
                    if(cameraControlDataPointer == CameraRecordSentinel)
                    {
                        break; // End of list, camera control data for this room not found
                    }
                    else if (data[cameraControlDataPointer] == roomIndex)
                    {
                        // Camera control data found for the current room
                        var controlCount = data[cameraControlDataPointer + 1];
                        for(int j = 0; j < controlCount; ++j)
                        {
                            var cameraControlDataAddress = cameraControlDataPointer + j * 9 + 2;
                            this.CameraControls.Add(Singleton.Instance.CameraControlFactory.CreateCameraControl(cameraControlDataAddress));
                        }
                        return;
                    }
                }
                throw new DataException($"Room has invalid camera data: Camera is of type \"HasControlAttributes\", but no camera controls found which correspond to room {roomIndex}");
            }
        }

        private void InitializeEntitySets(int roomDataAddress)
        {
            var data = Singleton.Instance?.RomDataProvider.Data() ?? throw new InternalException("Singleton not initialized (WL4EditorCore.Component.Room.InitializeEntitySets)");

            for(int i = 0; i < 3; ++i)
            {
                var entitySetAddress = GBAUtils.GetPointer(roomDataAddress + 28 + 4 * i);
                this.EntitySets.Add(Singleton.Instance.EntitySetFactory.CreateEntitySet(entitySetAddress));
            }
        }
    }
}
