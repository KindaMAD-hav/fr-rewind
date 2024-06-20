using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CoreMechanism : MonoBehaviour
{
    public float dashSpeed;
    public KeyCode dashKey = KeyCode.Q;

    public bool isDashing;
    public Vector2 tracerPosition;

    void Update()
    {
        if(Input.GetKeyDown(dashKey))
        {
            if (!isDashing)
            {
                tracerPosition = (Vector2)transform.position;
                isDashing = true;
            }
        }
        else
        {
            if (isDashing)
            {
                DashToPosition(tracerPosition);
            }
        }
    }
    void DashToPosition(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float distance = direction.magnitude;

        Vector2 dashPosition = transform.position;
        float dashTime = 0f;

        while(dashTime < distance/dashSpeed)
        {
            dashTime += Time.deltaTime;
            dashPosition = Vector2.Lerp(transform.position, targetPosition, dashTime * dashSpeed / distance);
            transform.position = dashPosition;
        }

        transform.position = targetPosition;
    }
}


