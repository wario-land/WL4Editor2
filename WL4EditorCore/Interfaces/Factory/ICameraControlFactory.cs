using WL4EditorCore.Interfaces.Component;

namespace WL4EditorCore.Interfaces.Factory
{
    public interface ICameraControlFactory
    {
        ICameraControl CreateCameraControl(int cameraControlDataAddress);
    }
}
