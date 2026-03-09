using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneChange : MonoBehaviour
{
    [SerializeField] string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
