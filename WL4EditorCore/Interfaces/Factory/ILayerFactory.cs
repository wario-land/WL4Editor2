using WL4EditorCore.Interfaces.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface ILayerFactory
    {
        ILayer CreateLayer(int layerDataAddress, LayerMappingType mappingType);
    }
}
