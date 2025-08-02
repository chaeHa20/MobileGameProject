using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using UnityHelper;

public class Goal
{
    public enum eNeedStatus
    {
        None,
        Initialize,
        Resume
    };

    private eNeedStatus m_needStatus = eNeedStatus.Initialize;
    private AI m_ai = null;
    private long m_entityUuid = 0;
    private eGoal m_type = eGoal.None;
    protected UpdateTimer m_updateTimer = null;

    public eNeedStatus needStatus { get { return m_needStatus; } private set { m_needStatus = value; } }
    public eGoal type { get { return m_type; } set { m_type = value; } }
    public virtual string alias => "";

    public virtual void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        if (Logx.isActive)
        {
            Logx.assert(null != ai);
            Logx.assert(0 != entityUuid);
        }

        m_ai = ai;
        m_entityUuid = entityUuid;

        clearNeedStatus();
    }

    protected void initUpdateTimer(float updateTime, bool immediately)
    {
        if (null == m_updateTimer)
            m_updateTimer = new UpdateTimer();
        m_updateTimer.initialize(updateTime, immediately);
    }

    public virtual void resume(AI ai, long entityUuid, ref bool isEnd)
    {
        if (Logx.isActive)
            Logx.assert(0 != entityUuid);

        m_ai = ai;
        m_entityUuid = entityUuid;

        clearNeedStatus();
    }

    public void setResumeNeedStatus()
    {
        needStatus = eNeedStatus.Resume;
    }

    public virtual void finalize(long entityUuid)
    {
        if (Logx.isActive)
            Logx.assert(0 != entityUuid);
    }

    public virtual void update(eTeam team, float dt, ref bool isEnd)
    {
    }

    private bool isValidUuid(long entityUuid)
    {
        return EntityManager.instance.isExist(entityUuid);
    }

    private void clearNeedStatus()
    {
        needStatus = eNeedStatus.None;
    }

    public void addGoal(Goal goal)
    {
        if (null == goal)
            return;

        m_ai.addGoal(goal);
    }

    protected T getEntity<T>(long entityUuid) where T : Entity
    {
        return EntityManager.instance.getEntity<T>(entityUuid);
    }

    protected T getEntity<T>() where T : Entity
    {
        return getEntity<T>(m_entityUuid);
    }


    public virtual Goal clone()
    {
        return null;
    }

#if UNITY_EDITOR
    public virtual void onDrawGizmos()
    {

    }
#endif

}