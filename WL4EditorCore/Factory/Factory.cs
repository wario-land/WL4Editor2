using WL4EditorCore.Factory.Component;
using WL4EditorCore.Factory.Interfaces;

namespace WL4EditorCore.Factory
{
    public class Factory
    {
        public static IFactory Instance { get; private set; }

        public Factory(IFactory instance = null)
        {
            if(instance == null)
            {
                instance = new FactoryInternal();
            }
            Instance = instance;
        }

        private class FactoryInternal : IFactory
        {
            public FactoryInternal()
            {
                this.LayerFactory = new LayerFactory();
                this.LevelFactory = new LevelFactory();
                this.Map16TileFactory = new Map16TileFactory();
                this.RoomFactory = new RoomFactory();
                this.Tile8x8Factory = new Tile8x8Factory();
            }

            public ILayerFactory LayerFactory { get; private set; }
            public ILevelFactory LevelFactory { get; private set; }
            public IMap16TileFactory Map16TileFactory { get; private set; }
            public IRoomFactory RoomFactory { get; private set; }
            public ITile8x8Factory Tile8x8Factory { get; private set; }
        }
    }
}