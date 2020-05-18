using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour
{
    //Data
    public RectTransform currentRect;
    public RectTransform HandleRect;
    private Vector2 joystickStartPos;
    private bool joystickIsActive;
    private Action<float, float> moveExecuteCallback;

    private readonly float JOYSTICK_RADIUS = 175f;
    public readonly float JOYSTICK_HALF_WIDTH = 175f;
    public readonly float JOYSTICK_HALF_HEIGHT = 175f;

    public void Initialize(Action<float, float> moveCallback)
    {
        moveExecuteCallback = moveCallback;
    }
    private Vector2 ScreenToCanvasPos(Vector2 touchPos)
    {
        touchPos.x -= Screen.width / 2;
        touchPos.y -= Screen.height / 2;
        return touchPos;
    }
    public void StartMove(Vector2 touchPos)
    {
        Vector2 touchPosInCanvas = ScreenToCanvasPos(touchPos);

        gameObject.SetActive(true);
        currentRect.anchoredPosition = touchPosInCanvas;
        joystickStartPos = touchPosInCanvas;
        joystickIsActive = true;
    }
    public void MoveJoystick(Vector2 touchPos)
    {
        if (!joystickIsActive)
            return;

        Vector2 touchPosInCanvas = ScreenToCanvasPos(touchPos);

        Vector2 stickDir = (touchPosInCanvas - joystickStartPos).normalized;
        float distance = Vector2.Distance(touchPosInCanvas, joystickStartPos);

        if (distance < JOYSTICK_RADIUS)
            HandleRect.anchoredPosition = stickDir * distance;
        else
            HandleRect.anchoredPosition = stickDir * JOYSTICK_RADIUS;

        moveExecuteCallback(stickDir.x, stickDir.y);
    }
    public void EndMove()
    {
        if (joystickIsActive)
        {
            gameObject.SetActive(false);
            joystickIsActive = false;
        }
    }
}
