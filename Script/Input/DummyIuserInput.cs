using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyIuserInput : IUserInput
{
    public enum State { Patrol, Chase, Attack ,Die}
    public State currentState;

    public Transform[] waypoints;
    public Transform player;

    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.0f;
    public float stoppingDistance = 1.0f;
    public float attackRange = 2.0f;
    public float attackRate = 1.0f;

    private int currentWaypoint = 0;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime = 2f;

    public float detectionRadius = 10f; // プレイヤーを検出する半径
    public float detectionAngle = 90f; // 扇形領域の角度
    public LayerMask playerLayer; // プレイヤーのレイヤー

    private Transform playerTransform; // プレイヤーの位置

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = State.Patrol;
        SetDestination(waypoints[currentWaypoint]);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                if (navMeshAgent.remainingDistance <= stoppingDistance)
                {
                    currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                    SetDestination(waypoints[currentWaypoint]);
                }
                if (CanSeePlayer())
                {
                    currentState = State.Chase;
                }
                break;
            case State.Chase:
                SetDestination(player);
                if (CanAttackPlayer())
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                AttackPlayer();
                if (!CanAttackPlayer() || !CanSeePlayer())
                {
                    currentState = State.Chase;
                }
                break;

            case State.Die:
                navMeshAgent.speed = 0f;
                navMeshAgent.isStopped = true;
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                break;
        }

       
    }

    void SetDestination(Transform target)
    {
        navMeshAgent.SetDestination(target.position);
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    bool CanSeePlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        foreach (Collider collider in colliders)
        {
            // 検出したオブジェクトがプレイヤーである場合
            if (collider.CompareTag("Player"))
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                // 検出したプレイヤーが扇形領域内にいる場合
                if (angle < detectionAngle * 0.5f)
                {
                    // プレイヤーが視界に入っている
                    //Debug.Log("プレイヤーが発見されました");
                    playerTransform = collider.transform;
                    return true;
                }                
            }
        }
        // プレイヤーが視界外にいる
        playerTransform = null;
        return false;
       
    }

    bool CanAttackPlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    void AttackPlayer()
    {
        
        if (CanAttackPlayer())
        {
            // TODO: 攻击玩家的代码
            navMeshAgent.SetDestination(transform.position);
            rt = true;
        }
       // else
       // {
       //     navMeshAgent.SetDestination(player.position);
       //     navMeshAgent.speed = chaseSpeed;
       //     navMeshAgent.stoppingDistance = attackRange;
       //     transform.LookAt(player);
       // }
    }


    void OnDrawGizmosSelected()
    {
        // 検出範囲を表示する
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // 扇形領域を表示する
        Vector3 detectionRight = Quaternion.Euler(0, detectionAngle * 0.5f, 0) * transform.forward;
        Vector3 detectionLeft = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + detectionRight * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + detectionLeft * detectionRadius);
        Gizmos.DrawLine(transform.position + detectionRight * detectionRadius, transform.position + detectionLeft * detectionRadius);
    }

    public void dead()
    {
        currentState = State.Die;
    }
}

//        rt = false;
