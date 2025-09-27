using UnityEngine;

public class RunnerSceneLoadData : GameSceneLoadData
{ }


public class RunnerScene : GameScene
{
    protected override void initialize()
    {
        base.initialize();
    }

    protected override void update()
    {
        base.update();

        if (null == InputManager.instance)
            return;

        var dt = Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            InputManager.instance.updateVertical(true, dt);
        if (Input.GetKey(KeyCode.S))
            InputManager.instance.updateVertical(false, dt);
        if (Input.GetKey(KeyCode.A))
            InputManager.instance.updateHorizontal(true, dt);
        if (Input.GetKey(KeyCode.D))
            InputManager.instance.updateHorizontal(false, dt);
    }
    
}
