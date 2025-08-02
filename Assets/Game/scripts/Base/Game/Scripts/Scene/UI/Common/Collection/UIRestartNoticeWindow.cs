using System;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIRestartNoticeWindowData : UIGameWindowData
{
    public eOption option;
    public Action<bool> restartCallback;
}

public class UIRestartNoticeWindow : UIGameWindow
{
    [SerializeField] Text m_title = null;
    [SerializeField] Text m_notice = null;
    [SerializeField] GameObject m_cancel = null;

    private eOption m_option = eOption.None;
    private Action<bool> m_restartCallback = null;

    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);

        var d = data as UIRestartNoticeWindowData;
        m_option = d.option;
        m_restartCallback = d.restartCallback;

        if (d.option == eOption.GraphicQuiality)
        {
            m_title.text = StringHelper.get("set_graphic");
            m_notice.text = StringHelper.get("restart_desc");
            m_cancel.SetActive(true);
        }
    }

    public void onClickOk()
    {
        base.onClose();
        m_restartCallback?.Invoke(true);
    }

    public void onClickCancle()
    {
        base.onClose();
        m_restartCallback?.Invoke(false);
    }
}