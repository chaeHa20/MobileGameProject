using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalPoolHelper<T> : MonoBehaviour where T : Component
{
    private Stack<T> m_pools = new Stack<T>();

    /* 일반화를 해보자,,
    private static LocalPoolHelper<T> m_instance = null;
    public static LocalPoolHelper<T> instance
    {
        get
        {
            if (null == m_instance)
            {
                GameObject gameObject = new GameObject(typeof(LocalPoolHelper<T>).Name);
                gameObject.transform.SetParent(null);
                m_instance = gameObject.AddComponent<LocalPoolHelper<T>>();
            }

            return m_instance;
        }
    }
    */

    public void push(T t)
    {
#if UNITY_EDITOR
        if (isExist(t))
        {
            if (Logx.isActive)
                Logx.error("Failed push {0}, exist instanceID {1}", name, t.GetInstanceID());
            return;
        }
#endif

        UIHelper.instance.setParent(gameObject, t.gameObject, SetParentOption.notFullAndNotReset());
        t.gameObject.SetActive(false);
        m_pools.Push(t);
    }

    public T pop(GameObject parent)
    {
        if (isEmpty())
        {
            return createPool(parent);
        }
        else
        {
            return popPool(parent);
        }
    }

    private bool isEmpty()
    {
        return 0 == m_pools.Count;
    }

    public void clear()
    {
        m_pools.Clear();
    }

    protected virtual T createPool(GameObject parent)
    {
        return null;
    }

    protected virtual T popPool(GameObject parent)
    {
        var t = m_pools.Pop();
        t.gameObject.SetActive(true);
        return t;
    }

#if UNITY_EDITOR
    private bool isExist(T t)
    {
        var e = m_pools.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.GetInstanceID() == t.GetInstanceID())
                return true;
        }

        return false;
    }
#endif
}