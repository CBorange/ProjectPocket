using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_QuestMark : MonoBehaviour
{
    public SpriteRenderer questionMark;
    public SpriteRenderer exclamationMark;

    private void FixedUpdate()
    {
        
    }
    private void DeactiveAllMarker()
    {
        questionMark.gameObject.SetActive(false);
        exclamationMark.gameObject.SetActive(false);
    }
    public void ChageMark_HasAcceptable()
    {
        DeactiveAllMarker();
        exclamationMark.gameObject.SetActive(true);

        Color newColor;
        ColorUtility.TryParseHtmlString("#FFF500", out newColor);
        exclamationMark.color = newColor;
    }
    public void ChangeMark_HasInProgress()
    {
        DeactiveAllMarker();
        questionMark.gameObject.SetActive(true);

        Color newColor;
        ColorUtility.TryParseHtmlString("#9C9C9C", out newColor);
        questionMark.color = newColor;
    }
    public void ChangeMark_HasComplete()
    {
        DeactiveAllMarker();
        questionMark.gameObject.SetActive(true);

        Color newColor;
        ColorUtility.TryParseHtmlString("#FFF500", out newColor);
        questionMark.color = newColor;
    }
    public void TurnOffMark()
    {
        DeactiveAllMarker();
    }
}
