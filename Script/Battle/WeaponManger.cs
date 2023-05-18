using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManger : IActorMangerInterface
{
    public Collider WeaponColL; // ��Ȥ������Υ��饤���`
    public Collider WeaponColR; // �҂Ȥ������Υ��饤���`

    public GameObject whL; // ��Ȥ������ϥ�ɥ�
    public GameObject whR; // �҂Ȥ������ϥ�ɥ�

    public WeaponController wcL; // ��Ȥ���������ȥ�`��
    public WeaponController wcR; // �҂Ȥ���������ȥ�`��

    private void Awake() // Awake�᥽�å�
    {
        // �����ϥ�ɥ��Ҋ�Ĥ��ƥХ����
        whL = transform.DeepFind("weaponHandleL").gameObject;
        wcL = BindWeaponController(whL);

        whR = transform.DeepFind("weaponHandleR").gameObject;
        wcR = BindWeaponController(whR);
    }

    private void Start() // Start�᥽�å�
    {
        // �����Υ��饤���`��ȡ��
        WeaponColL = whL.GetComponentInChildren<Collider>();
        WeaponColR = whR.GetComponentInChildren<Collider>();
    }

    // �����Υ��饤���`�����
    public void UpdateWeaponCollider(string side, Collider col)
    {
        if (side == "L") // ��ȤΈ���
        {
            WeaponColL = col;
        }
        else if (side == "R") // �҂ȤΈ���
        {
            WeaponColR = col;
        }
    }

    // WeaponController��Х����
    public WeaponController BindWeaponController(GameObject targetObj)
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>(); 
        if (tempWc == null) // ���ڤ��ʤ�����
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this; 

        return tempWc; // WeaponController�򷵤�
    }

    // �������Є��ˤ���
    public void WeaponEnable()
    {
        if (am.ac.CheckStateTag("AttackL")) // ��ȹ��ĤΈ���
        {
            WeaponColL.enabled = true;
        }
        if (am.ac.CheckStateTag("AttackR")) // �҂ȹ��ĤΈ���
        {
            WeaponColR.enabled = true;
        }
    }

    // ������o���ˤ���
    public void WeaponDisable()
    {
        if (WeaponColL != null) // ��ȤΥ��饤���`�����ڤ������
        {
            WeaponColL.enabled = false;
        }
        if (WeaponColR != null) // �҂ȤΥ��饤���`�����ڤ������
        {
            WeaponColR.enabled = false;
        }
    }
}
