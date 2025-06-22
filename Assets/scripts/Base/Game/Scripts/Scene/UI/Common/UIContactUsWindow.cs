using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIContactUsWindow : UIGameWindow
{
    [SerializeField] Toggle m_attachSavedData = null;

    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);

        m_attachSavedData.isOn = true;
    }

    public void onSend()
    {
        if (m_attachSavedData.isOn)
            sendDataMail();
        else
            sendMail(null);

        onClose();
    }

    private void sendDataMail()
    {
        writeData();

        string data = StringHelper.get("please_attach_saved_data", GameSettings.instance.app.cloudFileName);
        sendMail(data);
    }

    protected void sendMail(string data = null)
    {
        var mailAddress = GameSettings.instance.app.helpMailAddress;
        SystemHelper.sendHelpMail(mailAddress, data);
    }

    private void writeData()
    {
        CloudDataParser dataParser = new CloudDataParser();
        var data = dataParser.getData(AESSettings.instance.localData);

        var filename = GameSettings.instance.app.cloudFileName + ".json";
        string path = FileHelper.combine(Application.persistentDataPath, filename);

        FileHelper.writeStream(path, data);
    }
}
