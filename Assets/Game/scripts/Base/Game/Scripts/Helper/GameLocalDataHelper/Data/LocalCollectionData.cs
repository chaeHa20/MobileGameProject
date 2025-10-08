using System;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalCollectionData : LocalData
{
    [SerializeField] LocalCollections<LocalCollection> m_collections= new LocalCollections<LocalCollection>();

    public LocalCollections<LocalCollection> collections => m_collections;


    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        setDefaultCollectionss();
    }

    public override void checkValid()
    {
        base.checkValid();
    }

    private void setDefaultCollectionss()
    {
    }


    public LocalCollections<LocalCollection>.AddResult addCollection(CollectionRow collectionRow, long itemCount)
    {
        var totalExp = (float)itemCount * collectionRow.expPerItem;
        return m_collections.add(collectionRow.id, collectionRow.getType(), totalExp);
    }

    public void removeCollection(string uuid)
    {
        if (null != m_collections.find(uuid))
            m_collections.remove(uuid);
    }

    public override void setDailyReset()
    {
        var e = m_collections.getEnumerator();
        while (e.MoveNext())
        {
            var collection = e.Current;
            if(collection.isExpired())
                m_collections.remove(collection.uuid);
        }
    }
}