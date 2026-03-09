using UnityEngine;
using System.Collections.Generic;

public class CollectibleNotes : MonoBehaviour
{
    public static CollectibleNotes Instance;
    public List<string> collectedCircles = new List<string>();
    public EnemySpawnController enemySpawnController;
    public MusicTrack musicTrack;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        //else
            //Destroy(gameObject);
    }

    void Start()
    {
        enemySpawnController = FindObjectOfType<EnemySpawnController>();
        musicTrack = FindObjectOfType<MusicTrack>();

    }

    public void Collect(string id)
    {
        if (!collectedCircles.Contains(id))
        {
            collectedCircles.Add(id);
            Debug.Log("Collected: " + id);

            // Trigger enemy spawn check
            enemySpawnController.spawnEnemiesOnCollect();
            // Update the UI to reflect the collected note
            musicTrack.Refresh(collectedCircles);
        }
    }

    public void LooseNote()
    {
        if (collectedCircles.Count > 0)
        {
            string lostNote = collectedCircles[collectedCircles.Count - 1];
            collectedCircles.RemoveAt(collectedCircles.Count - 1);
            Debug.Log("Lost: " + lostNote);
            musicTrack.Refresh(collectedCircles);
            CollectibleCircle[] circles = FindObjectsOfType<CollectibleCircle>(true);
            foreach (var c in circles)
            {
                if (c.circleID == lostNote)
                {
                    c.gameObject.SetActive(true);
                    Debug.Log("Restored: " + lostNote);
                    return;
                }
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        //Debug.Log($"PlaySound called | audioSource: {audioSource} | clip: {collectSound}");

        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
