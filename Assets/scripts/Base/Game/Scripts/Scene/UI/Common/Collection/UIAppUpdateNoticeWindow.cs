using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIAppUpdateNoticeWindowData : UIGameWindowData
{
}

public class UIAppUpdateNoticeWindow : UIGameWindow
{
    [SerializeField] Text m_title = null;
    [SerializeField] Text m_notice = null;

    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);
        m_title.text = StringHelper.get("information");
        m_notice.text = StringHelper.get("app_update");
    }

    public void onClickOk()
    {
        base.onClose();
    }

}