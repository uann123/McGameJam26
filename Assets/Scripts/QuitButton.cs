using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Quit()
    {
        Debug.Log("Quit button clicked");

        // If running in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running as a built game
        Application.Quit();
#endif
    }
}
