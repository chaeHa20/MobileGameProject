using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityHelper;


#if UNITY_EDITOR
#endif

public class EntityData
{
    public GameObject parent;
    public int id;
    public long uuid;
    public long localUuid;
    public int resourceId;
    public eEntity type;
    public Vector3 position;
    public Quaternion rotation;
    public eTeam team;
    public Action loadModelCallback;
}

[ExecuteInEditMode]
public class Entity : PoolObject
{
    [SerializeField] GameObject m_modelParent = null;

    private int m_id = 0;
    private long m_uuid = 0;
    private long m_localUuid = 0;
    private eEntity m_type = eEntity.None;
    private StateFunction m_stateFunction = null;
    private eTeam m_team = eTeam.None;
    private EntityModel m_model = null;
    private Transform m_transform = null; 

    public int id => m_id;
    public long uuid => m_uuid;
    public long localUuid => m_localUuid;
    public float radius => null != m_model ?m_model.radius : 0.1f;
    public float sqrtRadius => m_model.sqrtRadius;
    public eEntity type => m_type;
    public eMotion motion => m_model.motion;
    public eTeam team => m_team;
    public EntityModel model => m_model;

    public virtual Vector3 forward
    {
        get { return (null == m_model) ? Vector3.zero : m_transform.forward; }//.getForward(); }
        set { if (null != m_model) m_transform.forward = value; }// m_model.setForward(value); }
    }
    public int modelId => (null == m_model) ? 0 : m_model.id;

#if UNITY_EDITOR
    public float realtimeRadius => m_model.realtimeRadius;
#endif

    public virtual Vector3 position3
    {
        get { return m_transform.position; }
        set { m_transform.position = value; }
    }

    public virtual Vector3 localPosition3
    {
        get { return m_transform.localPosition; }
        set { m_transform.localPosition = value; }
    }

    // TODO : 2024-03-15 by pms
    public Quaternion transformRotation
    {
        get { return m_transform.rotation; }
        set { m_transform.rotation = value; }
    }
    //

    public virtual void initialize(EntityData entityData)
    {
        if (null != entityData.parent)
            GraphicHelper.setParent(entityData.parent, gameObject);

        m_transform = transform;
        m_id = entityData.id;
        m_uuid = entityData.uuid;
        m_localUuid = entityData.localUuid;
        m_type = entityData.type;
        m_team = entityData.team;
        position3 = entityData.position;
        setRotation(entityData.rotation);
        loadModel(m_id, entityData.loadModelCallback);

        initStateFunction();

#if UNITY_EDITOR
        name = string.Format("{0}_L{1}_{2}", m_type.ToString(), m_localUuid, m_uuid);
#endif
    }

    protected virtual void loadModel(int id, Action callback)
    {

    }

    protected virtual void changeModel<T>(int modelId) where T : EntityModel
    {
        GamePoolHelper.getInstance().pop<T>(modelId, (model) =>
        {
            //var rotation = (null == m_model) ? Quaternion.identity : m_model.getRotation();

            disposeModel();
            m_model = model;
            GraphicHelper.setParent(m_modelParent, model.gameObject);
            //model.transform.rotation = rotation;
            changedModel();
        });
    }

    protected virtual void changedModel()
    {

    }

    protected void changeId(int id)
    {
        m_id = id;
    }

    public virtual void startBattle()
    {

    }

    public virtual void setModelLayer()
    {

    }

    public virtual void endBattle()
    {
        //if (type == eEntity.Mercenary || type == eEntity.Drone)
        //    playMotion(eMotion.Idle);
        //else
        setMotion(eMotion.Idle);
    }

    public virtual void update(float dt)
    {
        if (null != m_stateFunction)
            m_stateFunction.update(dt);
    }

    public virtual void fixedUpdate(float dt)
    {
        if (null != m_stateFunction)
            m_stateFunction.update(dt);
    }

    // TODO : 2024-04-16 by pms
    public virtual void setMoveSpeedForRush()
    {

    }
    public virtual void setBackMoveSpeedToOrigial()
    {

    }
    //

    protected void setRotation(in Quaternion q)
    {
        m_transform.rotation = q;
    }

    public virtual void setModelScale(float s)
    {
        if (null == m_model)
            return;

        m_model.setScale(s);
    }

    public virtual void setForward(in Vector3 forward)
    {
        m_transform.forward = forward;
    }

    protected void setActiveModel(bool isActive)
    {
        if (null == m_model)
            return;

        m_model.setActive(isActive);
    }

    protected virtual void initStateFunction()
    {
        m_stateFunction = new StateFunction();
    }

    protected void addStateFunctionCase(int state, StateFunctionCase stateFunctionCase)
    {
        m_stateFunction.addCase(state, stateFunctionCase);
    }

    protected void setStateFunctionCase(int state, params object[] args)
    {
        m_stateFunction.setCase(state, args);
    }

    protected void setPositionAndRotation(Transform source)
    {
        position3 = source.position;
        setRotation(source.rotation);
    }

    public void lerpModelDir(in Vector2 dir, float t)
    {
        if (null == m_model)
            return;

        m_model.lerpModelDir(dir, t);
    }

    public void lerpModelDir(in Vector3 dir, float t)
    {
        if (null == m_model)
            return;

        m_model.lerpModelDir(dir, t);
    }

    public void setMotion(eMotion motion, float animSpeed = 1.0f)
    {
        if (null == m_model)
            return;

        m_model.setMotion(motion, animSpeed);
    }

    public void crossFadeMotion(eMotion motion)
    {
        if (null == m_model)
            return;

        m_model.crossFadeMotion(motion);
    }

    protected void playMotion(eMotion motion, float normalizedTime = 0.0f)
    {
        if (null == m_model)
            return;

        m_model.playMotion(motion, normalizedTime);
    }

    public float getMotionLength(eMotion motion)
    {
        if (null == m_model)
            return 0.0f;

        return m_model.getMotionLength(motion);
    }

    protected virtual void setModel(int modelId, EntityModel model)
    {
        disposeModel();

        model.initialize(modelId, this);
        m_model = model;

        GraphicHelper.setParent(m_modelParent, model.gameObject);
    }

    protected void setModelParentLocalScale(float localScale)
    {
        m_modelParent.transform.localScale = new Vector3(localScale, localScale, localScale);
    }

    protected void setModelParentLocalScale(Vector3 localScale)
    {
        m_modelParent.transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
    }// TODO : 2024-07-01 by pms

    public void fadeOutHud()
    {
        if (null == m_model)
            return;

        m_model.fadeOutHud();
    }

    private void disposeModel()
    {
        if (null != m_model)
        {
            m_model.Dispose();
#if UNITY_EDITOR
            setActiveModel(false);
#endif
            m_model = null;
        }
    }

    protected override void Dispose(bool disposing)
    {
        disposeModel();

        base.Dispose(disposing);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        onDrawGizmos();
    }

    protected virtual void onDrawGizmos()
    {

    }
#endif
}
