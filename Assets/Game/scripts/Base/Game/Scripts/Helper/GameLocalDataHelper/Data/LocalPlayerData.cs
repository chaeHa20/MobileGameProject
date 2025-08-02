using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalPlayerData : LocalData
{
    [SerializeField] List<LocalItem> m_attachedItems = new List<LocalItem>();
    [SerializeField] string m_playerSocailId = "";

    public List<LocalItem> attachedItems => m_attachedItems;
    public string playerSocailId => playerSocailId;

    public void setPlayerSocialId(string id)
    {
        if (!string.IsNullOrEmpty(id) && id != m_playerSocailId)
            m_playerSocailId = id;
    }

    public void takeOnItem(eParts part, LocalItem item)
    {
        var index = (int)part - 1;

        if (null != m_attachedItems[index])
        {
            m_attachedItems[index] = null;
        }
        m_attachedItems[index] = item;

    }

    public void takeOffItem(eParts part)
    {
        var index = (int)part - 1;

        if (null != m_attachedItems[index])
        {
            m_attachedItems[index] = null;
        }
    }

    public override void serialize()
    {
        base.serialize();
    }

    private void initEquipments()
    {
        SystemHelper.forEachEnum<eParts>((e) =>
        {
            if (eParts.None == e)
                return;

            m_attachedItems.Add(null);
        });
    }
}

