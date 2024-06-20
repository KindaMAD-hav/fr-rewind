using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class TraceAndDash : MonoBehaviour
{
    public FrrewindInputActionAsset controls;

    public Vector2 playerPosition;
    public GameObject[] pointObject;//trail visual
    private int i = 0;

    public float deathTime;

    public float slowDelayinDropPoint;

    private Queue<GameObject> allDotQueue = new Queue<GameObject>();
    public int maxDotArraySize;

    public bool isSlowed = false;
    public float slowTime = 0.2f;

    private bool isTracing = false;
    private bool isPausing = false;


    void Awake()
    {
        controls = new FrrewindInputActionAsset();
        
    }

    private void Update()
    {
        isPausing = Keyboard.current.spaceKey.isPressed;
        if (Keyboard.current.spaceKey.isPressed)
        {
            Debug.Log("trin to slow");
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
        }
    }
    private void FixedUpdate()
    {
 
        playerPosition = transform.position;


       

        if (isPausing)
        {

            StartCoroutine(DropPointSlower());

        }
        else if (!isPausing)
        {
            DropPoint();
        }
    }

    private IEnumerator DropPointSlower()
    {

        while (isPausing) 
        {

            GameObject instantiatedObject = Instantiate(pointObject[i], playerPosition, Quaternion.identity);
            AddElementToArray(instantiatedObject);
            if (!isTracing)
            {
                Destroy(instantiatedObject, deathTime);
            }
            i++;
            if (i == pointObject.Length)
            {
                i -= 5;
            }
            yield return new WaitForSeconds(slowDelayinDropPoint);
        }
    }

    private void OnEnable()
    {
        controls.Enable();
       
    }


    private void OnDisable()
    {
        controls.Disable();
    }


    void DropPoint()
    {

        Debug.Log("dropping");
        GameObject instantiatedObject = Instantiate(pointObject[i], playerPosition, Quaternion.identity);
        AddElementToArray(instantiatedObject);
        if(!isTracing)
        {
            Destroy(instantiatedObject, deathTime);
        }
        i++;
        if (i == pointObject.Length)
        {
             i -= 5;
        }    
  
    }


    void AddElementToArray(GameObject element)
    {
        allDotQueue.Enqueue(element);
    
        if(allDotQueue.Count > maxDotArraySize) 
        {
            Destroy(allDotQueue.Dequeue());
        }
    }

    /*void SlowTime()
    {
        if(isSlowed)
        {
            Debug.Log("slowing");
        }
    }*/

}
