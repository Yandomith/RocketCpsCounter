using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The rocket to follow
    public Vector3 offset;         // Offset from the rocket (e.g., new Vector3(0, 2, -10))
    public float followSpeed = 5f; // Smooth follow speed
    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

}