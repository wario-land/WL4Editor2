using WL4EditorCore.Interfaces.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface ILevelFactory
    {
        /// <summary>
        /// Create a level object from specified passage and stage.
        /// </summary>
        /// <param name="passage">The passage from which to generate a level.</param>
        /// <param name="stage">The stage from which to generate a level.</param>
        /// <returns>The Level object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the passage or stage are outside the range of possible values.</exception>
        /// <exception cref="ArgumentException">If the passage and stage combo are within valid range, but reference an invalid stage.</exception>
        public ILevel CreateLevel(Passage passage, Stage stage);
    }
}