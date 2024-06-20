using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awareness : MonoBehaviour
{
    public bool Aware {  get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }
    
    [SerializeField]
    public float enemyVision;

    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<Shmovement>().transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 enemyToPlayerDist = player.position - transform.position;
            DirectionToPlayer = enemyToPlayerDist.normalized;

            if (enemyToPlayerDist.magnitude <= enemyVision)
            {
                Aware = true;
            }

            else
            {
                Aware = false;
            }
        }
    }
}
