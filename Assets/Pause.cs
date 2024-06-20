using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public KeyCode pauseMech = KeyCode.Space;
    public float holdDuration = 0.1f;
    public float currentHoldDuration = 0f;
    public bool isPaused = false;


    void Update()
    {

        
        Debug.Log("updating");
        if (Input.GetKeyDown(pauseMech))
        {
            Debug.Log("Button is being held");
            currentHoldDuration += Time.deltaTime;
            
            if (currentHoldDuration >= holdDuration)
            {
                ExecutePause();
                currentHoldDuration = 0f;
            }
        }
        else
        {
            currentHoldDuration = 0f;
        }
    }

    void ExecutePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Pause the game by setting the time scale to 0
            Time.timeScale = 0f;
        }
        else
        {
            // Resume the game by setting the time scale to 1
            Time.timeScale = 1f;
        }
    }
}
