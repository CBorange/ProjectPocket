using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface InputSystem
{
    Action InteractAction { get; set; }
    void FreeFrame_Update();
    void FixedFrame_Update();
    void Initialize();
    void ChangeInteractAction(Action interactAction, string actionType);
}
