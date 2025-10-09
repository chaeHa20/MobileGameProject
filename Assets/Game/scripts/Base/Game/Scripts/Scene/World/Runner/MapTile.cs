using System;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

[Serializable]
public enum eMapTile
{
    None = 0,
    Top = 1,
    Bottom = 2,
    Left = 3,
    Rigjt = 4,
}


public class MapTile : PoolObject
{
    [SerializeField] Vector2 m_myPosition = Vector2.zero;

    [SerializeField] Transform m_top;
    [SerializeField] Transform m_bottom;
    [SerializeField] Transform m_left;
    [SerializeField] Transform m_right;
    [SerializeField] GameObject m_obstacleParent;

    [Header("DeadZone Transform")]
    [SerializeField] Transform m_deadZone;

    private GameObject m_mapParent;
    private MapObstacles m_obstacle;
    public GameObject obstacleParent => m_obstacleParent;
    

    public virtual void initialize(Vector2 position, GameObject parent)
    {
        m_myPosition = position;
        m_mapParent = parent;

        loadDeadZone();
    }

    public void spawnMonster(int monsterId)
    {
        if (null == m_obstacle)
            return;
        var spawnPoint = m_obstacle.findMonsterSpawnPoint();
    }

    public void loadNewTile(eMapTile targetType)
    {
        if (MapTileManager.instance.isExistMapTile(m_myPosition, targetType))
            return;

        GamePoolHelper.getInstance().pop<MapTile>(eResource.MapTile, (tile) =>
        {
            var targetPosition = findTargetPosition(m_myPosition, targetType);
            GraphicHelper.setParent(m_mapParent, tile.gameObject);
            tile.transform.position = findTargetPosition(targetType);
            tile.initialize(targetPosition, m_mapParent);

            MapTileManager.instance.addMapTile(targetPosition, tile);
            MapTileManager.instance.removeFarestTilePoint(m_myPosition);
            loadMapModel(tile, null);
        });
    }


    public void loadDeadZone() // 타일 맵 상에 사망판정판 생성
    {
        if (MapTileManager.instance.isExistDeadZone(m_myPosition))
            return;

        GamePoolHelper.getInstance().pop<MapTileDeadZone>(eResource.MapTileDeadZone, (deadZone) =>
        {
            deadZone.initialize(m_myPosition, m_deadZone);
        });
    }

    protected virtual void loadMapModel(MapTile parent,Action callback) // 타일 맵 상에 오브젝트들 랜덤 생성
    {
        var randomIndex = UnityEngine.Random.Range(0, Define.MAX_ObBSTACLE_COUNT);
        GamePoolHelper.getInstance().pop<MapObstacles>(eResource.MapObstacles, randomIndex, (obstacle) =>
        {
            GraphicHelper.setParent(parent.obstacleParent, obstacle.gameObject);

            m_obstacle = obstacle;
            callback?.Invoke();
        });
    }

    protected Vector3 findTargetPosition(eMapTile targetType)
    {
        var result = transform.position;
        switch (targetType)
        {
            case eMapTile.Top:
                result = m_top.position;
                break;
            case eMapTile.Bottom:
                result = m_bottom.position;
                break;
            case eMapTile.Left:
                result = m_left.position;
                break;
            case eMapTile.Rigjt:
                result = m_right.position;
                break;
            default:
                break;
        }

        return result;
    }

    protected Vector2 findTargetPosition(Vector2 currentPos, eMapTile targetType)
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
}
