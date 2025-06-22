using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class InAppProductRow : TableRow
{
    private string m_oneStoreProductId;
    private string m_androidProductId;
    private string m_iosProductId;
    private bool m_isOnlyOneBuy;

    public string productId
    {
        get
        {
#if UNITY_ANDROID
            return m_androidProductId;
#else
            return m_androidProductId;
#endif
        }
    }
    public bool isOnlyOneBuy => m_isOnlyOneBuy;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);
        m_androidProductId = toString(cells, ref i);
        m_isOnlyOneBuy = toBool(cells, dataTypes, ref i);
    }
}

public class InAppProductTable : Table<InAppProductRow>
{

}
