using UnityEngine;
using UnityHelper;

public class GameSimulator : MonoBehaviour
{
    [SerializeField] int m_updateTime = 5;

    public static bool isNotShowOfflineGold { private get; set; }

    public void initialize()
    {
    }

#if !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (Logx.isActive)
            Logx.trace("GameSimulator OnApplicationPause {0}", pause);

        if (!isEnableCalcOfflineGoldScene())
            return;

        if (pause)
        {
            
        }
        else
        {
            if (GameSimulator.isNotShowOfflineGold)
            {
                GameSimulator.isNotShowOfflineGold = false;
                return;
            }
        }
    }
#endif

    private void OnApplicationQuit()
    {
        if (Logx.isActive)
            Logx.trace("GameSimulator OnApplicationQuit");
    }

    private bool isEnableCalcOfflineGoldScene()
    {
        return false;
    }
}
