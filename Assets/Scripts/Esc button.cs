using UnityEngine;

public class EscPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupImage; // drag your Image GameObject here

    private void Start()
    {
        if (popupImage != null)
            popupImage.SetActive(false); // start hidden
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (popupImage != null)
                popupImage.SetActive(true); // show it
        }
    }
}
