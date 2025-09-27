using UnityEngine;
using UnityHelper;

public class InputManager : NonMonoSingleton<InputManager>
{
    
    public void updateInput(float dt)
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical"); // 이거는 나중에 가상 조이스틱 만들어서 적용해야 함

        var targetDir = new Vector3(hor, 0.0f, ver);
        if (null != PlayerManager.instance)
            PlayerManager.instance.updateDirection(dt, targetDir);
    }

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
}
