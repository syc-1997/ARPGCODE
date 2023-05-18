using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // �ʥӥ��`������AI��ʹ���������ǰ���g

public class AIActorController : MonoBehaviour
{
    public GameObject model; // AI��ǥ�
    public PhysicMaterial frictionOne; // ͨ����Ħ��
    public PhysicMaterial frictionZero; // Ħ���ʤ�

    private Animator anim; // ���˥�`��
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;

    private bool lockPlanar = false;
    private bool trackDirction = false;
    private CapsuleCollider col; // AI�Υ��ץ��륳�饤��
    private Vector3 deltaPos;

    public enum State { Patrol, Chase, Attack, Hit, Die } // AI��״�B
    public State currentState; // �F�ڤ�AI��״�B

    public Transform[] waypoints; // �ѥȥ�`�뤹��ݥ����
    public Transform player; // �ץ쥤��`

    public float patrolSpeed = 2.0f; // �ѥȥ�`���ٶ�
    public float chaseSpeed = 4.0f; // ׷�E�ٶ�
    public float stoppingDistance = 1.0f; // Ŀ�ĵؤ����ֹͣ���x
    public float attackRange = 2.0f; // ���Ĺ���
    public float attackRate = 1.0f; // �����l��

    private int currentWaypoint = 0; // �F�ڤΥѥȥ�`��ݥ����
    private NavMeshAgent navMeshAgent; // �ʥӥ��`����󥨩`�������
    private float nextAttackTime = 2f;

    public float detectionRadius = 10f; // �ץ쥤��`��ʳ�����뾶
    public float detectionAngle = 90f; // �����I��νǶ�
    public LayerMask playerLayer; // �ץ쥤��`�Υ쥤��`

    private Transform playerTransform; // �ץ쥤��`��λ��
    private bool isdead = false;
    private bool hiting = false;

    bool attacking = false;

    private void Awake() 
    {
        anim = model.GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = State.Patrol;
        SetDestination(waypoints[currentWaypoint]);
    }

    private void Update() 
    {
        anim.SetFloat("forward", navMeshAgent.speed / 2f);
        switch (currentState)
        {
            case State.Patrol:
                navMeshAgent.speed = 2f;
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
                navMeshAgent.speed = 3.5f;
                if (CanAttackPlayer())
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                StartCoroutine(AttackPlayer());
                break;

            case State.Hit:
                StartCoroutine(HitState());
                break;

            case State.Die:
                navMeshAgent.speed = 0f;
                navMeshAgent.isStopped = true;
                if (!isdead)
                {
                    isdead = true;
                    anim.SetTrigger("die");
                }
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;

                Destroy(gameObject, 2f);
                break;
        }
    }
    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }

    void SetDestination(Transform target) // Ŀ�ĵؤ��O��
    {
        navMeshAgent.SetDestination(target.position);
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    bool CanSeePlayer() // �ץ쥤��`��ʳ��Ǥ��뤫�ɤ������ж�
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < detectionAngle * 0.5f)
                {
                    playerTransform = collider.transform;
                    return true;
                }
            }
        }
        playerTransform = null;
        return false;
    }

    bool CanAttackPlayer() // �ץ쥤��`�򹥓ĤǤ��뤫�ɤ������ж�
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    void OnDrawGizmosSelected() // �ʳ�������ʾ
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Vector3 detectionRight = Quaternion.Euler(0, detectionAngle * 0.5f, 0) * transform.forward;
        Vector3 detectionLeft = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + detectionRight * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + detectionLeft * detectionRadius);
        Gizmos.DrawLine(transform.position + detectionRight * detectionRadius, transform.position + detectionLeft * detectionRadius);
    }

    public void dead() // ����״�B�ؤ��w��
    {
        currentState = State.Die;
    }

    public void IsGround() // �ӵ�״�B���O��
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround() // �ǽӵ�״�B���O��
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter() // �ӵ�״�B����ä��Ȥ��΄I��
    {
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirction = false;
    }

    public void OnGroundExit() // �ӵ�״�B��������Ȥ��΄I��
    {
        col.material = frictionZero;
    }

    public void OnAttack1AEnter() // �����_ʼ
    {
        lockPlanar = false;
    }

    public void OnAttack1AUpdate() // ������
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1AVelocity");
    }

    public void OnAttackExit() // ���ĽK��
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnHitEnter() // �ҥåȕr
    {
        planarVec = Vector3.zero;
    }

    public void OnUpdateRM(object _deltaPos) // �ݥ������θ���
    {
        deltaPos += (Vector3)_deltaPos;
    }

    public void SetState(State newState) // ״�B���O��
    {
        currentState = newState;
    }

    private IEnumerator HitState() // �ҥå�״�B�΄I��
    {
        navMeshAgent.isStopped = true;
        if (!hiting)
        {
            anim.SetTrigger("hit");
            hiting = true;
        }
        yield return new WaitForSeconds(0.5f);
        hiting = false;
        navMeshAgent.isStopped = false;
        currentState = State.Chase;
    }

    private IEnumerator AttackPlayer() // �ץ쥤��`�ؤι��ĄI��
    {
        navMeshAgent.SetDestination(transform.position);
        if (CanAttackPlayer() && !attacking)
        {
            anim.SetTrigger("attack");
            attacking = true;
        }
        yield return new WaitForSeconds(1f);
        attacking = false;
        currentState = State.Chase;
    }
}
