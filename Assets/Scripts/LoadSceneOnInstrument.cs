using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnInstrument : MonoBehaviour
{
    public string nextSceneName;
    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}