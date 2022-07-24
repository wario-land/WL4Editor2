using WL4EditorCore.Component;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Interfaces.Factory;

namespace WL4EditorCore.Factory.Component
{
    public class RoomFactory : IRoomFactory
    {
        /// <inheritdoc />
        public IRoom CreateRoom(int roomDataAddress) => new Room(roomDataAddress);
    }
}
