using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    //public float moveSpeed = 2f;
    public float alertRange = 3f;
    public LayerMask playerLayer;
    public Transform[] patrolPoints;
    private NavMeshAgent agent;

    private int currentPointIndex = 0;
    //private Rigidbody2D rb;
    private bool isAlerted = false;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        //rb = GetComponent<Rigidbody2D>();
        if(patrolPoints.Length > 0){
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    void FixedUpdate() {
        if(!isAlerted){
            Patrol();
            CheckForPlayer();
        }else{
            AlertedBehavior();
        }
    }

    void Patrol(){
        if(patrolPoints.Length == 0)
            return;

        if(agent.remainingDistance < 0.2f){
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition(){
        int newPointIndex = Random.Range(0, patrolPoints.Length);
        while(newPointIndex == currentPointIndex){
            newPointIndex = Random.Range(0, patrolPoints.Length);
        }
        currentPointIndex = newPointIndex;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    void CheckForPlayer(){
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, alertRange, playerLayer);
        if(playerCollider != null){
            isAlerted = true;
        }
    }

    void AlertedBehavior(){
        Debug.Log("Player Spotted");
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRange);

        Gizmos.color = Color.green;
        if(patrolPoints.Length > 0){
            for(int i = 0; i<patrolPoints.Length; i++){
                if(patrolPoints[i] != null){
                    Gizmos.DrawWireSphere(patrolPoints[i].position, 0.2f);
                }
            }
        }
    }
}
