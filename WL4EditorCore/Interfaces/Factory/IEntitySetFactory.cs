using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface IEntitySetFactory
    {
        IEntitySet CreateEntitySet(int entitySetAddress);
    }
}
