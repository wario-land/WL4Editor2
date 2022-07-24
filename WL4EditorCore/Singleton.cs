using WL4EditorCore.Factory.Component;
using WL4EditorCore.Interfaces;
using WL4EditorCore.Interfaces.Factory;
using WL4EditorCore.Interfaces.Util;

namespace WL4EditorCore
{
    public class Singleton
    {
        public static ISingleton? Instance { get; private set; }

        public static void InitSingleton(ISingleton? instance = null)
        {
            if(instance == null)
            {
                instance = new FactoryInternal();
            }
            Instance = instance;
        }

        private class FactoryInternal : ISingleton
        {
            public IDoorFactory DoorFactory { get; } = new DoorFactory();
            public ILayerFactory LayerFactory { get; } = new LayerFactory();
            public ILevelFactory LevelFactory { get; } = new LevelFactory();
            public IMap16TileFactory Map16TileFactory { get; } = new Map16TileFactory();
            public IRomDataProvider RomDataProvider { get; } = new RomDataProvider();
            public IRoomFactory RoomFactory { get; } = new RoomFactory();
            public ITile8x8Factory Tile8x8Factory { get; } = new Tile8x8Factory();
            public ITilesetFactory TilesetFactory { get; } = new TilesetFactory();
        }
    }
}
