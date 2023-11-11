using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownWASD : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;

    [Header("Internal Movement")]
    public float moveSpeed = 5f;
    private Vector2 movement;

    [Header("External Movement")]
    public Vector2 outsideForce;
    public float forceDamping;
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 moveForce = movement * moveSpeed;
        moveForce += outsideForce;
        //Slows down the amount of force of outsideForce by force Dampining
        outsideForce /= forceDamping;

        if (Mathf.Abs(outsideForce.x) <= 0.01f && Mathf.Abs(outsideForce.x) <= 0.01f)
            outsideForce = Vector2.zero;

        rb.MovePosition(rb.position + moveForce * Time.fixedDeltaTime);
    }
}
