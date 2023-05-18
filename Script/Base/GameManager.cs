using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON; // Defective.JSONを使用

public class GameManager : MonoBehaviour
{
    public WeaponManger testWm; 

    private static GameManager instance;
    private DataBase weaponDB; // 武器データベース
    private WeaponFactory weaponFact;

    void Awake() 
    {
        CheckGameObject(); // ゲームオブジェクトをチェック
        CheckSingle(); // シングルインスタンスをチェック
    }

    private void Start() 
    {
        InitWeaponDB(); 
        InitWeaponFactory(); 

        // テスト用武器マネージャの武器コライダーを更新
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

    // シングルインスタンスをチェック
    private void CheckSingle()
    {
        if (instance == null) // インスタンスが存在しない場合
        {
            instance = this; // インスタンスに自身を設定
            DontDestroyOnLoad(gameObject); // シーンロード時に破棄しない
            return;
        }
        Destroy(this); // 既にインスタンスが存在する場合、自身を破棄
    }

    // ゲームオブジェクトをチェック
    private void CheckGameObject()
    {
        if (tag == "GM") // タグがGMの場合
        {
            return;
        }
        Destroy(this); // タグがGMでない場合、自身を破棄
    }
}