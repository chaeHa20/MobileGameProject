using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityHelper;

public class MapTileManager : MonoSingleton<MapTileManager>
{
    [SerializeField] NavMeshSurface m_navMeshSurface;
    [SerializeField] GameObject m_monsterSpawnParent;
    [SerializeField] int m_minTileCount = 16;
    [SerializeField] int m_maxMonsterCount = 16;
    [SerializeField] float m_monsterSpawnTime = 5.0f;

    private UpdateTimer m_monsterSpawnTimer;
    private bool m_isStartMonsterSpawn = false;
    private Dictionary<Vector2, MapTile> m_tiles = new Dictionary<Vector2, MapTile>();
    private Dictionary<Vector2, MapTileDeadZone> m_deadZone = new Dictionary<Vector2, MapTileDeadZone>();

    public int maxMonsterCount => m_maxMonsterCount;

    protected override void Awake()
    {
        base.Awake();
        m_isStartMonsterSpawn = false;
    }

    private void Update()
    {
        if (!m_isStartMonsterSpawn)
            return;

        if (m_monsterSpawnTimer.update(Time.deltaTime))
        {
            spawnMonster();
            initMonsterSpawnTimer();
        }
    }

    public void buildNavMesh()
    {
        if (null != m_navMeshSurface)
            m_navMeshSurface.BuildNavMesh();
    }

    public void initMonsterSpawnTimer()
    {
        var spawnUpdateTime = m_monsterSpawnTime;// 확장된 맵 상태에 따른 스폰 시간 변동 식 논의 필요
        if (null == m_monsterSpawnTimer)
            m_monsterSpawnTimer = new UpdateTimer();

        m_monsterSpawnTimer.initialize(spawnUpdateTime, false);

        m_isStartMonsterSpawn = true;
    }

    public bool isExistMapTile(Vector2 myPosition, eMapTile targetType)
    {
        var targetPos = findTargetPosition(myPosition, targetType);
        return m_tiles.ContainsKey(targetPos);
    }

    public MapTile findMapTile(Vector2 myPosition, eMapTile targetType)
    {
        var targetPos = findTargetPosition(myPosition, targetType);
        if (m_tiles.TryGetValue(targetPos, out MapTile tile))
            return tile;
        else
            return null;
    }

    public void addMapTile(Vector2 targetPos, MapTile tile)
    {
        if (1 == m_tiles.Count)
            initMonsterSpawnTimer();

        if (m_tiles.ContainsKey(targetPos))
        {
            m_tiles[targetPos].Dispose();
            m_tiles.Remove(targetPos);
        }

        buildNavMesh();
        m_tiles.Add(targetPos, tile);
    }

    public void addDeadZone(Vector2 targetPos, MapTileDeadZone deadZone)
    {
        if (m_deadZone.ContainsKey(targetPos))
        {
            m_deadZone[targetPos].Dispose();
            m_deadZone.Remove(targetPos);
        }

        m_deadZone.Add(targetPos, deadZone);
    }

    public bool isExistDeadZone(Vector2 targetPos)
    {
        return m_deadZone.ContainsKey(targetPos);
    }

    public void removeFarestTilePoint(Vector2 currentPos)
    {
        Vector2 maxDistancePoint = currentPos;
        var maxDistance = 0.0f;
        var e = m_tiles.GetEnumerator();
        while (e.MoveNext())
        {
            var point = e.Current.Key;
            var distance = Vector2.Distance(currentPos, point);
            if (maxDistance < distance)
            {
                maxDistance = distance;
                maxDistancePoint = point;
            }
        }

        if (maxDistancePoint == currentPos)
            return;
        else
            removeMapTile(maxDistancePoint);
    }

    private void removeMapTile(Vector2 targetPos)
    {
        if (m_tiles.Count <= m_minTileCount)
            return;

        if (m_tiles.ContainsKey(targetPos))
        {
            m_tiles[targetPos].Dispose();
            m_tiles.Remove(targetPos);
        }

        buildNavMesh();
    }

    private Vector2 findTargetPosition(Vector2 currentPos, eMapTile targetType)
    {
        var result = currentPos;
        switch (targetType)
        {
            case eMapTile.Top:
                result = new Vector2(currentPos.x, currentPos.y + 1.0f);
                break;
            case eMapTile.Bottom:
                result = new Vector2(currentPos.x, currentPos.y - 1.0f);
                break;
            case eMapTile.Left:
                result = new Vector2(currentPos.x - 1.0f, currentPos.y);
                break;
            case eMapTile.Rigjt:
                result = new Vector2(currentPos.x + 1.0f, currentPos.y);
                break;
            default:
                break;
        }

        return result;
    }

    private void spawnMonster()
    {
        var nearPoint = FindNearestTilePoint();
        if (m_tiles.ContainsKey(nearPoint))
            m_tiles[nearPoint].spawnMonster(2, m_monsterSpawnParent);
    }

    public Vector2 FindNearestTilePoint()
    {
        Vector2 currentPos = GameHelper.toVector2(PlayerManager.instance.playerCharacter.position);
        Vector2 minDistancePoint = FindFarTilePoint(currentPos);
        var minDistance = float.MaxValue;
        var e = m_tiles.GetEnumerator();
        while (e.MoveNext())
        {
            var point = e.Current.Key;
            if (!e.Current.Value.isExistObstacle)
                continue;

            var distance = Vector2.Distance(currentPos, point);
            if (distance < minDistance)
            {
                minDistance = distance;
                minDistancePoint = point;
            }
        }

        return minDistancePoint;
    }

    public Vector2 FindFarTilePoint(Vector2 currentPos)
    {
        Vector2 maxDistancePoint = currentPos;
        var maxDistance = 0.0f;
        var e = m_tiles.GetEnumerator();
        while (e.MoveNext())
        {
            var point = e.Current.Key;
            if (!e.Current.Value.isExistObstacle)
                continue;

            var distance = Vector2.Distance(currentPos, point);
            if (maxDistance < distance)
            {
                maxDistance = distance;
                maxDistancePoint = point;
            }
        }

        return maxDistancePoint;
    }
}
