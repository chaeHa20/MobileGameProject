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

    private bool m_isSkipIntro = false;

    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);

        var d = data as UIGameIntroToonWindowData;

        m_isSkipIntro = false;
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
                {
                    if (m_isSkipIntro)
                        base.onClose();
                    else
                        loadIntorToonSprite(index);
                }
            }));
        }
        else
        {
            GameCoroutineHelper.getInstance().waitSeconds(2f, () =>
            {
                if(m_isSkipIntro)
                    base.onClose();
                else
                    setEndShowIntroToon();
            });
        }
    }

    public void onClickSkipIntro()
    {
        skipIntroToon();
    }

    private void skipIntroToon()
    {
        var req = new Req_SkipIntro();

        GameLocalDataHelper.instance.request<Res_SkipIntro>(req, (res) =>
        {
            if(res.isSuccess)
            {
                m_isSkipIntro = true;
            }
        });
    }

    private void setEndShowIntroToon()
    {
        var req = new Req_ShowIntro();

        GameLocalDataHelper.instance.request<Res_ShowIntro>(req, (res) =>
        {
            if (res.isSuccess)
            {
                base.onClose();
            }
        });
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
