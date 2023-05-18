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

    public float detectionRadius = 10f; // �ץ쥤��`��ʳ�����뾶
    public float detectionAngle = 90f; // �����I��νǶ�
    public LayerMask playerLayer; // �ץ쥤��`�Υ쥤��`

    private Transform playerTransform; // �ץ쥤��`��λ��

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
            // �ʳ��������֥������Ȥ��ץ쥤��`�Ǥ������
            if (collider.CompareTag("Player"))
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                // �ʳ������ץ쥤��`�������I���ڤˤ������
                if (angle < detectionAngle * 0.5f)
                {
                    // �ץ쥤��`��ҕ�����äƤ���
                    //Debug.Log("�ץ쥤��`���kҊ����ޤ���");
                    playerTransform = collider.transform;
                    return true;
                }                
            }
        }
        // �ץ쥤��`��ҕ����ˤ���
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
            // TODO: ������ҵĴ���
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
        // �ʳ�������ʾ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // �����I����ʾ����
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
