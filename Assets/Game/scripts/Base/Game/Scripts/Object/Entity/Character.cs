using System.Collections.Generic;
using UnityEngine;
using UnityHelper;


public class CharacterData : AbilityEntityData
{
    public float scale = 1.0f;
    public int level;
}

public partial class Character : AbilityEntity
{
    public enum eStatus
    {
        None,
        Idle,
        Move,
        Chase,
        Attacking,
    };

    private Vector2 m_createPosition2 = Vector3.zero;
    private int m_level = 1;

    public CharacterModel characterModel => model as CharacterModel;

    public Transform center => characterModel.center;
    public int level => m_level;

    public Vector2 position2
    {
        get { return GameHelper.toVector2(position3); }
        set { base.position3 = GameHelper.toVector3(value, position3.y); }
    }

    private void Awake()
    {
        initNavMeshAgent();
    }

    public override void initialize(EntityData entityData)
    {
        var d = entityData as CharacterData;
        // base.initialize 호출 전에 설정돼야 된다.
        setModelParentLocalScale(d.scale);
        setEnableNavMesh(false);

        m_createPosition2 = new Vector2(d.position.x, d.position.z);
        m_level = d.level;

        base.initialize(entityData);

        initAttackData();
        initAI();
        initNavMeshAgent(entityData.position);
    }

    protected override void setModel(int id, EntityModel model)
    {
        base.setModel(id, model);

        setActiveModel(true);
    }

    public override void setModelLayer()
    {
        if (null != characterModel)
            characterModel.setModelLayer();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
