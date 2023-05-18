using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : IUserInput
{
    [Header("===== Key setting =====")]
    public string keyUp = "w"; // ���ƄӤΥ��`
    public string keyDown = "s"; // ���ƄӤΥ��`
    public string keyLeft = "a"; // ���ƄӤΥ��`
    public string keyRight = "d"; // ���ƄӤΥ��`

    public string keyRun; // �ߤ뤿��Υ��`
    public string keyJump; // �����פΥ��`

    public string btnLB; // ��ȤΥܥ���
    public string btnRB; // �҂ȤΥܥ���
    public string Mouse0; // �ޥ�����ܥ���
    public string Mouse1; // �ޥ����ҥܥ���
    public string keyQ; // Q���`
    public string keyE; // E���`

    public string keyLock; // ��å����뤿��Υ��`

    public MyButton KeyShift = new MyButton();
    public MyButton KeySpace = new MyButton();
    public MyButton buttonMouse0 = new MyButton();
    public MyButton buttonMouse1 = new MyButton();
    public MyButton KeyE = new MyButton();
    public MyButton KeyQ = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonRT = new MyButton();
    public MyButton KeyLock = new MyButton();

    Vector2 tempDAxis; // ���������ä�Vector2�͉���
    float Dright2; // �����ƄӤΤ���Ή���
    float Dup2; // �����ƄӤΤ���Ή���

    void Update()
    {
        // ���ܥ����״�B����¤��ޤ���
        KeyShift.Tick(Input.GetKey(keyRun));
        KeySpace.Tick(Input.GetKey(keyJump));
        buttonMouse0.Tick(Input.GetButton(Mouse0));
        buttonMouse1.Tick(Input.GetButton(Mouse1));
        KeyE.Tick(Input.GetKey(keyE));
        KeyQ.Tick(Input.GetKey(keyQ));
        KeyLock.Tick(Input.GetButton(keyLock));

        // �������Ҥ�������ȡ�ä���
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        Jup = Input.GetAxis("Mouse Y") * 2f;
        Jright = Input.GetAxis("Mouse X") * 2f;

        // �������o���Έ��ϡ��������Ҥ�������0�ˤ���
        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }
        // ���祤���ƥ��å����������򻬤餫�ˉ仯������
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        // �������Ҥ���������Ҥˉ�Q����
        tempDAxis = SquareToCircle(new Vector2(Dright, Dup));

        Dright2 = tempDAxis.x;
        Dup2 = tempDAxis.y;

        // �������Ҥ������Υ٥��ȥ��Ӌ�㤹��
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        // �ƄӤ䥢�������Υե饰����¤���
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
