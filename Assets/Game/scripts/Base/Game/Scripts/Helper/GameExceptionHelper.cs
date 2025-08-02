using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameExceptionHelper : ExceptionHelper<GameExceptionHelper>
{
    protected override void openSendMailMsgBox(string condition, string stackTrace)
    {
        //string title = StringHelper.get("information");
        //string body = StringHelper.get("system_exception_msg");
        //GameUIHelper.getInstance().openOkCancelMsgBox(title, body, eResource.UIMsgBox, () =>
        //{
        //    if (Debugx.isActive)
        //        sendDataMail(condition, stackTrace);
        //    else
        //        sendMail(condition, stackTrace);

        //    quitApplication();
        //}, () =>
        //{
        //    quitApplication();
        //});
    }

    private void sendDataMail(string condition, string stackTrace)
    {
        CloudDataParser dataParser = new CloudDataParser();
        sendMail(condition, stackTrace, dataParser.getData(AESSettings.instance.localData));
    }

    protected override void openEditorMsgBox(string condition, string stackTrace)
    {
        //string title = StringHelper.get("information");
        //string body = StringHelper.get("system_exception_msg");
        //GameUIHelper.getInstance().openCenterMsgBox(title, body, eResource.UIMsgBox, () =>
        //{
            quitApplication();
        //});
    }
}
