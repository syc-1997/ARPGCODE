using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory
{
    private DataBase weaponDB;

    public WeaponFactory(DataBase _weaponDB)
    {
        weaponDB = _weaponDB;
    }

    // �������֥������Ȥ����ɤ���᥽�å�
    public GameObject CreateWeapon(string weaponName, Vector3 pos, Quaternion rot)
    {
        // �ץ�ϥ֤��`�ɤ���
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        // �������֥������Ȥ����ɤ���
        GameObject obj = GameObject.Instantiate(prefab, pos, rot);

        // �����ǩ`���򥳥�ݩ`�ͥ�ȤȤ���׷�Ӥ���
        WeaponData wdate = obj.AddComponent<WeaponData>();
        // �����ǩ`����ǩ`���٩`�������i���z����O������
        wdate.ATK = weaponDB.weaponDataBaes[weaponName]["ATK"].floatValue;

        return obj;
    }

    // �������饤���`���֥������Ȥ����ɤ���᥽�å�
    public Collider CreateWeapon(string weaponName, string side, WeaponManger wm)
    {
        WeaponController wc;

        // ���Ҥɤ�����֤˳֤Ĥ��ǡ�������֤ĥ���ȥ�`��`���x�k����
        if (side == "L")
        {
            wc = wm.wcL;
        }
        else if (side == "R")
        {
            wc = wm.wcR;
        }
        else
        {
            return null;
        }

        // �ץ�ϥ֤��`�ɤ���
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        // �������֥������Ȥ����ɤ���
        GameObject obj = GameObject.Instantiate(prefab);

        // �������֥������Ȥ��H���֥������Ȥ򡢳֤ĥ���ȥ�`��`���O������
        obj.transform.parent = wc.transform;
        // �������֥������Ȥ�λ�ä򡢳֤ĥ���ȥ�`��`�����Ĥ��O������
        obj.transform.localPosition = Vector3.zero;
        // �������֥������Ȥλ�ܞ�򡢳֤ĥ���ȥ�`��`��һ�¤�����
        obj.transform.localRotation = Quaternion.identity;

        // �����ǩ`���򥳥�ݩ`�ͥ�ȤȤ���׷�Ӥ���
        WeaponData wdate = obj.AddComponent<WeaponData>();
        // �����ǩ`����ǩ`���٩`�������i���z����O������
        wdate.ATK = weaponDB.weaponDataBaes[weaponName]["ATK"].floatValue;
        // ��������ȥ�`��`�ˡ������ǩ`�����O������
        wc.wdata = wdate;

        // �����Υ��饤���`����ݩ`�ͥ�Ȥ򷵤�
        return obj.GetComponent<Collider>();
    }
}