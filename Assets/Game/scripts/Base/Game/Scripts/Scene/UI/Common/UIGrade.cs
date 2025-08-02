using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIGrade : MonoBehaviour
{
    [SerializeField] Text m_grade = null;

    public void setGrade(eGrade grade)
    {
        m_grade.text = StringHelper.get(grade);
        m_grade.color = GameResourceHelper.getInstance().getGradeColor(grade);
    }
}
