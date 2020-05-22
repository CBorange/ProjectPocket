using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface InputSystem
{
    void FreeFrame_Update();
    void FixedFrame_Update();
    void Initialize(Action<float, float> moveCameraCallback, Action<float, float> movePlayerCallback, Action jumpCallback);
}
