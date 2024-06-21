using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemybehaviour : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField] private ParticleSystem enemyDeathParticles = default;

    [SerializeField]
    private float rotationSpeed;

    private Rigidbody2D rb;

    private Awareness awareness;

    private Vector2 targetDirection;

    public float CameraShakeTime;
    public float CameraShakeIntensity;

    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        awareness = GetComponent<Awareness>();
    }

    private void FixedUpdate()
    {
        UpdateTargetToDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetToDirection()
       {
        if (awareness.Aware)
        {
            targetDirection = awareness.DirectionToPlayer;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        if(targetDirection == Vector2.zero)
        {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.SetRotation(rotation);
    }
    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = transform.up * speed;
        }
    }
    private void OnDestroy()
    {
        Instantiate(enemyDeathParticles, transform.position, Quaternion.identity);

        if (CameraShakeManager.Instance != null)
        {
            CameraShakeManager.Instance.ShakeCamera(CameraShakeTime, CameraShakeIntensity);
        }
    }

}
