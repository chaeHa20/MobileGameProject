using System;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;


public class MapTileDeadZone : PoolObject
{
    private Vector2 m_myPosition = Vector2.zero;

    public Vector2 myPosition => m_myPosition;

    public virtual void initialize(Vector2 position, Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        m_myPosition = position;
        MapTileManager.instance.addDeadZone(m_myPosition, this);
    }

    private void OnTriggerExit(Collider other)
    {
        // 몬스터 제거 코드 추가
    }
}
