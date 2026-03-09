using UnityEngine;

public class MovePlatBehaviorL : MonoBehaviour
{
    [Tooltip("Total distance to move from the start position along the X axis.")]
    [SerializeField] private float distance = 6f;
    [Tooltip("Speed of the movement (higher is faster).")]
    [SerializeField] private float speed = 1f;
    [Tooltip("If true, platform will move back and forth. If false, it will move one way and stop.")]
    [SerializeField] private bool pingPong = true;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingToTarget = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.left * distance;
    }

    void Update()
    {
        if (pingPong)
        {
            // Move back and forth between startPos and targetPos
            float t = Mathf.PingPong(Time.time * -speed, 1f);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
        }
        else
        {
            // Move from startPos to targetPos once
            if (movingToTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetPos) < 0.001f) movingToTarget = false;
            }
        }
    }
}