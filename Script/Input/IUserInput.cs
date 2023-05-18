using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("===== Output singnals =====")]
    public float Dup;           // 前後の入力
    public float Dright;        // 左右の入力
    public float Dmag;          // 入力の強さ
    public Vector3 Dvec;        // 入力のベクトル
    public float Jup;           // ジョイスティックの上下入力
    public float Jright;        // ジョイスティックの左右入力

    public bool run;            // 走る
    public bool roll;           // ロール
    public bool defense;        // 防御
    public bool jump;           // ジャンプ
    protected bool lastjump;    // 直前のジャンプ入力
    //public bool attack;
    public bool lb;             // 左バンパー
    public bool rb;             // 右バンパー
    public bool lt;             // 左トリガー
    public bool rt;             // 右トリガー

    public bool lockon;         // ロックオン
    protected bool lastattack;  // 直前の攻撃入力
    protected bool isHolding = false; // ボタンが押されているかどうか

    [Header("===== Orthers =====")]
    public bool inputEnabled = true;    // 入力が有効かどうか

    protected float targetDup;          // 目標前後入力
    protected float targetDright;       // 目標左右入力
    protected float velocityDup;        // 前後入力の速度
    protected float velocityDright;     // 左右入力の速度
    protected float holdTime;           // ボタンが押されている時間
    protected Vector2 SquareToCircle(Vector2 input) // 四角形座標を円形座標に変換する
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);
        return output;
    }

    // 前後左右の入力からベクトルと入力の強さを計算する
    protected void UpdateDmagDvec(float Dup2, float Dright2)
    {
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
    }
}

