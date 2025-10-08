using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityHelper;

public class UIGameCollectionScrollView : UIScrollView
{
    public class Data : BaseData
    {
        public List<LocalCollection> collections;
    }

    [SerializeField] eCollection m_collectionType = eCollection.None;

    public eCollection collectionType => m_collectionType;

    private List<UIGameCollectionScrollItem> m_sortedItems = null;


    private Dictionary<string, UIGameCollectionScrollItem> m_collectionsByUuid = new Dictionary<string, UIGameCollectionScrollItem>();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void initialize(BaseData baseData)
    {
        base.initialize(baseData);

        var data = baseData as Data;
        m_collectionsByUuid.Clear();


        initCollections(data.collections);
    }

    public UIGameCollectionWindowTab.eTab getCollectionsType()
    {
        switch (m_collectionType)
        {
            case eCollection.BGTemplete:
                return UIGameCollectionWindowTab.eTab.BGTemplete;
            case eCollection.Rigding:
                return UIGameCollectionWindowTab.eTab.Rigding;
            case eCollection.Weapon:
                return UIGameCollectionWindowTab.eTab.Weapon;
            case eCollection.Armor:
                return UIGameCollectionWindowTab.eTab.Armor;
            case eCollection.PlayerCharacter:
                return UIGameCollectionWindowTab.eTab.PlayerCharacter;
            default:
                return UIGameCollectionWindowTab.eTab.None;
        }
    }

    protected bool tryGetCollections<T>(string uuid, out T t) where T : UIGameCollectionScrollItem
    {
        if (m_collectionsByUuid.TryGetValue(uuid, out UIGameCollectionScrollItem item))
        {
            t = item as T;
            return true;
        }

        t = null;
        return false;
    }

    protected void addCollections(string uuid, UIGameCollectionScrollItem item)
    {
        m_collectionsByUuid.Add(uuid, item);
    }

    public List<string> getSelectItemUuids()
    {
        var uuids = new List<string>();
        foreach (var pair in m_collectionsByUuid)
        {
            if (pair.Value.isSelect)
                uuids.Add(pair.Key);
        }

        return uuids;
    }

    private void initCollections(List<LocalCollection> collections)
    {
        if (null == collections)
            return;

        resizeItem<UIGameCollectionScrollItem>(collections.Count);

        for (int i = 0; i < collections.Count; ++i)
        {
            var item = getItem<UIGameCollectionScrollItem>(i);
            var targetItemId = collections[i].id;

            var data = new UIGameCollectionScrollItem.Data
            {
                collectionType = getCollectionsType(),
                grade = collections[i].type.grade,
                collectionId = collections[i].id,
                uuid = collections[i].uuid,
            };
            item.initialize(this, data, i);

            addCollections(data.uuid, item);
        }
    }

    public void sort(eGrade grade)
    {
        var items = new List<UIGameCollectionScrollItem>();
        var e = getItemEnumerator();
        while (e.MoveNext())
        {
            var item = e.Current as UIGameCollectionScrollItem;

            if (item.grade == grade)
            {
                items.Add(item);
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }

        m_sortedItems = (from item in items
                         orderby item.isSelect descending, item.grade descending, item.name ascending
                         select item).ToList();

        for (int i = 0; i < m_sortedItems.Count; ++i)
        {
            m_sortedItems[i].transform.SetSiblingIndex(i);
        }
    }

    public void sortAll()
    {
        var items = new List<UIGameCollectionScrollItem>();
        var e = getItemEnumerator();
        while (e.MoveNext())
        {
            var item = e.Current as UIGameCollectionScrollItem;
            items.Add(item);
            item.gameObject.SetActive(true);
        }

        m_sortedItems = (from item in items
                         orderby item.isSelect descending, item.grade descending, item.name ascending
                         select item).ToList();

        for (int i = 0; i < m_sortedItems.Count; ++i)
        {
            m_sortedItems[i].transform.SetSiblingIndex(i);
        }
    }

    public List<UIGameCollectionScrollItem>.Enumerator getSortedItemEnumerator()
    {
        if (null == m_sortedItems)
        {
            return new List<UIGameCollectionScrollItem>().GetEnumerator();
        }
        else
        {
            return m_sortedItems.GetEnumerator();
        }
    }
}
