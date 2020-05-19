using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkerMover : MonoBehaviour
{
    private Vector3 originPos;
    private float moveRadian = 0f;
    private float moveRange = 0.1f;

    private void Awake()
    {
        originPos = transform.position;
    }
    private void FixedUpdate()
    {
        // Rot
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // Pos
        moveRadian += Time.deltaTime * 2;
        float sin = Mathf.Sin(moveRadian);

        Vector3 newPos = new Vector3(originPos.x, originPos.y + (moveRange * sin), originPos.z);
        transform.position = newPos;
    }
}
