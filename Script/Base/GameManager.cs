using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON; // Defective.JSON��ʹ��

public class GameManager : MonoBehaviour
{
    public WeaponManger testWm; 

    private static GameManager instance;
    private DataBase weaponDB; // �����ǩ`���٩`��
    private WeaponFactory weaponFact;

    void Awake() 
    {
        CheckGameObject(); // ���`�४�֥������Ȥ�����å�
        CheckSingle(); // ���󥰥륤�󥹥��󥹤�����å�
    }

    private void Start() 
    {
        InitWeaponDB(); 
        InitWeaponFactory(); 

        // �ƥ����������ޥͩ`������������饤���`�����
        testWm.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Sword", "R", testWm));
    }


    private void InitWeaponDB()
    {
        weaponDB = new DataBase();
    }


    private void InitWeaponFactory()
    {
        weaponFact = new WeaponFactory(weaponDB);
    }

    // ���󥰥륤�󥹥��󥹤�����å�
    private void CheckSingle()
    {
        if (instance == null) // ���󥹥��󥹤����ڤ��ʤ�����
        {
            instance = this; // ���󥹥��󥹤�������O��
            DontDestroyOnLoad(gameObject); // ���`���`�ɕr���Ɨ����ʤ�
            return;
        }
        Destroy(this); // �Ȥ˥��󥹥��󥹤����ڤ�����ϡ�������Ɨ�
    }

    // ���`�४�֥������Ȥ�����å�
    private void CheckGameObject()
    {
        if (tag == "GM") // ������GM�Έ���
        {
            return;
        }
        Destroy(this); // ������GM�Ǥʤ����ϡ�������Ɨ�
    }
}