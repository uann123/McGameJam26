using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public Vector3 platformVelocity { get; private set; }

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        platformVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
    }
}
