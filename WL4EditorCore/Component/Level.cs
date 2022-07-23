using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Component
{
    public class Level : ILevel
    {
        public uint LevelID;
        public string? LevelNameEN;
        public string? LevelNameJP;
        public IList<IRoom>? Rooms;
        public IList<IDoor>? Doors;
    }
}