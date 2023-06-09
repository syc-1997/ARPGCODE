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

    // 冷匂オブジェクトを伏撹するメソッド
    public GameObject CreateWeapon(string weaponName, Vector3 pos, Quaternion rot)
    {
        // プレハブをロ�`ドする
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        // 冷匂オブジェクトを伏撹する
        GameObject obj = GameObject.Instantiate(prefab, pos, rot);

        // 冷匂デ�`タをコンポ�`ネントとして弖紗する
        WeaponData wdate = obj.AddComponent<WeaponData>();
        // 冷匂デ�`タをデ�`タベ�`スから�iみ�zんで�O協する
        wdate.ATK = weaponDB.weaponDataBaes[weaponName]["ATK"].floatValue;

        return obj;
    }

    // 冷匂コライダ�`オブジェクトを伏撹するメソッド
    public Collider CreateWeapon(string weaponName, string side, WeaponManger wm)
    {
        WeaponController wc;

        // 恣嘔どちらの返に隔つかで、冷匂を隔つコントロ�`ラ�`を�x�kする
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

        // プレハブをロ�`ドする
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        // 冷匂オブジェクトを伏撹する
        GameObject obj = GameObject.Instantiate(prefab);

        // 冷匂オブジェクトの�Hオブジェクトを、隔つコントロ�`ラ�`に�O協する
        obj.transform.parent = wc.transform;
        // 冷匂オブジェクトの了崔を、隔つコントロ�`ラ�`の嶄伉に�O協する
        obj.transform.localPosition = Vector3.zero;
        // 冷匂オブジェクトの指��を、隔つコントロ�`ラ�`と匯崑させる
        obj.transform.localRotation = Quaternion.identity;

        // 冷匂デ�`タをコンポ�`ネントとして弖紗する
        WeaponData wdate = obj.AddComponent<WeaponData>();
        // 冷匂デ�`タをデ�`タベ�`スから�iみ�zんで�O協する
        wdate.ATK = weaponDB.weaponDataBaes[weaponName]["ATK"].floatValue;
        // 冷匂コントロ�`ラ�`に、冷匂デ�`タを�O協する
        wc.wdata = wdate;

        // 冷匂のコライダ�`コンポ�`ネントを卦す
        return obj.GetComponent<Collider>();
    }
}