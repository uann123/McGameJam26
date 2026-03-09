using UnityEngine;

public class DelayedCanvas : MonoBehaviour
{
    public GameObject myCanvas;

    void Start()
    {
        myCanvas.SetActive(false);
        Invoke(nameof(ShowCanvas), 5f);
    }

    void ShowCanvas()
    {myCanvas.SetActive(true);}
}
