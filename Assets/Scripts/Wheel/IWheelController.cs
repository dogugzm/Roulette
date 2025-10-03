using System;

namespace Wheel
{
    public interface IWheelController
    {
        void Spin(int? number);
        event Action<int> OnSpinComplete;
    }
}