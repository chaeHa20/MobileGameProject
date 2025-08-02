using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[Serializable]
public class LocalMember
{
    [SerializeField] int m_id = 0;
    [SerializeField] long m_uuid = 0;

    public int id => m_id;
    public long uuid => m_uuid;

    public virtual void initialize(int id)
    {
        m_id = id;
        m_uuid = LocalUuidData.instance.makeUuid();
    }

    public void changeId(int id)
    {
        m_id = id;
    }

    public virtual void serialize() { }
    public virtual void deserialize() { }
}

[Serializable]
public class LocalMembers<T> : IDisposable where T : LocalMember, new()
{
    [SerializeField] List<T> m_members = new List<T>();

    public int memberCount => m_members.Count;
    public List<T> members => m_members;// TODO : 2024-05-15 by pms

    public bool isExistMember(int id)
    {
        return m_members.Exists((c) => c.id == id);
    }

    public bool addMember(T t)
    {
        if (isExistMember(t.id))
            return false;

        m_members.Add(t);
        return true;
    }

    public T findMember(int id)
    {
        var c = (from entity in m_members
                 where entity.id == id
                 select entity).FirstOrDefault();

        return c;
    }

    public T findMember(long uuid)
    {
        var c = (from entity in m_members
                 where entity.uuid == uuid
                 select entity).FirstOrDefault();

        return c;
    }

    public T getMember(int index)
    {
        if (0 > index || memberCount <= index)
            return null;

        return m_members[index];
    }

    public T getFirstMember()
    {
        return getMember(0);
    }

    public T getLastMember()
    {
        return getMember(memberCount - 1);
    }

    public int findMemberIndex(int id)
    {
        var index = m_members.FindIndex(x => x.id == id);
        return index;
    }

    public bool removeMember(int id)
    {
        var index = findMemberIndex(id);
        return removeMemberAt(index);
    }

    public bool removeMemberAt(int index)
    {
        if (0 > index || memberCount <= index)
            return false;

        m_members.RemoveAt(index);

        return true;
    }

    protected void clearMembers()
    {
        m_members.Clear();
    }

    public void forEachMembers(Action<T> callback)
    {
        if (null == callback)
            return;

        foreach(var member in m_members)
        {
            callback(member);
        }
    }

    public List<T>.Enumerator getMemberEnumerator()
    {
        return m_members.GetEnumerator();
    }

    public List<T> findMembersLinq(Func<T, bool> callback)
    {
        var r = (from member in m_members
                    where callback(member)
                    select member).ToList();

        return r;
    }

    public T findMemberLinq(Func<T, bool> callback)
    {
        var r = (from member in m_members
                 where callback(member)
                 select member).FirstOrDefault();

        return r;
    }

    public virtual void serialize()
    {
        foreach(var member in m_members)
        {
            member.serialize();
        }
    }

    public virtual void deserialize() 
    {
        foreach (var member in m_members)
        {
            member.deserialize();
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            clearMembers();
        }
    }
}