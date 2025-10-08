
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class MapObstacles : PoolObject
{
    [SerializeField] Transform[] m_monsterSpawnPoints;

    public Transform findMonsterSpawnPoint()
    {
        if (0 == m_monsterSpawnPoints.Length)
            return null;

        var randomIndex = UnityEngine.Random.Range(0, m_monsterSpawnPoints.Length);

        return m_monsterSpawnPoints[randomIndex];
    }
}
