using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInput : IUserInput
{
    [Header("===== Joystick Setting =====")]
    public string axisX;    // 嘔スティックの罪�Sの兆念
    public string axisY;    // 嘔スティックの�k�Sの兆念
    public string axis4;    // 恣スティックの罪�Sの兆念
    public string axis5;    // 恣スティックの�k�Sの兆念

    public string keyRun;   // 恠るキ�`の兆念
    public string keyJump;  // ジャンプキ�`の兆念
    public string keyAttack; // 好�張��`の兆念
    public string keyY;     // Yキ�`の兆念

    public string btnLB;    // 恣バンパ�`の兆念
    public string btnRB;    // 嘔バンパ�`の兆念
    public string btnLT;    // 恣トリガ�`の兆念
    public string btnRT;    // 嘔トリガ�`の兆念
    public string keyLock;  // ロックオンキ�`の兆念

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
        // 光ボタンの彜�Bを厚仟します。
        buttonA.Tick (Input.GetButton(keyRun));
        buttonB.Tick (Input.GetButton(keyJump));
        buttonX.Tick (Input.GetButton(keyAttack));
        buttonY.Tick (Input.GetButton(keyY));
        buttonLB.Tick (Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonR3.Tick(Input.GetButton(keyLock));
        buttonLT.Tick (Input.GetAxis(btnLT) < 0 ? true : false);
        buttonRT.Tick(Input.GetAxis(btnRT) > 0 ? true : false);

        // ジョイスティックの秘薦�､鯣ゝ辰垢�
        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);

        // ジョイスティックの4桑朕と5桑朕の�Sの�､鯣ゝ辰垢�
        Jup = Input.GetAxis(axis5);
        Jright = Input.GetAxis(axis4);
        // inputEnabledがfalseの��栽、秘薦�､�0にする
        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }
        // ジョイスティックの秘薦�､鮖�らかに�篁�させる
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);



        // ジョイスティックの秘薦�､鰓�に、DmagとDvecを��麻する
        Dmag = Mathf.Clamp(Mathf.Abs(Dup)+ Mathf.Abs(Dright), 0.0f, 1.0f);
        Dvec = Dright * transform.right + Dup * transform.forward;

        // 卞�咾筌▲�ションのフラグを厚仟する
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
