using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface IDoorFactory
    {
        IDoor CreateDoor(int doorDataAddress);
    }
}
