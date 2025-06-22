using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoReferenceResolutionMatch : MonoBehaviour 
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
        canvasScaler.matchWidthOrHeight = (0.625f >= r) ? 0.0f : 1.0f;
    }
}

