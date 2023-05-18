using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    public WeaponManger wm; // ����������
    public WeaponData wdata; // �����ǩ`��

    private void Awake() 
    {
        // ��Ҫ�ؤ���WeaponData����ݩ`�ͥ�Ȥ�ȡ��
        wdata = GetComponentInChildren<WeaponData>();
    }

    public float GetATK() 
    {
        // WeaponData���鹥������ȡ�ä��Ʒ���
        return wdata.ATK;
    }
}
