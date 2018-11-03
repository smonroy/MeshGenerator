using UnityEngine;

/// <summary>
/// Represents a graph edge
/// </summary>
public class Edge : MonoBehaviour
{
    public Point point1;
    public Point point2;

    private void OnDrawGizmos()
    {
        if (point1 == null || point2 == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.transform.position, point2.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        if (point1 == null || point2 == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(point1.transform.position, point2.transform.position);
    }
}
