using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Interfaces.Component
{
    public interface ILevel
    {
        public Passage Passage { get; }
        public Stage Stage { get; }
        public string? LevelNameEN { get; }
        public string? LevelNameJP { get; }
        public uint HardTimerSeconds { get; }
        public uint NormalTimerSeconds { get; }
        public uint SuperHardTimerSeconds { get; }
        public IList<IRoom> Rooms { get; }
        public IList<IDoor> Doors { get; }
    }
}
