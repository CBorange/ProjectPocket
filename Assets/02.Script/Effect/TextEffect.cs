using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    // Data
    private Transform baseObject;
    private Vector3 basePos;
    private Text myText;
    private bool isActive;
    private Action<TextEffect> returnToPoolCallback;
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        transform.LookAt(Camera.main.transform);
        Vector3 myAngle = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, myAngle.y + 180, 0);
        transform.position = basePos + new Vector3(0, 2 + elapsedTime * 3f, 0);
    }
    public void Initialize(Action<TextEffect> returnToPoolCallback, Transform baseObj)
    {
        baseObject = baseObj;
        myText = GetComponent<Text>();
        this.returnToPoolCallback = returnToPoolCallback;
    }
    private void EndMove()
    {
        if (!isActive)
            return;
        gameObject.SetActive(false);
        isActive = false;
        returnToPoolCallback(this);
    }
    public void Play(float damage, Color color)
    {
        basePos = baseObject.position;
        isActive = true;
        elapsedTime = 0f;
        myText.text = damage.ToString();
        myText.color = color;
        gameObject.SetActive(true);

        Invoke("EndMove", 0.4f);
    }
    public void ForceEndMove()
    {
        gameObject.SetActive(false);
        isActive = false;
        returnToPoolCallback(this);
    }
}
