using WL4EditorCore.Component;
using WL4EditorCore.Exception;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Interfaces.Factory;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Factory.Component
{
    public class LevelFactory : ILevelFactory
    {
        /// <inheritdoc />
        public ILevel CreateLevel(Passage passage, Stage stage)
        {
            ValidatePassageAndStage(passage, stage);
            if (Singleton.Instance == null)
            {
                throw new InternalException("Singleton not initialized (WL4EditorCore.Factory.Component.LevelFactory.<ctor>)");
            }
            var data = Singleton.Instance.RomDataProvider.Data();

            var level = new Level();

            // TODO populate Level object

            return level;
        }

        private void ValidatePassageAndStage(Passage passage, Stage stage)
        {
            if (passage < 0 || ((int)passage) >= Enum.GetNames(typeof(Passage)).Length)
            {
                throw new ArgumentOutOfRangeException($"Passage out of range: {passage}");
            }
            var tt = Enum.GetNames(typeof(Stage));
            if (stage < 0 || ((int)stage) >= Enum.GetNames(typeof(Stage)).Length)
            {
                throw new ArgumentOutOfRangeException($"Stage out of range: {stage}");
            }
            bool[][] _valid = new bool[][]
            {
                new bool[] { true, false, true , false, true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, true , true , true , true },
                new bool[] { true, false, false, false, true }
            };
            if (!_valid[(int)passage][(int)stage])
            {
                throw new ArgumentException($"Invalid passage/stage selection. Passage: {passage} Stage: {stage}");
            }
        }
    }
}