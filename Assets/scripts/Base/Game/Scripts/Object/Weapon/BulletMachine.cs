using System;
using System.Collections;
using UnityEngine;

public class BulletMachine : MonoBehaviour
{
    [SerializeField] GameObject m_firePoint;
    [SerializeField] float m_fireInterval = 0.0f;

    private int m_bulletModelId = 0;
    private eFireMachineType m_machineType;

    public virtual void initialize(Character owner, int bulletId)
    {
    }

    public virtual void fire(float raycastMaxDistance, bool isLoadFireEffect, Action callback)
    {
        StartCoroutine(coFire(raycastMaxDistance, isLoadFireEffect, callback));
    }

    IEnumerator coFire(float raycastMaxDistance, bool isLoadFireEffect, Action callback)
    {
        fireBullet(raycastMaxDistance, isLoadFireEffect);
        if (0 < m_fireInterval)
            yield return new WaitForSeconds(m_fireInterval);

        callback?.Invoke();
    }

    protected virtual void fireBullet(float raycastMaxDistance, bool isLoadFireEffect)
    {
        //if (isLoadFireEffect)
        //    loadFireEffect(firePoint);
    }
}
