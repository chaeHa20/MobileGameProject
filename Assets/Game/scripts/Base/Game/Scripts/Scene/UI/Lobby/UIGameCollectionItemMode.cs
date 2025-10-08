using UnityEngine;
using UnityHelper;

public class UIGameCollectionItemMode : UIComponent
{
    [SerializeField] UIGameCollectionItemGradeTab m_gradeTypeTab = null;
    [SerializeField] UIGameCollectionScrollView m_scrollView = null;

    private eGrade m_grade = eGrade.None;

    public eGrade grade => m_grade;

    public void initialize()
    {
        if (null != m_gradeTypeTab)
            m_gradeTypeTab.initialize(null);
        if (null != m_scrollView)
            initScrollView();
    }

    public UIGameCollectionWindowTab.eTab getCollectionsType()
    {
        if (null == m_scrollView)
            return UIGameCollectionWindowTab.eTab.None;
        else
            return m_scrollView.getCollectionsType();
    }


    public void selectGradeTypeTab(eGrade grade)
    {
        m_grade = grade;
        initScrollView(grade);
    }

    private void initScrollView(eGrade grade)
    {
        GameLocalDataHelper.getInstance().requestGetCollectionGrade(m_scrollView.collectionType, grade, (collections) =>
        {
            var data = new UIGameCollectionScrollView.Data
            {
                collections = collections,
            };

            m_scrollView.initialize(data);
            m_scrollView.sort(grade);
        });
    }

    private void initScrollView()
    {
        GameLocalDataHelper.getInstance().requestGetCollectionType(m_scrollView.collectionType, (collections) =>
        {
            var data = new UIGameCollectionScrollView.Data
            {
                collections = collections,
            };

            m_scrollView.initialize(data);
            m_scrollView.sortAll();
        });
    }
}
