using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIGameIntroToonWindowData : UIGameMainWindowData
{
    public int lastIndex;
}

public class UIGameIntroToonWindow : UIGameMainWindow
{
    [SerializeField] Image m_toon;

    private int m_startIndex = 0;

    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);

        var d = data as UIGameIntroToonWindowData;
        startLoadToon(d.lastIndex);
    }

    private void startLoadToon(int startIndex)
    {
        var realPath = GameTableHelper.instance.getIntroSpritePath((int)eResource.introToon, startIndex);
        m_toon.sprite = GameResourceHelper.instance.getSprite(realPath);

        waitToonShow(startIndex);
    }

    private void fadeInSprite(int index)
    {
        index++;
        if (index < Define.INTOR_TOON_COUNT)
        {
            var changeType = CoroutineHelper.createTimeType(0.5f);
            CoroutineHelper.instance.start(CoroutineHelper.instance.coChangeValue(1.0f, 0.0f, changeType, (value, done) =>
            {
                m_toon.color = new Color(m_toon.color.r, m_toon.color.g, m_toon.color.b, value);
                if (done)
                    loadIntorToonSprite(index);
            }));
        }
        else
        {
            GameCoroutineHelper.getInstance().waitSeconds(2f, () =>
            {
                base.onClose();
            });
        }
    }

    private void showSprite(int index)
    {
        var changeType = CoroutineHelper.createTimeType(1.0f);
        CoroutineHelper.instance.start(CoroutineHelper.instance.coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
        {
            m_toon.color = new Color(m_toon.color.r, m_toon.color.g, m_toon.color.b, value);
            if (done)
                waitToonShow(index);
        }));
    }

    private void waitToonShow(int index)
    {
        GameCoroutineHelper.getInstance().waitSeconds(1.5f, () =>
        {
            fadeInSprite(index);
        });
    }


    private void loadIntorToonSprite(int index)
    {
        var realPath = GameTableHelper.instance.getIntroSpritePath((int)eResource.introToon, index);
        m_toon.sprite = GameResourceHelper.instance.getSprite(realPath);

        showSprite(index);
    }
}
