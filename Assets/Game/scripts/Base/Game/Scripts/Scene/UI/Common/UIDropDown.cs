using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIDropDown : MonoBehaviour
{
    private void Start()
    {
        setLanguage();
    }

    public void setLanguage()
    {
        var dropDown = GetComponent<Dropdown>();
        if (null == dropDown)
            return;

        dropDown.value = (int)LanguageHelper.language;
        var textList = dropDown.options;
        for (int index = 0; index < textList.Count; index++)
        {
            var texsItem = textList[index];
            texsItem.text = setDropdownItem(index);
        }
    }

    private string setDropdownItem(int index)
    {
        return StringHelper.get((eLanguage)index);
    }
}
