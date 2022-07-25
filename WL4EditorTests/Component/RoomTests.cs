using Moq;
using System.Collections.Immutable;
using WL4EditorCore.Interfaces.Component;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorTests.Component
{
    [TestClass]
    public class RoomTests : TestBase
    {
        private static ImmutableArray<byte> testData;
        private IList<MethodInvocation> Invocations = new List<MethodInvocation>();

        #region Test Setup
        [ClassInitialize]
        public static void ClassInit(TestContext tc)
        {
            testData = ImmutableArray.Create(TestData.ConstructTestRoomData());
        }

        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(testData);
            Mocks.MockLayerFactory.Setup(a => a.CreateLayer(It.IsAny<int>(), It.IsAny<LayerMappingType>()))
                .Callback<int, LayerMappingType>((a, b) => Invocations.Add(new MethodInvocation("CreateLayer", a, b)))
                .Returns(new Mock<ILayer>().Object);
            Mocks.MockCameraControlFactory.Setup(a => a.CreateCameraControl(It.IsAny<int>()))
                .Callback<int>(a => Invocations.Add(new MethodInvocation("CreateCameraControl", a)))
                .Returns(new Mock<ICameraControl>().Object);
            Mocks.MockEntitySetFactory.Setup(a => a.CreateEntitySet(It.IsAny<int>()))
                .Callback<int>(a => Invocations.Add(new MethodInvocation("CreateEntitySet", a)))
                .Returns(new Mock<IEntitySet>().Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mocks.MockRomDataProvider.Invocations.Clear();
            Mocks.MockLayerFactory.Invocations.Clear();
            Mocks.MockCameraControlFactory.Invocations.Clear();
            Mocks.MockEntitySetFactory.Invocations.Clear();
            TestClassSynchronizationLock.ReleaseMutex();
        }
        #endregion
    }
}
