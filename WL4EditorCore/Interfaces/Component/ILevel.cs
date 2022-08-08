using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Interfaces.Component
{
    public interface ILevel
    {
        Passage Passage { get; }
        Stage Stage { get; }
        string? LevelNameEN { get; }
        string? LevelNameJP { get; }
        uint HardTimerSeconds { get; }
        uint NormalTimerSeconds { get; }
        uint SuperHardTimerSeconds { get; }
        IList<IRoom> Rooms { get; }
        IList<IDoor> Doors { get; }
    }
}
