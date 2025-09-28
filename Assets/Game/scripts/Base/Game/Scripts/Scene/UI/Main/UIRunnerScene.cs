using UnityEngine;
using UnityHelper;

public class UIRunnerScene : UIGameScene
{
    [SerializeField] UIJoyStick m_joystick;

    protected override void update()
    {
        base.update();

        if (null == m_joystick)
            return;

        var dt = Time.deltaTime;
        m_joystick.updateJoystick(dt);
    }
}
