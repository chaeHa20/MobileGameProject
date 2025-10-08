using UnityEngine;
using UnityHelper;

public class UIGameCollectionItemGradeTab : UITab
{
    [SerializeField] 
    UIGameCollectionItemMode m_collectionMode = null;
    public override void initialize(BaseData baseData)
    {
        base.initialize(baseData);

        setTab((int)eGrade.None);
    }

    public void onSelect(int tabId, bool isFirst)
    {
        m_collectionMode.selectGradeTypeTab((eGrade)tabId);
    }
}
