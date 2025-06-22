using System;
using UnityEngine;
using UnityHelper;

public class UIManager : MonoSingleton<UIManager>, IDisposable
{
    public void initManager(bool isCinematicScene = false)
    {
        if (isCinematicScene)
            adjustCinematicScreenResolution();
        else
            adjustFovToFitScreenResolution();
    }

    private void adjustCinematicScreenResolution()
    {
        var adjustFovToFitScreenResolution = GameSettings.instance.stage.adjustFovToFitScreenResolution;

        var baseRatio = adjustFovToFitScreenResolution.getBaseRatio();
        var curRatio = (float)Screen.height / Screen.width;
        var r = curRatio / baseRatio; 

        Camera.main.orthographicSize = r * Camera.main.orthographicSize;
    }


    private void adjustFovToFitScreenResolution()
    {
        var adjustFovToFitScreenResolution = GameSettings.instance.stage.adjustFovToFitScreenResolution;

        var baseRatio = adjustFovToFitScreenResolution.getBaseRatio();
        var curRatio = (float)Screen.height / Screen.width;
        var r = curRatio / baseRatio;
        
        var fov = r * adjustFovToFitScreenResolution.baseFov;
        var size = r * adjustFovToFitScreenResolution.baseSize;
        
        Camera.main.fieldOfView = fov;
        Camera.main.orthographicSize = size;
    }

    public void Dispose()
    {
        
    }
}
