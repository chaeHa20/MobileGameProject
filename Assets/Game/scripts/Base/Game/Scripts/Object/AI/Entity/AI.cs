using System.Collections.Generic;
using System;
using System.Text;
using UnityHelper;

public class AI : IDisposable 
{
    private Stack<Goal> m_goals = new Stack<Goal>();
    private Goal m_defaultGoal = null;
    private List<Goal> m_additionalGoals = new List<Goal>();
    private long m_entityUuid = 0;

#if UNITY_EDITOR
    private string m_totalGoalAlias = "";
#endif

    public long entityUuid => m_entityUuid;

    ~AI()
    {
        Dispose(false);
    }

    public virtual void initialize(long entityUuid, Goal defaultGoal)
    {
        m_entityUuid = entityUuid;
        m_defaultGoal = defaultGoal;
        m_additionalGoals.Clear();
        m_goals.Clear();
    }

    private void checkEmptyGoal()
    {
        if (0 == m_goals.Count)
        {
            m_goals.Push(m_defaultGoal);
        }
    }
    
    public void update(eTeam team, float dt)
    {
        checkEmptyGoal();

        bool isEnd = false;

        Goal goal = m_goals.Peek();
        switch (goal.needStatus)
        {
            case Goal.eNeedStatus.Initialize: goal.initialize(this, m_entityUuid, ref isEnd); break;
            case Goal.eNeedStatus.Resume:     goal.resume(this, m_entityUuid, ref isEnd); break;
        }

        if (!isEnd)
        {
            goal.update(team, dt, ref isEnd);
        }

        if (isEnd)
        {
            popGoal();
        }

        updateAdditionalGoals();
        setTotalGoalAlias();
    }

    private void popGoal()
    {
        if (0 < m_goals.Count)
        {
            if (Logx.isActive)
                Logx.traceColor("popGoal uuid {0}, goalName {1}", "green", m_entityUuid, m_goals.Peek().type.ToString());

            m_goals.Peek().finalize(m_entityUuid);
            m_goals.Pop();
        }
        else
        {
            if (Logx.isActive)
                Logx.traceColor("popGoal empty", "green");
        }
    }

    public void clearGoal()
    {
        m_goals.Clear();
        m_additionalGoals.Clear();
    }

    public void addGoal(Goal goal, bool isImmediately = false)
    {
        if (isImmediately)
        {
            if (0 < m_goals.Count)
            {
                var lastGoal = m_goals.Peek();
                if (lastGoal.type == goal.type)
                    m_goals.Pop();
                else
                    lastGoal.setResumeNeedStatus();
            }

            m_goals.Push(goal);
        }
        else
        {
            if (0 < m_additionalGoals.Count)
            {
                if (m_additionalGoals[m_additionalGoals.Count - 1].type == goal.type)
                    m_additionalGoals.RemoveAt(m_additionalGoals.Count - 1);
            }

            m_additionalGoals.Add(goal);
        }

        if (Logx.isActive)
            Logx.traceColor("addGoal uuid {0}, goalType {1}, goalCount {2}, addGoalCount {3}, immediately {4}", "green", m_entityUuid, goal.type, m_goals.Count, m_additionalGoals.Count, isImmediately);
    }

    private void updateAdditionalGoals()
    {
        if (0 == m_additionalGoals.Count)
            return;

        if (0 < m_goals.Count)
        {
            m_goals.Peek().setResumeNeedStatus();
        }

        foreach (var goal in m_additionalGoals)
        {
            if (0 < m_goals.Count)
            {
                if (m_goals.Peek().type == goal.type)
                    m_goals.Pop();
            }

            m_goals.Push(goal);
        }

        if (Logx.isActive)
            Logx.traceColor("clear additionalGoals uuid {0}", m_entityUuid, "green");

        m_additionalGoals.Clear();        
    }

    public string getCurGoalName()
    {
        eGoal goal = getCurGoalType();
        return goal.ToString();
    }

    public eGoal getCurGoalType()
    {
        if (0 == m_goals.Count)
            return eGoal.None;

        return m_goals.Peek().type;
    }

    public string getGoalAlias()
    {
#if UNITY_EDITOR
        return m_totalGoalAlias;
#else
        return "";
#endif
    }

    private void setTotalGoalAlias()
    {
#if UNITY_EDITOR
        var sb = new StringBuilder();
        var e = m_goals.GetEnumerator();
        while (e.MoveNext())
        {
            sb.Append(e.Current.alias + "/");
        }

        m_totalGoalAlias = sb.ToString();
#endif
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach(Goal goal in m_goals)
            {
                goal.finalize(m_entityUuid);
            }

            m_goals.Clear();
        }
    }

#if UNITY_EDITOR
    public void onDrawGizmos()
    {
        if (0 < m_goals.Count)
            m_goals.Peek().onDrawGizmos();
    }
#endif
}
