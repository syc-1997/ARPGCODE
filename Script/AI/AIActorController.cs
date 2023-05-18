using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ナビゲーションとAIを使うための名前空間

public class AIActorController : MonoBehaviour
{
    public GameObject model; // AIモデル
    public PhysicMaterial frictionOne; // 通常の摩擦
    public PhysicMaterial frictionZero; // 摩擦なし

    private Animator anim; // アニメータ
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;

    private bool lockPlanar = false;
    private bool trackDirction = false;
    private CapsuleCollider col; // AIのカプセルコライダ
    private Vector3 deltaPos;

    public enum State { Patrol, Chase, Attack, Hit, Die } // AIの状態
    public State currentState; // 現在のAIの状態

    public Transform[] waypoints; // パトロールするポイント
    public Transform player; // プレイヤー

    public float patrolSpeed = 2.0f; // パトロール速度
    public float chaseSpeed = 4.0f; // 追跡速度
    public float stoppingDistance = 1.0f; // 目的地からの停止距離
    public float attackRange = 2.0f; // 攻撃範囲
    public float attackRate = 1.0f; // 攻撃頻度

    private int currentWaypoint = 0; // 現在のパトロールポイント
    private NavMeshAgent navMeshAgent; // ナビゲーションエージェント
    private float nextAttackTime = 2f;

    public float detectionRadius = 10f; // プレイヤーを検出する半径
    public float detectionAngle = 90f; // 扇形領域の角度
    public LayerMask playerLayer; // プレイヤーのレイヤー

    private Transform playerTransform; // プレイヤーの位置
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

    void SetDestination(Transform target) // 目的地を設定
    {
        navMeshAgent.SetDestination(target.position);
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    bool CanSeePlayer() // プレイヤーを検出できるかどうかを判定
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

    bool CanAttackPlayer() // プレイヤーを攻撃できるかどうかを判定
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    void OnDrawGizmosSelected() // 検出範囲を表示
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

    public void dead() // 死亡状態への遷移
    {
        currentState = State.Die;
    }

    public void IsGround() // 接地状態を設定
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround() // 非接地状態を設定
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter() // 接地状態に入ったときの処理
    {
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirction = false;
    }

    public void OnGroundExit() // 接地状態から出たときの処理
    {
        col.material = frictionZero;
    }

    public void OnAttack1AEnter() // 攻撃開始
    {
        lockPlanar = false;
    }

    public void OnAttack1AUpdate() // 攻撃中
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1AVelocity");
    }

    public void OnAttackExit() // 攻撃終了
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnHitEnter() // ヒット時
    {
        planarVec = Vector3.zero;
    }

    public void OnUpdateRM(object _deltaPos) // ポジションの更新
    {
        deltaPos += (Vector3)_deltaPos;
    }

    public void SetState(State newState) // 状態の設定
    {
        currentState = newState;
    }

    private IEnumerator HitState() // ヒット状態の処理
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

    private IEnumerator AttackPlayer() // プレイヤーへの攻撃処理
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
