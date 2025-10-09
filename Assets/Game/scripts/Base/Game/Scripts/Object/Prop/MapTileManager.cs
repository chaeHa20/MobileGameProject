using UnityEngine;
using Unity.AI.Navigation;
using UnityHelper;
using System.Collections.Generic;

public class MapTileManager : MonoSingleton<MapTileManager>
{
    [SerializeField] NavMeshSurface m_navMeshSurface;
    [SerializeField] int m_minTileCount = 16;

    private Dictionary<Vector2, MapTile> m_tiles = new Dictionary<Vector2, MapTile>();
    private Dictionary<Vector2, MapTileDeadZone> m_deadZone = new Dictionary<Vector2, MapTileDeadZone>();

    public void buildNavMesh()
    {
        if(null != m_navMeshSurface)
            m_navMeshSurface.BuildNavMesh();
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
        if(m_tiles.ContainsKey(targetPos))
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
            if(maxDistance < distance)
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
                result = new Vector2(currentPos.x -1.0f, currentPos.y);
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
