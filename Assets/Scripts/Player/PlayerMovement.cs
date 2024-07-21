using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;
    public float playerSpeed = 5f;
    private bool isStunned = false;
    public float stunDuration = 2f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }
    
    private void FixedUpdate() {
        if(!isStunned){
            rb.MovePosition(rb.position + movement * playerSpeed * Time.deltaTime);
        }
    }

    public void Stunned(float duration){
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration){
        isStunned = true;

        // animacao de stun

        yield return new WaitForSeconds(duration);

        isStunned = false;
        //sai animacao de stun
    }
}
