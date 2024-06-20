using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlowTime : MonoBehaviour
{
    public FrrewindInputActionAsset controls;

    public bool isSlowed = false;
    public float slowTime  = 0.2f;

    private void Awake()
    {
        controls = new FrrewindInputActionAsset();
    }
    void FixedUpdate()
    {
        
    }
    void OnSlowPause()
    {   
        /*if (Keyboard.current.spaceKey.isPressed)
        {
            if (isSlowed)
            {
                isSlowed = false;
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.deltaTime;
            }
            else
            {
                isSlowed = true;
                Time.timeScale = slowTime;
                Time.fixedDeltaTime = slowTime * Time.deltaTime;

            }
        }*/

    }
}
