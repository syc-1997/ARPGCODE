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

    // 武器オブジェクトを生成するメソッド
    public GameObject CreateWeapon(string weaponName, Vector3 pos, Quaternion rot)
    {
        // プレハブをロードする
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        // 武器オブジェクトを生成する
        GameObject obj = GameObject.Instantiate(prefab, pos, rot);

        // 武器データをコンポーネントとして追加する
        WeaponData wdate = obj.AddComponent<WeaponData>();
        // 武器データをデータベースから読み込んで設定する
        wdate.ATK = weaponDB.weaponDataBaes[weaponName]["ATK"].floatValue;

        return obj;
    }

    // 武器コライダーオブジェクトを生成するメソッド
    public Collider CreateWeapon(string weaponName, string side, WeaponManger wm)
    {
        WeaponController wc;

        // 左右どちらの手に持つかで、武器を持つコントローラーを選択する
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

        // プレハブをロードする
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        // 武器オブジェクトを生成する
        GameObject obj = GameObject.Instantiate(prefab);

        // 武器オブジェクトの親オブジェクトを、持つコントローラーに設定する
        obj.transform.parent = wc.transform;
        // 武器オブジェクトの位置を、持つコントローラーの中心に設定する
        obj.transform.localPosition = Vector3.zero;
        // 武器オブジェクトの回転を、持つコントローラーと一致させる
        obj.transform.localRotation = Quaternion.identity;

        // 武器データをコンポーネントとして追加する
        WeaponData wdate = obj.AddComponent<WeaponData>();
        // 武器データをデータベースから読み込んで設定する
        wdate.ATK = weaponDB.weaponDataBaes[weaponName]["ATK"].floatValue;
        // 武器コントローラーに、武器データを設定する
        wc.wdata = wdate;

        // 武器のコライダーコンポーネントを返す
        return obj.GetComponent<Collider>();
    }
}