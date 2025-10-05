using UnityEngine;


public class TileTrigger : MonoBehaviour
{
    [SerializeField] eMapTile m_type;
    [SerializeField] MapTile m_tile;

    private bool m_isTriggerEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (m_isTriggerEnter)
            return;
        if (other.CompareTag("Player"))
        {
            m_isTriggerEnter = true;
            m_tile.loadNewTile(m_type);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_isTriggerEnter)
            return;
        if (other.CompareTag("Player"))
        {
            m_isTriggerEnter = false;
        }
    }
}
