using WL4EditorCore.Component;
using WL4EditorCore.Interfaces.Component;
using WL4EditorCore.Interfaces.Factory;

namespace WL4EditorCore.Factory.Component
{
    public class CameraControlFactory : ICameraControlFactory
    {
        public ICameraControl CreateCameraControl(int cameraControlDataAddress) => new CameraControl(cameraControlDataAddress);
    }
}
