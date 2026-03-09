using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraBehavior : MonoBehaviour
{

    [Header("Zoom")]
    public Camera targetCamera; // optional, if not set will use Camera.main
    public float zoomFOV = 40f; // for perspective
    public float zoomOrthoSize = 3f; // for orthographic
    public float zoomInDuration = 0.25f;
    public float holdDuration = 0.5f;
    public float zoomOutDuration = 0.25f;


    [Header("Shake")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;

    [Header("Shake Follow Target")]
    public Transform shakeTarget; // set to player; if null will use this.transform

    private bool isEffectPlaying = false;
    private float originalFOV;
    private float originalOrthoSize;

    private Coroutine shakeCoroutine;

    void Reset()
    {
        targetCamera = Camera.main;
        shakeTarget = transform;
    }

    void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (shakeTarget == null) shakeTarget = transform;

        if (targetCamera != null)
        {
            originalFOV = targetCamera.fieldOfView;
            originalOrthoSize = targetCamera.orthographicSize;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("CameraBehavior detected collision with: " + collision.gameObject.name);
        if (!isEffectPlaying && collision.gameObject.CompareTag("Finish")||!isEffectPlaying && collision.gameObject.CompareTag("Enemy") )    {
            Debug.Log("Camera zoom and shake effect triggered.");
            StartCoroutine(ZoomAndShake());

        }


        //ADD SCENE WHEN PUSH
    }

    IEnumerator ZoomAndShake()
    {
        if (targetCamera == null) yield break;
        isEffectPlaying = true;

        // Zoom in
        float t = 0f;
        if (targetCamera.orthographic)
        {
            float start = targetCamera.orthographicSize;
            while (t < zoomInDuration)
            {
                t += Time.deltaTime;
                targetCamera.orthographicSize = Mathf.Lerp(start, zoomOrthoSize, t / zoomInDuration);
                yield return null;
            }
            targetCamera.orthographicSize = zoomOrthoSize;
        }
        else
        {
            float start = targetCamera.fieldOfView;
            while (t < zoomInDuration)
            {
                t += Time.deltaTime;
                targetCamera.fieldOfView = Mathf.Lerp(start, zoomFOV, t / zoomInDuration);
                yield return null;
            }
            targetCamera.fieldOfView = zoomFOV;
        }

        // Start shake while holding zoom
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeFollowTarget());

        // Hold zoom
        yield return new WaitForSeconds(holdDuration);

        // Zoom out
        t = 0f;
        if (targetCamera.orthographic)
        {
            float start = targetCamera.orthographicSize;
            while (t < zoomOutDuration)
            {
                t += Time.deltaTime;
                targetCamera.orthographicSize = Mathf.Lerp(start, originalOrthoSize, t / zoomOutDuration);
                yield return null;
            }
            targetCamera.orthographicSize = originalOrthoSize;
        }
        else
        {
            float start = targetCamera.fieldOfView;
            while (t < zoomOutDuration)
            {
                t += Time.deltaTime;
                targetCamera.fieldOfView = Mathf.Lerp(start, originalFOV, t / zoomOutDuration);
                yield return null;
            }
            targetCamera.fieldOfView = originalFOV;
        }

        // Stop shake + snap camera back to follow target
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        SnapCameraToTarget();

        isEffectPlaying = false;
    }

    IEnumerator ShakeFollowTarget()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            Vector3 basePos = GetTargetPos();
            Vector3 offset = Random.insideUnitSphere * shakeMagnitude;
            offset.z = 0f; // 2D shake

            targetCamera.transform.position = new Vector3(
                basePos.x + offset.x,
                basePos.y + offset.y,
                targetCamera.transform.position.z
            );

            yield return null;
        }

        SnapCameraToTarget();
    }

    Vector3 GetTargetPos()
    {
        Vector3 p = shakeTarget.position;
        // Keep camera z unchanged so it doesn't move through the scene
        return new Vector3(p.x, p.y, targetCamera.transform.position.z);
    }

    void SnapCameraToTarget()
    {
        Vector3 basePos = GetTargetPos();
        targetCamera.transform.position = basePos;
    }
}