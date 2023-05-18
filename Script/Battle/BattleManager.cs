using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorMangerInterface
{
    private CapsuleCollider defCol;

    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();

        defCol.isTrigger = true;

    }
    private void OnTriggerEnter(Collider col)
    {
        
            am.TryDoDamage(col.GetComponentInParent<AIWeaponController>());

    }
}
