using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DotTrail : MonoBehaviour
{
    public Vector2 playerPosition;
    public GameObject[] pointObject;//trail visual
    private int i=0;

    public float deathTime;
    public float traillDeathTime;

    private List <GameObject> pointObjects = new List<GameObject>();//trail logic

    private bool isTracing = false;
    private int currentTrailIndex;

    public FrrewindInputActionAsset controls;
    
    private InputAction traceAction;



    private void Awake()
    {
        controls.Player.Trace.performed += context => Trace();
    }

    void Trace()
    {
        Debug.Log("isTracing");
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    void FixedUpdate()
    {
        playerPosition = transform.position;
        DropPoint();

        if (traceAction.WasPerformedThisFrame())
        {
            if(!isTracing)
            {
                isTracing = true;
               FreezeAllObjects();
                Debug.Log("space");
            }
           // TraceTrail();
        }
        else if(isTracing)
        {
            isTracing=false;
            UnFreezeAllObjects();
            Debug.Log("space2");
        }
    }

    void DropPoint()
    {
        GameObject instantiatedobject = Instantiate(pointObject[i], playerPosition, Quaternion.identity);
        pointObjects.Add(instantiatedobject);
        i++;
        if (i == pointObject.Length)
        {
            i -= 5;
        }
        if (isTracing)
        {
            Destroy(instantiatedobject, traillDeathTime);
        }
        else
        {
            Destroy(instantiatedobject, deathTime);
        }
    }
   void TraceTrail()
    {
        if (currentTrailIndex < pointObjects.Count)
        {
            transform.position = pointObjects[currentTrailIndex].transform.position;
            currentTrailIndex++;
        }
    }
    void FreezeAllObjects()
    {
        Time.timeScale = 0f;
    }
    void UnFreezeAllObjects()
    {
        Time.timeScale = 1f;
    } 
}
