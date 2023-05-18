using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : IUserInput
{
    [Header("===== Key setting =====")]
    public string keyUp = "w"; // 上移動のキー
    public string keyDown = "s"; // 下移動のキー
    public string keyLeft = "a"; // 左移動のキー
    public string keyRight = "d"; // 右移動のキー

    public string keyRun; // 走るためのキー
    public string keyJump; // ジャンプのキー

    public string btnLB; // 左側のボタン
    public string btnRB; // 右側のボタン
    public string Mouse0; // マウス左ボタン
    public string Mouse1; // マウス右ボタン
    public string keyQ; // Qキー
    public string keyE; // Eキー

    public string keyLock; // ロックするためのキー

    public MyButton KeyShift = new MyButton();
    public MyButton KeySpace = new MyButton();
    public MyButton buttonMouse0 = new MyButton();
    public MyButton buttonMouse1 = new MyButton();
    public MyButton KeyE = new MyButton();
    public MyButton KeyQ = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonRT = new MyButton();
    public MyButton KeyLock = new MyButton();

    Vector2 tempDAxis; // 方向入力用のVector2型変数
    float Dright2; // 左右移動のための変数
    float Dup2; // 上下移動のための変数

    void Update()
    {
        // 各ボタンの状態を更新します。
        KeyShift.Tick(Input.GetKey(keyRun));
        KeySpace.Tick(Input.GetKey(keyJump));
        buttonMouse0.Tick(Input.GetButton(Mouse0));
        buttonMouse1.Tick(Input.GetButton(Mouse1));
        KeyE.Tick(Input.GetKey(keyE));
        KeyQ.Tick(Input.GetKey(keyQ));
        KeyLock.Tick(Input.GetButton(keyLock));

        // 上下左右の入力を取得する
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        Jup = Input.GetAxis("Mouse Y") * 2f;
        Jright = Input.GetAxis("Mouse X") * 2f;

        // 入力が無効の場合、上下左右の入力を0にする
        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }
        // ジョイスティックの入力値を滑らかに変化させる
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        // 上下左右の入力を楕円に変換する
        tempDAxis = SquareToCircle(new Vector2(Dright, Dup));

        Dright2 = tempDAxis.x;
        Dup2 = tempDAxis.y;

        // 上下左右の入力のベクトルを計算する
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        // 移動やアクションのフラグを更新する
        run = (KeyShift.IsPressing && !KeyShift.IsDelaying) 
            || (KeyShift.IsExteding && !KeyShift.IsDelaying);
        roll = (KeyShift.OnReleased && KeyShift.IsDelaying);

        jump = KeySpace.OnPressed;

        defense = KeyQ.IsPressing;

        lockon = KeyLock.OnPressed;

        rt = buttonMouse0.OnPressed;
        lt = buttonMouse1.OnPressed;
        rb = KeyE.IsPressing;
        lb = KeyQ.IsPressing;

    }


    
}
