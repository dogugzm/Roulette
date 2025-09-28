using System;

public interface IWheelController
{
    void Spin();
    event Action<int> OnSpinComplete;
}