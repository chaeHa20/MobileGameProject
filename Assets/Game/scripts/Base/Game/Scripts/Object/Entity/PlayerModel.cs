using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

public class PlayerModel : EntityModel
{
    [SerializeField] GameObject m_gaugeHudPosition = null;
    [SerializeField] GameObject m_subCharacterPivot = null;
    [SerializeField] Transform m_center = null;

    public GameObject gaugeHudPosition => m_gaugeHudPosition;
    public GameObject subCharacterPivot => m_subCharacterPivot;
    public Transform center => (null == m_center) ? transform : m_center;    


    public override void initialize(int id, Entity entity)
    {
        base.initialize(id, entity);
    }

    public void setModelLayer()
    {
        if (gameObject.layer != (int)eLayer.Default)
            GraphicHelper.setLayer(gameObject, (int)eLayer.Default);// TODO : 2024-07-31 by pms
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }

        base.Dispose(disposing);
    }

    public override void setMotion(eMotion motion, float animSpeed)
    {
        base.setMotion(motion, animSpeed);
    }

    public override void crossFadeMotion(eMotion motion)
    {
        base.crossFadeMotion(motion);
    }

    public override void playMotion(eMotion motion, float normalizedTime = 0)
    {
        base.playMotion(motion, normalizedTime);
    }
}
