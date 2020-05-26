using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionConfigure : MonoBehaviour
{
    public enum LetterBoxType
    {
        Horizontal,
        Vertical
    };
    #region Singleton
    private static ResolutionConfigure instance;
    public static ResolutionConfigure Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<ResolutionConfigure>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("ResolutionConfigure").AddComponent<ResolutionConfigure>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<ResolutionConfigure>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        CreateLetterBox();
    }
    void OnPreCull() => GL.Clear(true, true, Color.black);
    #endregion
    private LetterBoxType letterBoxType;
    public LetterBoxType CurLetterBoxType
    {
        get { return letterBoxType; }
    }
    private float letterboxRatio;
    public float LetterBoxRatio
    {
        get { return letterboxRatio; }
    }
    private void CreateLetterBox()
    {
        Camera[] camera = Camera.allCameras;
        for (int i = 0; i < camera.Length; ++i)
        {
            Rect rect = camera[i].rect;
            float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로)
            float scalewidth = 1f / scaleheight;

            if (scaleheight < 1)
            {
                letterBoxType = LetterBoxType.Vertical;
                letterboxRatio = 1 - scaleheight;
                rect.height = scaleheight;
                rect.y = (1f - scaleheight) / 2f;
            }
            else
            {
                letterBoxType = LetterBoxType.Horizontal;
                letterboxRatio = 1 - scalewidth;
                rect.width = scalewidth;
                rect.x = (1f - scalewidth) / 2f;
            }
            camera[i].rect = rect;
        }
        
    }

    
}
