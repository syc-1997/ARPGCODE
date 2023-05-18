using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model; // �ץ쥤��`����饯���`�Υ�ǥ륪�֥�������
    public ThirdPersonCamera camcon; // ���������������ץ�
    public IUserInput pi; // ��`���`����������ץ�
    public float WalkSpeed = 1.5f; // �i���ٶ�
    public float RunMultplier = 2.0f; // �ߤ��ٶȱ���
    public float jumpVelocity = 5.0f; // �����פ��ٶ�
    public float rollVelocity = 3.0f; // ��ܞ�����ٶ�

    [Space(10)]
    [Header("========================")]
    public PhysicMaterial frictionOne;      // ����ޥƥꥢ��
    public PhysicMaterial frictionZero;     // ����ޥƥꥢ��

    private Animator anim;                  // ���˥�`���`����ݩ`�ͥ��
    private Rigidbody rigid;                // Rigidbody ����ݩ`�ͥ��
    private Vector3 planarVec;              // ƽ���Ϥ��ٶȥ٥��ȥ�
    private Vector3 thrustVec;              // ���M���٥��ȥ�
    private bool canAttack;                 // ���Ŀ��ܤ��ɤ���

    private bool lockPlanar = false;        // ƽ���Ϥ΄Ӥ������ޤ��뤫�ɤ���
    private bool trackDirction = false;     // �Ƅӷ����׷�E���뤫�ɤ���
    private CapsuleCollider col;            // ���ץ��륳�饤���`
                                            //private float lerpTarget;
    private Vector3 deltaPos;               // λ�äβ��

    public bool leftIsShield = true;        // ���֤�ܤˤ��뤫�ɤ���


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
            camcon.LockUnlock();    // �������å��ޤ��ϥ����å�
        }
        // ����餬�����å�����Ƥ������
        if (camcon.lockState == false)
        {
            // ǰ�M����Υ��˥�`����������
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), (pi.run) ? 2.0f : 1.0f, 0.5f));
            anim.SetFloat("right", 0);  // �ҷ���Υ��˥�`����������
        }
        else
        {
            // ����餬��å�����Ƥ������
            Vector3 localDvec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", localDvec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDvec.x * ((pi.run) ? 2.0f : 1.0f));
        }

        // �⤷��`�뤹�뤫��������ٶȤ�6f�򳬤������
        if (pi.roll == true || rigid.velocity.magnitude > 6f)
        {
            // ���˥�`�����Υ�`��ȥꥬ�`�򥻥åȤ������Ĳ��ɤˤ���
            anim.SetTrigger("roll");
            canAttack = false;
            
        }

        // �����פ������
        if (pi.jump)
        {
            // ���˥�`�����Υ����ץȥꥬ�`�򥻥åȤ���
            anim.SetTrigger("Jump");
        }

        // �⤷�ҥȥꥬ�`�ޤ�����ȥꥬ�`��Ѻ���줿���ϡ�����ˤ��뤫�������ФǤ�����ϡ����Ŀ��ܤʈ���
        if ((pi.rt || pi.lt) && (CheckState("ground") || CheckStateTag("AttackR") || CheckStateTag("AttackL")) && canAttack)
        {
            // �⤷�ҥȥꥬ�`��Ѻ���줿����
            if (pi.rt)
            {
                // ���˥�`�����ι��ĥȥꥬ�`�򥻥åȤ���
                anim.SetBool("R0L1", false);
                anim.SetTrigger("attack");
            }
            // �⤷��ȥꥬ�`��Ѻ���졢��Ȥ��ܤǤʤ�����
            else if (pi.lt && !leftIsShield)
            {
                // ���˥�`�����ι��ĥȥꥬ�`�򥻥åȤ���
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");
            }
        }

        // �⤷�ҥܥ���ޤ�����ܥ���Ѻ���줿���ϡ�����ˤ��뤫�������ФǤ�����ϡ����Ŀ��ܤʈ���
        if ((pi.rb || pi.lb) && (CheckState("ground") || CheckStateTag("AttackR") || CheckStateTag("AttackL")) && canAttack)
        {
            // �ҥܥ���Ѻ���줿����
            if (pi.rb)
            {
                anim.SetBool("R0L1", false);
                anim.SetBool("charge", pi.rb);
            }
            // ��ܥ���Ѻ���졢��Ȥ��ܤǤʤ�����
            else if (pi.lb && !leftIsShield)
            {
                anim.SetBool("R0L1", true);
                anim.SetBool("charge", pi.lb);
            }
        }
        if (!pi.rb && !pi.lb)
        {
            // ���˥�`�����Υ���`���ȥꥬ�`��������
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
