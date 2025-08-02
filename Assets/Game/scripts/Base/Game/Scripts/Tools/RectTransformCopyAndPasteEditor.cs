using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RectTransformCopyAndPaste))]
public class RectTransformCopyAndPasteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        onSetTransformToUI();
        onGetTransformFromUI();
    }

    private void onSetTransformToUI()
    {
        if (GUILayout.Button("Set transform to UI"))
        {
            var cnp = target as RectTransformCopyAndPaste;
            cnp.setTransformToUI();
        }
    }

    private void onGetTransformFromUI()
    {
        if (GUILayout.Button("Get transform from UI"))
        {
            var cnp = target as RectTransformCopyAndPaste;
            cnp.getTransformFromUI();
        }
    }
}
#endif