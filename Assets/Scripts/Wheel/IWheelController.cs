using System;

namespace Wheel
{
    public interface IWheelController
    {
        void Spin();
        event Action<int> OnSpinComplete;
    }
}