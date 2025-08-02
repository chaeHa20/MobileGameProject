using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

public class CharacterModel : EntityModel
{
    [SerializeField] GameObject m_gaugeHudPosition = null;    
    [SerializeField] Transform m_center = null;
    [SerializeField] CharacterCollider m_characterCollider = null;
    [SerializeField] CharacterAnimator m_characterAnimator = null;

    protected float m_workingSpeed = 1.0f;
    protected float m_movingSpeed = 1.0f;
    private UIGauge m_loadingGauge;

    public GameObject gaugeHudPosition => m_gaugeHudPosition;
    public Transform center => (null == m_center) ? transform : m_center;    

    public override void initialize(int id, Entity entity)
    {
        base.initialize(id, entity);

        m_workingSpeed = 1.0f;
        m_movingSpeed = 1.0f;

        if (null != m_characterCollider)
            m_characterCollider.initialize(entity as Character);
        if (null != m_characterAnimator)
            m_characterAnimator.initialize(entity as Character);

#if UNITY_EDITOR
        checkExistCharacterAnimatorComponent();
#endif
    }

    public void endWokring()
    {
     
    }

    public void loadFireEffects()
    {
     
    }

    public void setModelLayer()
    {
        if (gameObject.layer != (int)eLayer.Default)
            GraphicHelper.setLayer(gameObject, (int)eLayer.Default);// TODO : 2024-07-31 by pms
    }

    public void setAnimSpeed()
    {
        if (eMotion.Move == motion)
            setAnimSpeed(m_movingSpeed);
        else
            setAnimSpeed(1.0f);
    }

    public void setHpGauge(float curTime, float workingTime)
    {
        m_loadingGauge.setValue(curTime, workingTime);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }

        base.Dispose(disposing);
    }

#if UNITY_EDITOR
    protected void checkExistCharacterAnimatorComponent()
    {
        if (null == animator)
            return;

        var characterAnimator = animator.GetComponent<CharacterAnimator>();

        if (null == characterAnimator)
        {
            if (Logx.isActive)
                Logx.error("Dont't exist CharacterAnimator component in {0}", name);
            return;
        }

        if (characterAnimator.isEmptyCharacter)
        {
            if (Logx.isActive)
                Logx.error("Is empty CharacterAnimator's character in {0}", name);
        }
    }
#endif

    public void setWorkingSpeed(float workingSpeed)
    {
        m_workingSpeed = workingSpeed;
    }

    protected virtual void setAnimatorParameters(eMotion motion)
    {
        if (eMotion.Move == motion)
            setAnimatorParameter("Speed", m_movingSpeed);
    }

    public override void setMotion(eMotion motion, float animSpeed)
    {
        setAnimatorParameters(motion);

        base.setMotion(motion, animSpeed);
    }

    public override void crossFadeMotion(eMotion motion)
    {
        setAnimatorParameters(motion);

        base.crossFadeMotion(motion);
    }

    public override void playMotion(eMotion motion, float normalizedTime = 0)
    {
        setAnimatorParameters(motion);

        base.playMotion(motion, normalizedTime);
    }
}
