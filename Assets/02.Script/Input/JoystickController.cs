﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour
{
    // Controller
    public PlayerMovementController MovementController;

    //Data
    public RectTransform currentRect;
    public RectTransform HandleRect;
    private Vector2 joystickStartPos;
    private bool joystickIsActive;
    public bool JoystickIsActive
    {
        get { return joystickIsActive; }
    }

    // Resolution
    private readonly float JOYSTICK_RADIUS = 175f;

    public void Initialize()
    {
    }
    private Vector2 ScreenToCanvasPos(Vector2 touchPos)
    {
        touchPos.x = touchPos.x * ResolutionConfigure.Instance.CanvasWidthRatio;
        touchPos.y = touchPos.y * ResolutionConfigure.Instance.CanvasHeightRatio;
        touchPos.x -= (Screen.width / 2) * ResolutionConfigure.Instance.CanvasWidthRatio;
        touchPos.y -= (Screen.height / 2) * ResolutionConfigure.Instance.CanvasHeightRatio;
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
        touchPos.x *= ResolutionConfigure.Instance.CanvasWidthRatio;
        touchPos.y *= ResolutionConfigure.Instance.CanvasHeightRatio;
        float letterBoxSize = ResolutionConfigure.Instance.BaseCanvas_Width * ResolutionConfigure.Instance.LetterBoxRatio;
        letterBoxSize /= 2;
        if (ResolutionConfigure.Instance.CurLetterBoxType == ResolutionConfigure.LetterBoxType.Horizontal)
        {
            if ((touchPos.x > JOYSTICK_RADIUS + letterBoxSize) && (touchPos.y < ResolutionConfigure.Instance.BaseCanvas_Height - JOYSTICK_RADIUS) &&
                (touchPos.y > JOYSTICK_RADIUS))
                return true;
            else
                return false;
        }
        else
        {
            if ((touchPos.x > JOYSTICK_RADIUS + letterBoxSize) && (touchPos.y < ResolutionConfigure.Instance.BaseCanvas_Height - JOYSTICK_RADIUS + letterBoxSize) &&
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

        MovementController.HorizontalMovement(stickDir.x, stickDir.y);
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
