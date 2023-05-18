using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInput : IUserInput
{
    [Header("===== Joystick Setting =====")]
    public string axisX;    // �ҥ��ƥ��å��κ��S����ǰ
    public string axisY;    // �ҥ��ƥ��å��οk�S����ǰ
    public string axis4;    // �󥹥ƥ��å��κ��S����ǰ
    public string axis5;    // �󥹥ƥ��å��οk�S����ǰ

    public string keyRun;   // �ߤ륭�`����ǰ
    public string keyJump;  // �����ץ��`����ǰ
    public string keyAttack; // ���ĥ��`����ǰ
    public string keyY;     // Y���`����ǰ

    public string btnLB;    // ��Х�ѩ`����ǰ
    public string btnRB;    // �ҥХ�ѩ`����ǰ
    public string btnLT;    // ��ȥꥬ�`����ǰ
    public string btnRT;    // �ҥȥꥬ�`����ǰ
    public string keyLock;  // ��å����󥭩`����ǰ

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
        // ���ܥ����״�B����¤��ޤ���
        buttonA.Tick (Input.GetButton(keyRun));
        buttonB.Tick (Input.GetButton(keyJump));
        buttonX.Tick (Input.GetButton(keyAttack));
        buttonY.Tick (Input.GetButton(keyY));
        buttonLB.Tick (Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonR3.Tick(Input.GetButton(keyLock));
        buttonLT.Tick (Input.GetAxis(btnLT) < 0 ? true : false);
        buttonRT.Tick(Input.GetAxis(btnRT) > 0 ? true : false);

        // ���祤���ƥ��å�����������ȡ�ä���
        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);

        // ���祤���ƥ��å���4��Ŀ��5��Ŀ���S�΂���ȡ�ä���
        Jup = Input.GetAxis(axis5);
        Jright = Input.GetAxis(axis4);
        // inputEnabled��false�Έ��ϡ���������0�ˤ���
        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }
        // ���祤���ƥ��å����������򻬤餫�ˉ仯������
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);



        // ���祤���ƥ��å�����������Ԫ�ˡ�Dmag��Dvec��Ӌ�㤹��
        Dmag = Mathf.Clamp(Mathf.Abs(Dup)+ Mathf.Abs(Dright), 0.0f, 1.0f);
        Dvec = Dright * transform.right + Dup * transform.forward;

        // �ƄӤ䥢�������Υե饰����¤���
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
