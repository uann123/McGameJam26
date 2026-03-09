using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DelayedScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] float delay = 7f;

    void Start()
    {
        StartCoroutine(ChangeAfterDelay());
    }

    IEnumerator ChangeAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
