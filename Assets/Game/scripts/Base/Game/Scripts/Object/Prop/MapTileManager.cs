using UnityEngine;
using UnityHelper;
using System.Collections.Generic;

public class MapTileManager : MonoSingleton<MapTileManager>
{
    private Dictionary<Vector2, MapTile> m_tiles = new Dictionary<Vector2, MapTile>();

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

        m_tiles.Add(targetPos, tile);
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
