using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class AIBattleManager : MonoBehaviour
{
    public AIActorManger aam;
    private CapsuleCollider defCol;

    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();

        defCol.isTrigger = true;

    }
    private void OnTriggerEnter(Collider col)
    {

            aam.TryDoDamage(col.GetComponentInParent<WeaponController>());

    }
}
