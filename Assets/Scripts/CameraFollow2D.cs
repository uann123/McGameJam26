#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Tooltip("Time for camera to catch up. Lower = snappier.")]
    private float smoothTime = 0.08f;

    [SerializeField, Tooltip("Whether the camera should follow on the Z axis as well.")]
    private bool followZ = false;

    private Transform target;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        target = transform.parent;
        if (target == null)
        {
            Debug.LogWarning("CameraFollow2D: no parent found to follow.");
            return;
        }

        // Capture current world offset so the camera keeps its relative position to the parent
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        if (!followZ) targetPos.z = transform.position.z; // keep camera's existing Z for 2D

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}