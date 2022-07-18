using WL4EditorCore.Factory.Interfaces;

namespace WL4EditorCore.Factory
{
    public interface IFactory
    {
        ILayerFactory LayerFactory { get; }
        ILevelFactory LevelFactory { get; }
        IMap16TileFactory Map16TileFactory { get; }
        IRoomFactory RoomFactory { get; }
        ITile8x8Factory Tile8x8Factory { get; }
    }
}