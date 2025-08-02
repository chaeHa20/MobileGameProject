using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityHelper;

public class GameSceneLoadData : SceneLoadData
{

}

public class GameScene : WorldScene
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void initialize()
    {
        base.initialize();
    }

    protected virtual void setQualitySettings()
    {
        GameLocalDataHelper.getInstance().requestGetGameOption((localGameOption) =>
        {
            QualitySettings.SetQualityLevel((int)localGameOption.graphicQuality);

#if UNITY_ANDROID
            if (eGraphicQuality.Low == localGameOption.graphicQuality)
            {
                var camData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
                if (null != camData)
                {
                    camData.renderShadows = false;
                    camData.renderPostProcessing = false;
                    camData.antialiasing = AntialiasingMode.None;
                }
            }
#endif
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            
        }

        base.Dispose(disposing);
    }
}
