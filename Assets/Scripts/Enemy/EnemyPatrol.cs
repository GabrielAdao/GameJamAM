using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public float fieldOfView = 45f;
    public float alertRange = 3f;
    public LayerMask playerLayer;
    public Transform[] patrolPoints;
    private NavMeshAgent agent;
    public float rotationSpeed = 5f;
    public float alertDuration = 5f;
    public float workZoneCheckDelay = 10f;

    private int currentPointIndex = 0;
    private bool isAlerted = false;
    private PlayerEnergy playerEnergy;
    private WorkZone workZone;

    void Start() {
        playerEnergy = FindObjectOfType<PlayerEnergy>();
        workZone = FindObjectOfType<WorkZone>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        if(patrolPoints.Length > 0){
            SetNextPatrolPoint();
        }

        StartCoroutine(CheckWorkZoneStatus());
    }

    void Update() {
        if(!isAlerted){
            Patrol();
            CheckForPlayer();
        }
        RotateTowardsMovementDirection();
    }

    void Patrol(){
        if(patrolPoints.Length == 0)
            return;

        if(agent.remainingDistance < 0.2f){
            SetNextPatrolPoint();
        }
    }

    void SetNextPatrolPoint(){
        int newPointIndex = Random.Range(0, patrolPoints.Length);
        while(newPointIndex == currentPointIndex){
            newPointIndex = Random.Range(0, patrolPoints.Length);
        }
        currentPointIndex = newPointIndex;
        agent.SetDestination(patrolPoints[currentPointIndex].position);

    }

    void RotateTowardsMovementDirection(){
        Vector3 direction = agent.velocity.normalized;
        if(direction != Vector3.zero){
            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.right, direction, step, 0.0f);
            transform.right = newDirection;
        }
    }

    IEnumerator CheckWorkZoneStatus(){
        while(true){
            if(workZone != null && !workZone.IsOnCooldown()){
                yield return new WaitForSeconds(workZoneCheckDelay);
                if(!workZone.IsOnCooldown() && !workZone.isHolding){
                    Debug.Log("Workzone still active");
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void CheckForPlayer(){
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, alertRange, playerLayer);
        if(playerCollider != null){
            Vector2 directionToPlayer = (playerCollider.transform.position - transform.position).normalized;
            float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);
            if(angleToPlayer < fieldOfView /2){
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, alertRange, playerLayer);
                if(hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")){
                    isAlerted = true;
                    StartCoroutine(AlertedBehavior());
                }
            }
        }
    }

    IEnumerator AlertedBehavior(){
        if(playerEnergy.isRecharging){
            PlayerLifeSystem playerLifeSystem = FindObjectOfType<PlayerLifeSystem>();
            playerLifeSystem.PlayerLoseLife();
            alertDuration = 5f;
        }
        yield return new WaitForSeconds(alertDuration);
        isAlerted = false;
        alertDuration = 1f;
        SetNextPatrolPoint();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        if(patrolPoints.Length > 0){
            for(int i = 0; i<patrolPoints.Length; i++){
                if(patrolPoints[i] != null){
                    Gizmos.DrawWireSphere(patrolPoints[i].position, 0.2f);
                }
            }
        }

        Gizmos.color = Color.yellow;
        Vector3 rightBoundary = Quaternion.Euler(0,0, fieldOfView / 2) * transform.right * alertRange;
        Vector3 leftBoundary = Quaternion.Euler(0,0, -fieldOfView / 2) * transform.right * alertRange;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);

        DrawFieldOfView(transform.position, transform.right, alertRange, fieldOfView);
    }

    void DrawFieldOfView(Vector3 position, Vector3 direction, float radius, float angle ){
        int stepCount = Mathf.RoundToInt(angle * 2);
        float stepAngleSize = angle / stepCount;

        Vector3 previousPoint = position + Quaternion.Euler(0,0, -angle / 2) * direction * radius;

        for(int i = 1; i<= stepCount; i++){
            float stepAngle = -angle / 2 + stepAngleSize * i;
            Vector3 newPoint = position + Quaternion.Euler(0,0,stepAngle) * direction * radius;

            Gizmos.DrawLine(previousPoint, newPoint);
            previousPoint = newPoint;
        }
    }
}
