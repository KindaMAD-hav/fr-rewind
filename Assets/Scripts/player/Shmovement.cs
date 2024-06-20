using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shmovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;
    
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 shmovementInput;
    private Vector2 shmovementVelocity;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    public void InputPlayer(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        SetPlayerVelocity();
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        shmovementInput = new Vector2(movementInput.x, movementInput.y);
        //shmovementInput.Normalize();
        rb.velocity = shmovementInput * speed;
    }


    private void SetPlayerVelocity()
    {
        shmovementInput = Vector2.SmoothDamp(shmovementInput, movementInput, ref shmovementVelocity, 0.1f);
        shmovementInput.Normalize();
        rb.velocity = shmovementInput * speed;
    }


    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }
}
