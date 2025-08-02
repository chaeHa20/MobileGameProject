using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityHelper;
using static AISettings;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "App/AISettings instance")]
public class AISettings : ScriptableObject
{
    private static AISettings m_instance = null;

    public static AISettings instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = Resources.Load<AISettings>("Settings/AISettings");
            }

            return m_instance;
        }
    }

    [Serializable]
    public class Options
    {
        public float updateTime = 0.1f;
    }

    [Serializable]
    public class IdleGoal : Options
    {

    }

    [Serializable]
    public class DecoNpcGoal : Options
    {

    }

    [Serializable]
    public class CustomerOrderingGoal : Options
    {

    }

    [Serializable]
    public class DefaultGoal : Options
    {
        
    }

    [Serializable]
    public class SearchTargetGoal : Options
    {

    }

    [Serializable]
    public class FollowGoal : Options
    {
        [Tooltip("해당 거리보다 작게 있을 경우 따라가지 않고 대기한다.")]
        public float sqrtMinFollowTargetDistance = 0.4f;
    }

    [Serializable]
    public class KnockBack : Options
    {
        [Tooltip("뒤로 밀리는 거리")]
        public float len = 0.3f;
        [Tooltip("뒤로 밀리는 속도")]
        public float v = 1.0f;
    }

    [Serializable]
    public class MoveGoal
    {
        public float cantMoveWaitTimer = 1.0f;
        public float outOfTargetSqrtDistance = 3.0f;
        public float outOfTargetUpdateTime = 1.0f;
        public fMinMax lookAtTargetSpeedForMove = new fMinMax(1.0f, 18.0f);
    }
    
    [Serializable]
    public class Moving
    {
        [SerializeField] float m_maxForce = 0.2f;
        public float maxForce { get { return m_maxForce; }}
    }


    [Serializable]
    public class OrderingGoal
    {
        public float lookAtTargetSpeedForOrdering = 10.0f;
    }

    [SerializeField] IdleGoal m_idleGoal = new IdleGoal();
    [SerializeField] DecoNpcGoal m_decoNpcGoal = new DecoNpcGoal();
    [SerializeField] CustomerOrderingGoal m_customerOrderingGoal = new CustomerOrderingGoal();
    [SerializeField] DefaultGoal m_defaultGoal = new DefaultGoal();
    [SerializeField] SearchTargetGoal m_searchTargetGoal = new SearchTargetGoal();
    [SerializeField] FollowGoal m_followGoal = new FollowGoal();
    [SerializeField] KnockBack m_knockBack = new KnockBack();
    [SerializeField] MoveGoal m_moveGoal = new MoveGoal();
    [SerializeField] OrderingGoal m_orderingGoal = new OrderingGoal();
    [SerializeField] Moving m_moving = new Moving();

    public IdleGoal idleGoal => m_idleGoal;
    public DecoNpcGoal decoNpcGoal => m_decoNpcGoal;
    public CustomerOrderingGoal customerOrderingGoal => m_customerOrderingGoal;
    public DefaultGoal defaultGoal => m_defaultGoal;
    public SearchTargetGoal searchTargetGoal => m_searchTargetGoal;
    public FollowGoal followGoal => m_followGoal;
    public KnockBack knockBack => m_knockBack;
    public MoveGoal moveGoal => m_moveGoal;
    public OrderingGoal orderingGoal => m_orderingGoal;
    public Moving moving => m_moving;

#if UNITY_EDITOR
    [MenuItem("Settings/AISettings")]
    static void Settings()
    {
        Selection.activeObject = instance;
    }

    public static void create()
    {
        if (null == instance)
        {
            var asset = ScriptableObject.CreateInstance<AISettings>();

            var name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Game/Resources/Settings/AISettings.asset");
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
