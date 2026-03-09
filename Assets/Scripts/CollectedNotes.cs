using UnityEngine;

public class CollectibleCircle : MonoBehaviour
{
    public string circleID;

    [Header("Audio")]
    public AudioClip collectClip;
    public float volume = 1f;


    [Header("Floating Settings")]
    public float floatAmplitude = 0.25f;   // how high it moves
    public float floatSpeed = 2f;           // how fast it moves

    private Vector3 startPosition;

    void Start()
    {
        // Save the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Create a smooth up-and-down motion using sine
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = startPosition.y + yOffset;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleNotes.Instance.Collect(circleID);
            if (collectClip != null)
            {
                AudioSource.PlayClipAtPoint(
                    collectClip,
                    Camera.main.transform.position,
                    volume
                );
            }
            gameObject.SetActive(false);
        }
    }
}