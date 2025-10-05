using UnityEngine;
using UnityHelper;

public class InputManager : NonMonoSingleton<InputManager>
{
#if UNITY_EDITOR
    public void updateVertical(bool isUp, float dt)
    {
        var value = isUp ? 1.0f : -1.0f;
        if (null != PlayerManager.instance)
            PlayerManager.instance.updateMove(dt, value);
    }

    public void updateHorizontal(bool isLeft, float dt)
    {
        var value = isLeft ? -1.0f : 1.0f;
        var targetRot = new Vector3(0.0f, value, 0.0f);
        if (null != PlayerManager.instance)
            PlayerManager.instance.updateRotation(dt, targetRot);
    }
#endif
}
