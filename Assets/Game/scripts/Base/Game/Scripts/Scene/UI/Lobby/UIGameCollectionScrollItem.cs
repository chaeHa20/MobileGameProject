using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;
using TMPro;

public class UIGameCollectionScrollItem : UIScrollItem
{
    public class Data : BaseData
    {
        public UIGameCollectionWindowTab.eTab collectionType;
        public eGrade grade;
        public int collectionId;
        public string uuid;
    }

    [Header("Component Setting")]
    [SerializeField] GameObject m_select = null;
    [SerializeField] GameObject m_itemParent = null;

    [Header("UI Setting")]
    [SerializeField] Image m_collectionIcon = null;
    [SerializeField] Text m_name = null;
    [SerializeField] TMP_Text m_gradeName = null;

    private int m_collectionId = 0;
    private string m_collectionUuid = "0";
    private UIGameCollectionWindowTab.eTab m_collectionType = UIGameCollectionWindowTab.eTab.None;
    private eGrade m_grade = eGrade.None;

    public bool isSelect => m_select.activeSelf;
    public eGrade grade => m_grade;

    private void Awake()
    {
    }

    public override void initialize(UIScrollView scrollView, BaseData baseData, int itemIndex)
    {
        base.initialize(scrollView, baseData, itemIndex);

        var data = baseData as Data;

        m_grade = data.grade;
        m_collectionType = data.collectionType;
        m_collectionId = data.collectionId;
        m_collectionUuid = data.uuid;

        setSelectMode();
    }

    private void setSelectMode()
    {
        m_select.SetActive(false);

        if (m_grade <= 0)
            return;

    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        Dispose();
    }
}
