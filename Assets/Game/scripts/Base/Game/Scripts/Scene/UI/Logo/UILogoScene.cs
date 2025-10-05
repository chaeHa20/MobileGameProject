using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UILogoScene : UIGameScene
{
    [SerializeField] Button m_startPlayButton;

    public void initUILogoScene()
    {
        m_startPlayButton.gameObject.SetActive(false);
    }

    public void onActivePlayButton()
    {
        m_startPlayButton.gameObject.SetActive(true);
    }

    public void onClickStartPlay()
    {
        GameSceneHelper.getInstance().loadStartScene(true, true);
    }
}
