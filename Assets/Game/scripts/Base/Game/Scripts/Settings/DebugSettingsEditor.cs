using UnityEngine;
using UnityHelper;
using System.Numerics;





#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DebugSettings))]
public class DebugSettingsEditor : BaseDebugSettingsEditor
{
    private float m_timeScale = 1.0f;
    private int m_targetMapId = 1;
    private int m_targetStageId = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        onTimeScale();
        onAddCurrency();
    }

    private void onTimeScale()
    {
        var timeScale = EditorGUILayout.Slider("Time Scale", m_timeScale, 0.0f, 20.0f);
        if (timeScale != m_timeScale)
        {
            if (Debugx.isActive)
                TimeHelper.timeScale = timeScale;
        }

        m_timeScale = timeScale;
    }

    private void onAddCurrency()
    {
        EditorGUILayout.Space();

        var editorValues = (target as DebugSettings).editorValues;
        editorValues.addCurrencyValue = EditorGUILayout.TextField("Add Currency Value", editorValues.addCurrencyValue);
        editorValues.addCurrencyType = (eCurrency)EditorGUILayout.EnumPopup(editorValues.addCurrencyType);
        if (GUILayout.Button("Add Currency"))
        {
            if (EditorApplication.isPlaying)
            {
                if (eCurrency.None == editorValues.addCurrencyType)
                {
                    EditorUtility.DisplayDialog("오류", "재화 타입이 잘 못 되었습니다..", "확인");
                    return;
                }

                if (BigInteger.TryParse(editorValues.addCurrencyValue, out BigInteger addValue))
                {
                    var req = new Req_AddCurrency
                    {
                        currencyType = editorValues.addCurrencyType,
                        addValue = addValue
                    };
                    GameLocalDataHelper.instance.request<Res_AddCurrency>(req, (res) =>
                    {
                        if (res.isSuccess)
                        {
                            GameMsgHelper.instance.add(new SetCurrencyItemMsg(eCurrencyValueId.Main, res.currency));
                        }
                    });
                }
            }
        }
    }
}

#endif