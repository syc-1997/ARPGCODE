using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    public WeaponManger wm; // 武器管理者
    public WeaponData wdata; // 武器データ

    private void Awake() 
    {
        // 子要素からWeaponDataコンポーネントを取得
        wdata = GetComponentInChildren<WeaponData>();
    }

    public float GetATK() 
    {
        // WeaponDataから攻撃力を取得して返す
        return wdata.ATK;
    }
}
