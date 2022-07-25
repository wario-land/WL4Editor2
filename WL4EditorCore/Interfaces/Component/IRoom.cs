using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Interfaces.Component
{
    public interface IRoom
    {
        byte TilesetID { get; }
        ushort BGMVolume { get; }
        byte WaterDistanceFromTop { get; }
        CameraControlType CameraControlType { get; }
        LayerSpecialEffect LayerSpecialEffect { get; }
        LayerPriority LayerPriority { get; }
        AlphaBlending AlphaBlending { get; }
        LayerScrollingType LayerScrollingType { get; }
        ITileset? Tileset { get; }
        IList<ILayer>? Layers { get; }
        IList<ICameraControl> CameraControls { get; }
        IList<IEntitySet> EntitySets { get; }
    }
}
