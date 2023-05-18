using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    public AIWeaponManger awm;
    public WeaponData wdata;

    private void Awake()
    {
        wdata = GetComponentInChildren<WeaponData>();
    }

    public float GetATK()
    {
        return wdata.ATK;
    }
}
