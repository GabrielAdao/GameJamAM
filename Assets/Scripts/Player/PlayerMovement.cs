using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;
    public float playerSpeed = 5f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }
    
    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * playerSpeed * Time.deltaTime);
    }
}
