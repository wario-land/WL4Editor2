using WL4EditorCore.Component;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Interfaces.Factory;

namespace WL4EditorCore.Factory.Component
{
    public class EntitySetFactory : IEntitySetFactory
    {
        public IEntitySet CreateEntitySet(int entitySetAddress) => new EntitySet(entitySetAddress);
    }
}
