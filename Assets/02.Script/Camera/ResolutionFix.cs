using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionFix : MonoBehaviour
{
    private float screenWidth;
    private float screenHeight;
    void Awake()
    {
        Camera[] camera = Camera.allCameras;
        for (int i = 0; i < camera.Length; ++i)
        {
            Rect rect = camera[i].rect;
            float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로)
            float scalewidth = 1f / scaleheight;

            if (scaleheight < 1)
            {
                rect.height = scaleheight;
                rect.y = (1f - scaleheight) / 2f;
            }
            else
            {
                rect.width = scalewidth;
                rect.x = (1f - scalewidth) / 2f;
            }
            camera[i].rect = rect;
        }
        
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}
