using UnityEngine;
using UnityHelper;

public class LogoScene : GameScene
{
    [SerializeField] LocalePlugin m_localePlugin = null;
    [SerializeField] UILogoScene m_uiLogoScene = null;

    protected override void initialize()
    {
        base.initialize();
        m_uiLogoScene.initBi();

        createSettings();
        GameAdHelper.instance.initialize(AdSettings.instance, initApp);
    }

    private void createSettings()
    {
#if UNITY_EDITOR
        DebugSettings.create();
        GameSettings.create();
        AISettings.create();
#endif
    }

    private void initApp()
    {
        InitializeApp.create(gameObject, m_localePlugin, () =>
        {
            GameSceneHelper.getInstance().loadStartScene(true, true);
        });
    }
}
