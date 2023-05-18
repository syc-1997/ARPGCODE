using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActorManger : MonoBehaviour
{
    public AIActorController aac;
    public AIBattleManager abm;
    public AIWeaponManger awm;
    public AIStateManager asm;

    // Start is called before the first frame update
    void Awake()
    {
        aac = GetComponent<AIActorController>();

        GameObject model = aac.model;
        GameObject obj = GetComponent<GameObject>();

        GameObject sensor = transform.Find ("sensor").gameObject;

    }

 
    void Update()
    {
        
    }

    public void TryDoDamage(WeaponController targetWc)
    {
        if(asm.HP > 0)
        {
            asm.AddHp(-1 * targetWc.GetATK());
        }
    }

    public void Hit()
    {
        aac.SetState(AIActorController.State.Hit);
    }

    public void Die()
    {
       aac.SetState(AIActorController.State.Die);
       
        // if (ac.camcon.lockState == true  && dii == null)
        // {
        //     ac.camcon.LockUnlock();
        // }
        
    }
}
