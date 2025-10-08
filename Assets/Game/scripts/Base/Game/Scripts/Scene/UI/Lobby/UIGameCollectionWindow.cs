using UnityEngine;
using System.Collections.Generic;
using UnityHelper;

public class UIGameCollectionWindow : UIGameWindow
{
    [SerializeField] UIGameCollectionWindowTab m_tab = null;
    [SerializeField] List<UIGameCollectionWindowTabContent> m_contents = new List<UIGameCollectionWindowTabContent>();

    private Dictionary<UIGameCollectionWindowTab.eTab, UIGameCollectionWindowTabContent> m_contentsByTabType = new Dictionary<UIGameCollectionWindowTab.eTab, UIGameCollectionWindowTabContent>();

    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);
        initContents();

        initTab(UIGameCollectionWindowTab.eTab.BGTemplete);
    }

    private void initTab(UIGameCollectionWindowTab.eTab tab)
    {
        var data = new UIGameCollectionWindowTab.Data
        {
            tab = tab,
        };

        m_tab.initialize(data);
    }

    private void initContents()
    {
        foreach(var content in m_contents)
        {
            var key = content.tabMode.getCollectionsType();
            if (m_contentsByTabType.ContainsKey(key))
                m_contentsByTabType[key] = content;
            else
                m_contentsByTabType.Add(key, content);
        }
    }

    public void onSelectTab(int tabId, bool isFirst)
    {
        setTab((UIGameCollectionWindowTab.eTab)tabId);
    }

    private void setTab(UIGameCollectionWindowTab.eTab tab)
    {
    }
}
