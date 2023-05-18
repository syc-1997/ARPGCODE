using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StateManagerクラス定義
public class StateManager : IActorMangerInterface
{
    //public ActorManger am; // ActorManager（現在はコメントアウト）
    public float HP = 15.0f; // HP初期値
    public float HPMax = 15.0f; // HP最大値

    // 一次状態フラグ
    [Header("1st Order state flags")]
    public bool isGround; // 地面にいるか
    public bool isJump; // ジャンプしているか
    public bool isFall; // 落下しているか
    public bool isRoll; // ロールしているか
    public bool isAttack; // 攻撃しているか
    public bool isHit; // ヒットしているか
    public bool isDie; // 死亡しているか
    public bool isBlocked; // ブロックしているか
    public bool isDefense; // 防御しているか

    private void Start() // 開始時に呼び出されるメソッド
    {
        HP = HPMax; // HPを最大値に設定
    }
    private void Update() // 毎フレーム呼ばれるメソッド
    {
        // 各状態をチェック
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("Jump");
        isFall = am.ac.CheckState("Fall");
        isRoll = am.ac.CheckState("Roll");
        isAttack = am.ac.CheckStateTag("AttackR") || am.ac.CheckStateTag("AttackL");
        isHit = am.ac.CheckState("Hit");
        isDie = am.ac.CheckState("Dead");
        isBlocked = am.ac.CheckState("blocked");
        isDefense = am.ac.CheckState("defense", "defense");
    }

    // HPを加算（または減算）するメソッド
    public void AddHp(float value)
    {
        HP += value; // HPに値を加算
        HP = Mathf.Clamp(HP, 0, HPMax); // HPを0から最大HPの間でクランプ

        if (HP > 0) // HPが0より大きい場合
        {
            am.Hit(); // ヒットメソッドを呼び出す
        }
        else // それ以外の場合（HPが0以下）
        {
            am.Die(); // 死亡メソッドを呼び出す
        }
    }

    // テストメソッド（HPを表示）
    public void Test()
    {
        print(HP);
    }
}
