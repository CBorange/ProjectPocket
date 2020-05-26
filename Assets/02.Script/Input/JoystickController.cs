using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour
{
    //Data
    public Canvas OverlayCanvas;
    public RectTransform currentRect;
    public RectTransform HandleRect;
    private Vector2 joystickStartPos;
    private bool joystickIsActive;
    private Action<float, float> moveExecuteCallback;

    private float baseCanvas_Width;
    private float baseCanvas_Height;
    private float widthRatio;
    private float heightRatio;
    private readonly float JOYSTICK_RADIUS = 175f;

    public void Initialize(Action<float, float> moveCallback)
    {
        baseCanvas_Width = OverlayCanvas.GetComponent<RectTransform>().rect.width;
        baseCanvas_Height = OverlayCanvas.GetComponent<RectTransform>().rect.height;

        widthRatio = baseCanvas_Width / Screen.width;
        heightRatio = baseCanvas_Height / Screen.height;

        moveExecuteCallback = moveCallback;
    }
    private Vector2 ScreenToCanvasPos(Vector2 touchPos)
    {
        touchPos.x = touchPos.x * widthRatio;
        touchPos.y = touchPos.y * heightRatio;
        touchPos.x -= (Screen.width / 2) * widthRatio;
        touchPos.y -= (Screen.height / 2) * heightRatio;
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
    public bool IsPossibleToMoveJoystick(Vector2 touchPos)
    {
        touchPos.x *= widthRatio;
        touchPos.y *= heightRatio;
        float letterBoxSize = baseCanvas_Width * ResolutionConfigure.Instance.LetterBoxRatio;
        letterBoxSize /= 2;
        if (ResolutionConfigure.Instance.CurLetterBoxType == ResolutionConfigure.LetterBoxType.Horizontal)
        {
            if ((touchPos.x > JOYSTICK_RADIUS + letterBoxSize) && (touchPos.y < baseCanvas_Height - JOYSTICK_RADIUS) &&
                (touchPos.y > JOYSTICK_RADIUS))
                return true;
            else
                return false;
        }
        else
        {
            if ((touchPos.x > JOYSTICK_RADIUS + letterBoxSize) && (touchPos.y < baseCanvas_Height - JOYSTICK_RADIUS + letterBoxSize) &&
                (touchPos.y > JOYSTICK_RADIUS + letterBoxSize))
                return true;
            else
                return false;
        }
        
    }
    public void MoveJoystick(Vector2 touchPos)
    {
        if (!joystickIsActive)
            return;

        Vector2 touchPosInCanvas = ScreenToCanvasPos(touchPos);

        Vector2 stickDir = (touchPosInCanvas - joystickStartPos).normalized;
        float distance = Vector2.Distance(touchPosInCanvas, joystickStartPos);

        if (distance < JOYSTICK_RADIUS / 2)
            HandleRect.anchoredPosition = stickDir * distance;
        else
            HandleRect.anchoredPosition = stickDir * JOYSTICK_RADIUS / 2;

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
