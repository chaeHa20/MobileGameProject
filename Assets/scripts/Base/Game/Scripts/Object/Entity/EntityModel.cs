using UnityEngine;
using UnityHelper;

public class EntityModel : PoolObject
{
    [SerializeField] GameObject m_radiusObject = null;
    [SerializeField] Animator m_animator = null;

    private float m_radius = 0.0f;
    private float m_sqrtRadius = 0.0f;
    private Transform m_transform = null;
    private int m_id = 0;

    public int id => m_id;
    public float radius => m_radius;
    public float sqrtRadius => m_sqrtRadius;
    public eMotion motion => (null == m_animator) ? eMotion.Idle : (eMotion)m_animator.GetInteger("state");
    public float animationSpeed => m_animator.speed;
    protected Animator animator => m_animator;

#if UNITY_EDITOR
    public float realtimeRadius
    {
        get
        {
            var r = Vector3.Distance(Vector3.zero, m_radiusObject.transform.localPosition);
            return r * m_radiusObject.transform.lossyScale.x;
        }
    }
#endif

    protected virtual void Awake()
    {
        m_transform = transform;
    }

    public virtual void initialize(int id, Entity entity)
    {
        m_id = id;
        //playMotion(eMotion.Idle, Random.value);
#if UNITY_EDITOR
        if (null == entity.model)
            setMotion(eMotion.Idle, 1.0f);
        else
            setMotion(entity.motion, 1.0f);
#else
        setMotion(eMotion.Idle, 1.0f);
#endif
        setRadius();
    }

    private void setRadius()
    {
        if (null == m_radiusObject)
            return;

        // scale이 변경되었을 수 있기 때문에 world 좌표로 하자.
        m_radius = Vector3.Distance(transform.position, m_radiusObject.transform.position);
        m_radius *= m_radiusObject.transform.lossyScale.x;
        m_sqrtRadius = m_radius * m_radius;
    }

    public void setActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    protected void setRotation(in Quaternion q)
    {
        m_transform.rotation = q;
    }

    protected Quaternion getRotation()
    {
        return m_transform.rotation;
    }

    public void setScale(float s)
    {
        m_transform.localScale = new Vector3(s, s, s);

        setRadius();
    }

    public Vector3 getScale()
    {
        return m_transform.localScale;
    }

    protected void setForward(in Vector3 forward)
    {
        m_transform.forward = forward;
    }

    protected Vector3 getForward()
    {
        return m_transform.forward;
    }

    public void lerpModelDir(in Vector2 dir, float t)
    {
        lerpModelDir(new Vector3(dir.x, 0.0f, dir.y), t);
    }

    public void lerpModelDir(in Vector3 dir, float t)
    {
        m_transform.forward = Vector3.Lerp(m_transform.forward, dir, t);
    }

    public virtual void setMotion(eMotion motion, float animSpeed)
    {
        if (null != m_animator)
        {
            m_animator.SetInteger("state", (int)motion);
            m_animator.SetFloat("Speed", animSpeed);
            m_animator.speed = animSpeed;
        }
    }

    public void setRandomMotion()
    {
        if (null != m_animator)
        {
            var value = UnityEngine.Random.Range(0, 11);
            m_animator.SetInteger("Result", value);
        }
    }
    
    public void setAnimSpeed(float animSpeed)
    {
        if (null != m_animator)
        {
            m_animator.speed = animSpeed;
            m_animator.SetFloat("Speed", animSpeed);
        }
    }
    
    public virtual void crossFadeMotion(eMotion motion)
    {
        if (null != m_animator)
        {
            m_animator.SetInteger("state", (int)motion);
            m_animator.CrossFade(motion.ToString(), 0.3f, 0, Random.value);
        }
    }

    public virtual void playMotion(eMotion motion, float normalizedTime = 0.0f)
    {
        if (null != m_animator)
        {
            m_animator.SetInteger("state", (int)motion);
            m_animator.Play(motion.ToString(), 0, normalizedTime);
        }
    }

    public float getMotionLength(eMotion motion)
    {
        if (null == m_animator)
            return 0.0f;

        return GraphicHelper.getLengthContainClipName(m_animator, motion.ToString());
    }

    protected void setAnimatorParameter(string parameterName, float parameterValue)
    {
        if (null != m_animator)
        {
            m_animator.SetFloat(parameterName, parameterValue);
        }
    }

    public virtual void setFocused(bool isFocused)
    {

    }

    public virtual void fadeOutHud()
    {

    }

    public int getEventCount(string functionName)
    {
        if (null == m_animator)
            return 0;

        int eventCount = 0;

        var clips = m_animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            var events = clip.events;
            foreach (var e in events)
            {
                if (functionName == e.functionName)
                {
                    ++eventCount;
                }
            }
        }

        return eventCount;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        onDrawGizmos();
    }

    protected virtual void onDrawGizmos()
    {
        var gizmo = DebugSettings.instance.gizmo;

        if (gizmo.isDrawEntityRadius)
        {
            float radius = 0.0f;
            if (null != m_radiusObject)
                radius = Vector3.Distance(transform.position, m_radiusObject.transform.position);

            EditorHelper.drawGizmoCircle(transform.position, radius, Color.red);
        }
    }
#endif
}
