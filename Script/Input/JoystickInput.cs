using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInput : IUserInput
{
    [Header("===== Joystick Setting =====")]
    public string axisX;    // 右スティックの横軸の名前
    public string axisY;    // 右スティックの縦軸の名前
    public string axis4;    // 左スティックの横軸の名前
    public string axis5;    // 左スティックの縦軸の名前

    public string keyRun;   // 走るキーの名前
    public string keyJump;  // ジャンプキーの名前
    public string keyAttack; // 攻撃キーの名前
    public string keyY;     // Yキーの名前

    public string btnLB;    // 左バンパーの名前
    public string btnRB;    // 右バンパーの名前
    public string btnLT;    // 左トリガーの名前
    public string btnRT;    // 右トリガーの名前
    public string keyLock;  // ロックオンキーの名前

    public MyButton buttonA = new MyButton();  
    public MyButton buttonB = new MyButton();  
    public MyButton buttonX = new MyButton();  
    public MyButton buttonY = new MyButton();  
    public MyButton buttonLB = new MyButton(); 
    public MyButton buttonRB = new MyButton(); 
    public MyButton buttonLT = new MyButton(); 
    public MyButton buttonRT = new MyButton(); 
    public MyButton buttonR3 = new MyButton(); 




    void Update()
    {
        // 各ボタンの状態を更新します。
        buttonA.Tick (Input.GetButton(keyRun));
        buttonB.Tick (Input.GetButton(keyJump));
        buttonX.Tick (Input.GetButton(keyAttack));
        buttonY.Tick (Input.GetButton(keyY));
        buttonLB.Tick (Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonR3.Tick(Input.GetButton(keyLock));
        buttonLT.Tick (Input.GetAxis(btnLT) < 0 ? true : false);
        buttonRT.Tick(Input.GetAxis(btnRT) > 0 ? true : false);

        // ジョイスティックの入力値を取得する
        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);

        // ジョイスティックの4番目と5番目の軸の値を取得する
        Jup = Input.GetAxis(axis5);
        Jright = Input.GetAxis(axis4);
        // inputEnabledがfalseの場合、入力値を0にする
        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }
        // ジョイスティックの入力値を滑らかに変化させる
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);



        // ジョイスティックの入力値を元に、DmagとDvecを計算する
        Dmag = Mathf.Clamp(Mathf.Abs(Dup)+ Mathf.Abs(Dright), 0.0f, 1.0f);
        Dvec = Dright * transform.right + Dup * transform.forward;

        // 移動やアクションのフラグを更新する
        run = (buttonA.IsPressing && !buttonA.IsDelaying)
            || (buttonA.IsExteding && !buttonA.IsDelaying);
        roll = (buttonA.OnReleased && buttonA.IsDelaying);

        defense = buttonLB.IsPressing;

        jump = buttonB.OnPressed;

        lockon = buttonR3.OnPressed;
        rt = buttonRT.OnPressed;
        lt = buttonLT.OnPressed;
        rb = buttonRB.IsPressing;
        lb = buttonLB.IsPressing;


    }
}
