using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject enemyPrefab;
    [Serializable]
    public class SpawnWave
    {
        [Tooltip("When collected circles reaches this number, this wave triggers.")]
        public int triggerOnNumOfCollected;
        private bool triggered = false;

        [Tooltip("Drag empty GameObjects here. Enemies will spawn at their Transform positions.")]
        public List<GameObject> spawnPoints = new List<GameObject>();

        // Convenience: get positions quickly
        public IEnumerable<Vector3> GetSpawnPositions()
        {
            foreach (var go in spawnPoints)
            {
                if (go != null) yield return go.transform.position;
            }
        }

        public void setStatusToTriggered()
        {
            triggered = true;
        }

        public bool isTriggered()
        {
            return triggered;
        }
    }

    [Header("Waves (add as many as you want)")]
    public List<SpawnWave> spawnWaves = new List<SpawnWave>();

    // Create a onCollect event, and sucscribe to it
    // Whever a note is collected, check if any wave should be triggered
    public void spawnEnemiesOnCollect()
    {
        int numCollected = CollectibleNotes.Instance.collectedCircles.Count;
        foreach (var wave in spawnWaves)
        {
            if (!wave.isTriggered() && numCollected >= wave.triggerOnNumOfCollected)
            {
                // Spawn enemies at the designated spawn points
                foreach (var pos in wave.GetSpawnPositions())
                {
                    Instantiate(enemyPrefab, pos, Quaternion.identity);
                }
                wave.setStatusToTriggered();
            }
        }
    }
}