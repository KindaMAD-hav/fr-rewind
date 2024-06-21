using System.Collections;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(duration, magnitude));
    }
}
