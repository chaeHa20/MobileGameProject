using UnityEngine;
using UnityHelper;

public class RunnerSceneLoadData : GameSceneLoadData
{ }


public class RunnerScene : GameScene
{
    [SerializeField] UIRunnerScene m_ui;
    [SerializeField] Player m_player;
    [SerializeField] GameObject m_mapTileParent;

    protected override void initialize()
    {
        base.initialize();

        initLoadMapTile();
    }

    private void initLoadMapTile()
    {
        GamePoolHelper.getInstance().pop<MapTile>(eResource.MapTile, (t) =>
        {
            GraphicHelper.setParent(m_mapTileParent, t.gameObject);
            
            t.initialize(Vector2.zero, m_mapTileParent);

            MapTileManager.instance.addMapTile(Vector2.zero, t);

            PlayerManager.instance.loadPlayerCharacter();
        });
    }


    protected override void update()
    {
        base.update();

#if UNITY_EDITOR
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
#endif
    }
    
}
