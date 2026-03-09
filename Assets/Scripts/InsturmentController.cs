using UnityEngine;

public class InsturmentController : MonoBehaviour
{
    public CollectibleNotes collectibleNotes;
    public GameObject instrument;
    public Transform insturmentSpawnPosition;

    private bool spawned = false;
    private GameObject spawnedInstrument;
    public int numOfCollectedToSpawn; 

    void Start()
    {
        collectibleNotes = FindObjectOfType<CollectibleNotes>();
    }

    void Update()
    {
        if (spawned) return;
        if (collectibleNotes == null) return;

        if (collectibleNotes.collectedCircles.Count >= numOfCollectedToSpawn)
        {
            spawned = true;

            DisableAllEnemies();
            spawnedInstrument = Instantiate(
                instrument,
                insturmentSpawnPosition.position,
                insturmentSpawnPosition.rotation
            );
        }
    }

    void DisableAllEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var go in enemies)
            go.SetActive(false);
    }
}