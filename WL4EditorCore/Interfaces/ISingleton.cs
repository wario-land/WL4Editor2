using WL4EditorCore.Interfaces.Factory;
using WL4EditorCore.Interfaces.Util;

namespace WL4EditorCore.Interfaces
{
    public interface ISingleton
    {
        IDoorFactory DoorFactory { get; }
        ILayerFactory LayerFactory { get; }
        ILevelFactory LevelFactory { get; }
        IMap16TileFactory Map16TileFactory { get; }
        IRomDataProvider RomDataProvider { get; }
        IRoomFactory RoomFactory { get; }
        ITile8x8Factory Tile8x8Factory { get; }
        ITilesetFactory TilesetFactory { get; }
    }
}
