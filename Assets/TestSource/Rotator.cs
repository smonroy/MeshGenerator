using UnityEngine;

/// <summary>
/// Rotates an object over time
/// </summary>
public class Rotator : MonoBehaviour
{
    public Vector3 eulerPerSecond;
    
	private void Update()
    {
        transform.Rotate(eulerPerSecond * Time.deltaTime);
	}
}
