using UnityEngine;
using UnityHelper;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : BaseGameSettingsEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        onAddCaemraSetting();
        onClearCameraSettings();
        onRemoveStageCameraSettings();
    }

    private void onAddCaemraSetting()
    {
        EditorGUILayout.Space();

        var editorValues = (target as GameSettings).cameraSettingStageId;
        var cameraScrollMaxZValue = (target as GameSettings).cameraScrollMaxTopPositionZValue;
        var caemraScrollMinZValue = (target as GameSettings).cameraScrollMinBottomPositionZValue;
        if (GUILayout.Button("Add CameraSetting"))
        {
            var camera = Camera.main;
            GameSettings.CameraTransform newSetting = new GameSettings.CameraTransform();
            newSetting.stageId = editorValues;
            newSetting.fov = camera.fieldOfView;
            newSetting.size = camera.orthographicSize;
            newSetting.position = camera.transform.localPosition;
            newSetting.rotation = camera.transform.localRotation;
            newSetting.cameraScrollMaxZValue = cameraScrollMaxZValue;
            newSetting.cameraScrollMinZValue = caemraScrollMinZValue;

            GameSettings.instance.cameraTransformSettings.Add(newSetting);
        }
    }

    private void onClearCameraSettings()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear CameraSetting"))
        {
            GameSettings.instance.cameraTransformSettings.Clear();
        }
    }

    private void onRemoveStageCameraSettings()
    {
        EditorGUILayout.Space();

        var editorValues = (target as GameSettings).cameraSettingStageId;
        var ButtonName = string.Format("Remove CameraSetting in Stage {0}", editorValues);
        if (GUILayout.Button(ButtonName))
        {
            for(int index =0; index < GameSettings.instance.cameraTransformSettings.Count; index++)
            {
                if (editorValues == GameSettings.instance.cameraTransformSettings[index].stageId)
                {
                    GameSettings.instance.cameraTransformSettings.RemoveAt(index);
                    return;
                }
            }
        }
    }
}

#endif