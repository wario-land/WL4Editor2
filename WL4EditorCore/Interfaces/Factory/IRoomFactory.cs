using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface IRoomFactory
    {
        IRoom CreateRoom(int roomDataAddress, int roomIndex, uint levelID);
    }
}
