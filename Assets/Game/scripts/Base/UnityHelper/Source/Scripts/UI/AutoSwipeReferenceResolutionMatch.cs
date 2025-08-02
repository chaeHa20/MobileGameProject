using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSwipeReferenceResolutionMatch : MonoBehaviour 
{
	// Use this for initialization
	void Awake () 
    {
        setMatch();
	}
	
    private void setMatch()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        float r = (float)Screen.width/(float)Screen.height;
        canvasScaler.matchWidthOrHeight = (1.0f > r) ? 0.0f : 1.0f;
    }
}
