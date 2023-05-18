using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    public enum STATE
    {
        IDLE,      // �����ɥ�״�B���Τ⤷�Ƥ��ʤ���
        RUN,       // �g��״�B��������ȥ������У�
        FINISHED   // �K��״�B��������ȥ����󤬽K��ä���
    }
    public STATE state;         // MyTimer�άF�ڤ�״�B����В���

    public float duration = 1.0f; // �����ީ`�γ־A�r�g
    private float elapsedTime = 0; // �U�^�r�g
    public void Tick()
    {
        switch (state)
        {
            case (STATE.IDLE):  // �����ɥ�״�B�ǤϺΤ⤷�ʤ�
                break;
            case (STATE.RUN):   // �g��״�B�Ǥϕr�g�򥫥���ȥ����󤹤�
                elapsedTime += Time.deltaTime; // �U�^�r�g����¤���
                if (elapsedTime >= duration)   // �־A�r�g���_�����顢״�B��FINISHED�ˉ������
                {
                    state = STATE.FINISHED;
                }
                break;
            case (STATE.FINISHED): // �K��״�B�ǤϺΤ⤷�ʤ�
                break;
            default: // ״�B�������ʈ��Ϥϥ���`���ʾ����
                Debug.Log("MyTimer error");
                break;
        }
    }

    public void Go()
    {
        elapsedTime = 0;  // �U�^�r�g����ڻ�����
        state = STATE.RUN; // ״�B��RUN�ˉ������
    }
}