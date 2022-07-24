using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface ITilesetFactory
    {
        ITileset CreateTileset(int index);
    }
}
