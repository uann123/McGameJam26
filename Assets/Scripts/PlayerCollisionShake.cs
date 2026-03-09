using UnityEngine;

public class PlayerCollisionShake : MonoBehaviour
{
    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }



void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Enemy"))
    {
        Debug.Log("Collision with Enemy detected, triggering camera shake.");
        cameraShake.Shake();
    }
}


}
