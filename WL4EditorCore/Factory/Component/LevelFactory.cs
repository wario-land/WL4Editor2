using WL4EditorCore.Component;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Interfaces.Factory;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Factory.Component
{
    public class LevelFactory : ILevelFactory
    {
        /// <inheritdoc />
        public ILevel CreateLevel(Passage passage, Stage stage) => new Level(passage, stage);
    }
}
