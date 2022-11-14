using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f; 

    private Vector2 moveInput;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void FixedUpdate() //FixedUpdate
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);//Time.fixedDeltaTime
    }





    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

}
