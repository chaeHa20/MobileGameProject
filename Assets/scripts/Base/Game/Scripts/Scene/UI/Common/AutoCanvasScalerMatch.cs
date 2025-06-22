using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoCanvasScalerMatch : MonoBehaviour
{
    void Awake()
    {
        setMatch();
    }

    private void setMatch()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        // 0.75는 16:12비율이다.

        float r = (float)Screen.width / (float)Screen.height;
        canvasScaler.matchWidthOrHeight = (0.75f > r) ? 0.0f : 1.0f;
    }
}
