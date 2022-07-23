using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface IDoorFactory
    {
        public IDoor CreateDoor(uint doorDataAddress);
    }
}
