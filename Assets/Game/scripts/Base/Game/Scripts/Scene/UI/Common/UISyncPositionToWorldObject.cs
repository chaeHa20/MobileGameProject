using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UISyncPositionToWorldObject : MonoBehaviour
{
    private Coroutine m_coSyncPosition = null;

    public static void start(GameObject uiSyncPositionToWorldObject, GameObject worldObject)
    {
        var uiSyncPosition = uiSyncPositionToWorldObject.GetComponent<UISyncPositionToWorldObject>();
        uiSyncPosition.start(worldObject);
    }

    public void start(GameObject worldObject)
    {
        if (null != m_coSyncPosition)
            StopCoroutine(m_coSyncPosition);

        if (gameObject.activeInHierarchy)
            m_coSyncPosition = StartCoroutine(coSyncPosition(worldObject));
    }

    IEnumerator coSyncPosition(GameObject worldObject)
    {
        var canvas = UIHelper.instance.canvasGroup.getFirstCanvas().canvas;
        RectTransform rtCanvas = canvas.GetComponent<RectTransform>();
        RectTransform rt = GetComponent<RectTransform>();

        while (true)
        {
            if (null == worldObject)
                break;

            var camera = Camera.main;
            if (null != camera)
            {
                var worldPosition = worldObject.transform.position;
                UIHelper.instance.worldToCanvasPosition(Camera.main, rtCanvas, worldPosition, rt);
            }

            yield return null;
        }

        m_coSyncPosition = null;
    }
}
