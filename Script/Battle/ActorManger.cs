using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ActorManager���饹���x
public class ActorManger : MonoBehaviour
{
    public ActorController ac; // �������`����ȥ�`��
    public BattleManager bm; // �Хȥ�ޥͩ`����
    public WeaponManger wm; // �����ݥ�ޥͩ`����
    public StateManager sm; // ���Ʃ`�ȥޥͩ`����


    void Awake()
    {
        ac = GetComponent<ActorController>(); // ActorController����ݩ`�ͥ�Ȥ�ȡ��

        GameObject model = ac.model; // ��ǥ륪�֥������Ȥ�ȡ��
        GameObject obj = GetComponent<GameObject>(); // GameObject��ȡ��

        GameObject sensor = transform.Find("sensor").gameObject; // ���󥵩`���֥������Ȥ�ȡ��
        bm = Bind<BattleManager>(sensor); 
        wm = Bind<WeaponManger>(model); 
        sm = Bind<StateManager>(gameObject); 
    }

    // �����ͥ�å��᥽�åɤǡ�������ָ�����줿GameObject�ˌ�����ָ�����줿�����פΥ���ݩ`�ͥ�Ȥ�Х����
    private T Bind<T>(GameObject go) where T : IActorMangerInterface
    {
        T temInstance;
        temInstance = go.GetComponent<T>(); // ����ݩ`�ͥ�Ȥ�ȡ��
        if (temInstance == null) // ����ݩ`�ͥ�Ȥ����ڤ��ʤ�����
        {
            temInstance = go.AddComponent<T>(); // ����ݩ`�ͥ�Ȥ�׷��
        }
        temInstance.am = this; // ActorManager���O��
        return temInstance; // ���󥹥��󥹤򷵤�
    }



    // ����`��
    public void TryDoDamage(AIWeaponController targetWc)
    {
        if (sm.HP > 0) // HP��0���󤭤�����
        {
            sm.AddHp(-1 * targetWc.GetATK()); // HP��p��
        }
    }

    // �ҥåȥ᥽�å�
    public void Hit()
    {
        ac.IssueTrigger("hit"); // "hit"�ȥꥬ�`�ΰk��
    }

    // �����᥽�å�
    public void Die()
    {
        ac.IssueTrigger("die"); // "die"�ȥꥬ�`�ΰk��
        ac.pi.inputEnabled = false; // ������o����

        // һ���r�g��˥��`�४�֥������Ȥ��Ɨ�
        Destroy(gameObject, 2f);
    }
}
