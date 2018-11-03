using UnityEngine;

/// <summary>
/// Moves the node back and forth along a patrol path.
/// </summary>
public class Patrol : MonoBehaviour
{
    public Transform target;

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        startingPosition = transform.position;
        targetPosition = target.position;
    }

    private void Update()
    {
        float t = 1f - ((Mathf.Cos(Time.time) + 1) * 0.5f);
        transform.position = Vector3.Lerp(startingPosition, targetPosition, t);
    }
}
