using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface IRoomFactory
    {
        public IRoom CreateRoom(uint roomDataAddress);
    }
}
