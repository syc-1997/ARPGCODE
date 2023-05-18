using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StateManager���饹���x
public class StateManager : IActorMangerInterface
{
    //public ActorManger am; // ActorManager���F�ڤϥ����ȥ����ȣ�
    public float HP = 15.0f; // HP���ڂ�
    public float HPMax = 15.0f; // HP���

    // һ��״�B�ե饰
    [Header("1st Order state flags")]
    public bool isGround; // ����ˤ��뤫
    public bool isJump; // �����פ��Ƥ��뤫
    public bool isFall; // ���¤��Ƥ��뤫
    public bool isRoll; // ��`�뤷�Ƥ��뤫
    public bool isAttack; // ���Ĥ��Ƥ��뤫
    public bool isHit; // �ҥåȤ��Ƥ��뤫
    public bool isDie; // �������Ƥ��뤫
    public bool isBlocked; // �֥�å����Ƥ��뤫
    public bool isDefense; // �������Ƥ��뤫

    private void Start() // �_ʼ�r�˺��ӳ������᥽�å�
    {
        HP = HPMax; // HP����󂎤��O��
    }
    private void Update() // ���ե�`����Ф��᥽�å�
    {
        // ��״�B������å�
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("Jump");
        isFall = am.ac.CheckState("Fall");
        isRoll = am.ac.CheckState("Roll");
        isAttack = am.ac.CheckStateTag("AttackR") || am.ac.CheckStateTag("AttackL");
        isHit = am.ac.CheckState("Hit");
        isDie = am.ac.CheckState("Dead");
        isBlocked = am.ac.CheckState("blocked");
        isDefense = am.ac.CheckState("defense", "defense");
    }

    // HP����㣨�ޤ��Ϝp�㣩����᥽�å�
    public void AddHp(float value)
    {
        HP += value; // HP�˂������
        HP = Mathf.Clamp(HP, 0, HPMax); // HP��0�������HP���g�ǥ�����

        if (HP > 0) // HP��0���󤭤�����
        {
            am.Hit(); // �ҥåȥ᥽�åɤ���ӳ���
        }
        else // ��������Έ��ϣ�HP��0���£�
        {
            am.Die(); // �����᥽�åɤ���ӳ���
        }
    }

    // �ƥ��ȥ᥽�åɣ�HP���ʾ��
    public void Test()
    {
        print(HP);
    }
}
