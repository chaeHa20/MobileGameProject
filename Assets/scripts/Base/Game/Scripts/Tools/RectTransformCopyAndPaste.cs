using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class RectTransformCopyAndPaste : MonoBehaviour
{
    [SerializeField] RectTransform m_ui = null;

#if UNITY_EDITOR
    public void setTransformToUI()
    {
        if (null == m_ui)
            return;

        m_ui.localPosition = transform.localPosition;
        m_ui.localRotation = transform.localRotation;
        m_ui.localScale = transform.localScale;
#if UNITY_EDITOR
        EditorHelper.markPrefabDirty();
#endif
    }

    public void getTransformFromUI()
    {
        if (null == m_ui)
            return;

        transform.localPosition = m_ui.localPosition;
        transform.localRotation = m_ui.localRotation;
        transform.localScale = m_ui.localScale;
#if UNITY_EDITOR
        EditorHelper.markPrefabDirty();
#endif
    }
#endif
}
