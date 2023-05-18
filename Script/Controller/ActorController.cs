using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model; // プレイヤーキャラクターのモデルオブジェクト
    public ThirdPersonCamera camcon; // カメラ制御スクリプト
    public IUserInput pi; // ユーザー入力スクリプト
    public float WalkSpeed = 1.5f; // 歩く速度
    public float RunMultplier = 2.0f; // 走る速度倍率
    public float jumpVelocity = 5.0f; // ジャンプの速度
    public float rollVelocity = 3.0f; // 回転する速度

    [Space(10)]
    [Header("========================")]
    public PhysicMaterial frictionOne;      // 物理マテリアル
    public PhysicMaterial frictionZero;     // 物理マテリアル

    private Animator anim;                  // アニメーターコンポーネント
    private Rigidbody rigid;                // Rigidbody コンポーネント
    private Vector3 planarVec;              // 平面上の速度ベクトル
    private Vector3 thrustVec;              // 推進力ベクトル
    private bool canAttack;                 // 攻撃可能かどうか

    private bool lockPlanar = false;        // 平面上の動きを制限するかどうか
    private bool trackDirction = false;     // 移動方向を追跡するかどうか
    private CapsuleCollider col;            // カプセルコライダー
                                            //private float lerpTarget;
    private Vector3 deltaPos;               // 位置の差分

    public bool leftIsShield = true;        // 左手を盾にするかどうか


    private void Awake()
    {
        IUserInput[] inputs= GetComponents<IUserInput>();
        foreach(var input in inputs)
        {
            if(input.enabled == true)
            {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        if (pi.lockon)
        {
            camcon.LockUnlock();    // カメラをロックまたはアンロック
        }
        // カメラがアンロックされている場合
        if (camcon.lockState == false)
        {
            // 前進方向のアニメーションを制御
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), (pi.run) ? 2.0f : 1.0f, 0.5f));
            anim.SetFloat("right", 0);  // 右方向のアニメーションを制御
        }
        else
        {
            // カメラがロックされている場合
            Vector3 localDvec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", localDvec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDvec.x * ((pi.run) ? 2.0f : 1.0f));
        }

        // もしロールするか、剛体の速度が6fを超える場合
        if (pi.roll == true || rigid.velocity.magnitude > 6f)
        {
            // アニメーションのロールトリガーをセットし、攻撃不可にする
            anim.SetTrigger("roll");
            canAttack = false;
            
        }

        // ジャンプする場合
        if (pi.jump)
        {
            // アニメーションのジャンプトリガーをセットする
            anim.SetTrigger("Jump");
        }

        // もし右トリガーまたは左トリガーが押された場合、地面にいるか、攻撃中である場合、攻撃可能な場合
        if ((pi.rt || pi.lt) && (CheckState("ground") || CheckStateTag("AttackR") || CheckStateTag("AttackL")) && canAttack)
        {
            // もし右トリガーが押された場合
            if (pi.rt)
            {
                // アニメーションの攻撃トリガーをセットする
                anim.SetBool("R0L1", false);
                anim.SetTrigger("attack");
            }
            // もし左トリガーが押され、左側が盾でない場合
            else if (pi.lt && !leftIsShield)
            {
                // アニメーションの攻撃トリガーをセットする
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");
            }
        }

        // もし右ボタンまたは左ボタンが押された場合、地面にいるか、攻撃中である場合、攻撃可能な場合
        if ((pi.rb || pi.lb) && (CheckState("ground") || CheckStateTag("AttackR") || CheckStateTag("AttackL")) && canAttack)
        {
            // 右ボタンが押された場合
            if (pi.rb)
            {
                anim.SetBool("R0L1", false);
                anim.SetBool("charge", pi.rb);
            }
            // 左ボタンが押され、左側が盾でない場合
            else if (pi.lb && !leftIsShield)
            {
                anim.SetBool("R0L1", true);
                anim.SetBool("charge", pi.lb);
            }
        }
        if (!pi.rb && !pi.lb)
        {
            // アニメーションのチャージトリガーを解除する
            anim.SetBool("charge", false);
        }



        if (leftIsShield)
        {
            
            if (CheckState("ground")&& canAttack)
            {
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1f);
            }
            else
            {
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0f);
            }
        }
        else
        {
            anim.SetBool("defense", false);
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0f);
        }
        


        if (camcon.lockState == false)
        {
            if (pi.Dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.1f);
            }

            if (lockPlanar == false)
            {
                planarVec =  pi.Dmag * model.transform.forward * WalkSpeed * ((pi.run) ? RunMultplier : 1.0f);
            }
        }
        else
        {
            if(trackDirction == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }
            if(lockPlanar == false)
            {
                planarVec = pi.Dvec * WalkSpeed * ((pi.run) ? RunMultplier : 1.0f);
            }
        }
        
    }
    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    public bool CheckState(string stateName , string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }





    public void OnJumpEnter()
    {
        thrustVec = new Vector3(0, jumpVelocity, 0);
        pi.inputEnabled = false;
        lockPlanar = true;
        trackDirction = true;
    }

    public void OnJumpExit()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        trackDirction = false;
    }

    public void IsGround()
    {
        anim.SetBool("isGround" , true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }
    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirction = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {

        pi.roll = false;
        pi.inputEnabled = false;
        lockPlanar = true;
        trackDirction = true;
    }

    public void OnRollExit()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;

    }


    public void OnAttack1AEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = false;

    }

    public void OnAttack1AUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1AVelocity");
    }

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }
    public void OnHitEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;

    }

    public void OnUpdateRM(object _deltaPos)
    {
        deltaPos += (Vector3)_deltaPos;
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
}
