using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTrail : MonoBehaviour
{
    public GameObject[] dotPrefab;
    public GameObject playerClonePrefab;
    public int maxDots = 50;
    public float dotLifetime = 2.0f;
    int a = 0;

    private List<GameObject> dots = new List<GameObject>();
    private float timeInterval = 0.02f;
    private float timeSinceLastDot = 0f;

    public float slowTime;
    private bool isSlowed = false;
    bool spacePressed = false;
    private GameObject playerClone;
    public float dashSpeed;

    private bool isDashing = false;

    [SerializeField] private ParticleSystem playerDeathParticles = default;
    public float CameraShakeTime;
    public float CameraShakeIntensity;

    private CameraShake cameraShake;
    private LevelGenerator levelGenerator;

    private HealthBar healthBar;
    public float healthDecreaseRate = 1f;
    public float enemyKillHealthValue = 1f;


    void Start()
    {       

        cameraShake = Camera.main.GetComponent<CameraShake>();

        levelGenerator = FindObjectOfType<LevelGenerator>();

        healthBar = FindObjectOfType<HealthBar>();

        StartCoroutine(DecreaseHealthOverTime());
    }

    void Update()
    {
        spacePressed = Keyboard.current.spaceKey.isPressed;
        HandleTimeSlow();

        timeSinceLastDot += Time.unscaledDeltaTime;

        float adjustedTimeInterval = timeInterval / Time.timeScale;

        if (timeSinceLastDot >= adjustedTimeInterval)
        {
            LeaveDot();
            timeSinceLastDot = 0f;
        }
    }

    void LeaveDot()
    {
        GameObject newDot = Instantiate(dotPrefab[a], transform.position, transform.rotation);
        dots.Add(newDot);
        a++;
        if (a == dotPrefab.Length)
        {
            a -= 5;
        }

        Destroy(newDot, dotLifetime);

        if (dots.Count > maxDots)
        {
            Destroy(dots[0]);
            dots.RemoveAt(0);
        }
    }

    void HandleTimeSlow()
    {
        if (spacePressed)
        {
            isSlowed = true;
            isDashing = false;
            Time.timeScale = slowTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            SpawnClone();
        }
        else if (!spacePressed)
        {
            isSlowed = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            StartCoroutine(DashToClone());
        }
    }

    void SpawnClone()
    {
        if (playerClone != null) return;
        playerClone = Instantiate(playerClonePrefab, transform.position, transform.rotation);
        StartCoroutine(MoveCloneAlongTrail());
    }

    IEnumerator MoveCloneAlongTrail()
    {
        int index = dots.Count - 1;
        while (isSlowed && index >= 0)
        {
            if (playerClone == null) yield break;
            playerClone.transform.position = dots[index].transform.position;
            index--;
            yield return new WaitForSeconds(0.02f * slowTime);
        }
    }

    IEnumerator DashToClone()
    {
        if (playerClone == null) yield break;

        isDashing = true;

        while ((transform.position - playerClone.transform.position).sqrMagnitude > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerClone.transform.position, dashSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = playerClone.transform.position;
        Destroy(playerClone);
        ClearDots();
        isDashing = false;
    }

    void ClearDots()
    {
        foreach (var dot in dots)
        {
            if (dot != null)
            {
                Destroy(dot);
            }
        }
        dots.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isDashing)
            {
                Destroy(collision.gameObject);

                healthBar.UpdateHealth(enemyKillHealthValue);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (isDashing)
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("EndPoint")) 
        { 
            levelGenerator.NextLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isDashing)
            {

                healthBar.UpdateHealth(enemyKillHealthValue);

                Destroy(collision.gameObject);

            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (isDashing)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnDestroy()
    {
        if (CameraShakeManager.Instance != null)
        {
            CameraShakeManager.Instance.ShakeCamera(CameraShakeTime, CameraShakeIntensity);
        }

        Instantiate(playerDeathParticles, transform.position, Quaternion.identity);

        if (playerClone != null)
        {
            Destroy(playerClone);
        }

        healthBar.UpdateHealth(-100);
    }

    IEnumerator DecreaseHealthOverTime()
    {
        while(true)
        {
         healthBar.UpdateHealth(-healthDecreaseRate);
         yield return new WaitForSeconds(1f);
        }
    }
}
