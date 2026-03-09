using UnityEngine;

public class pauseMenu : MonoBehaviour //whatever going on is in pauseMenu
{
    public GameObject PauseScreen;
    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        PauseScreen.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {TogglePause();}

    }

    void TogglePause()
    {
        isPaused = !isPaused;
        PauseScreen.SetActive(isPaused);

        if (isPaused)
        {Time.timeScale = 0f;}
        else
        {Time.timeScale = 1f;}
    }
}
