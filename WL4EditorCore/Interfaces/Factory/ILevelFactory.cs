using WL4EditorCore.Interfaces.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface ILevelFactory
    {
        /// <summary>
        /// Create a level object from specified passage and stage. <see cref="WL4EditorCore.Component.Level.Level(Passage, Stage)"/>
        /// </summary>
        /// <param name="passage">The passage from which to generate a level.</param>
        /// <param name="stage">The stage from which to generate a level.</param>
        /// <returns>The Level object.</returns>
        public ILevel CreateLevel(Passage passage, Stage stage);
    }
}
