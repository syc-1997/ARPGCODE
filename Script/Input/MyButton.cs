using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton 
{
    public bool IsPressing = false; // Ѻ����A���Ƥ��뤫
    public bool OnPressed = false; // �ܥ���Ѻ���줿˲�g
    public bool OnReleased = false; // �ܥ����x���줿˲�g
    public bool IsExteding = false; // �LѺ��״�B���ɤ���
    public bool IsDelaying = false; // �ܥ����x����Ƥ���һ���r�g�ڤ��ɤ���

    public float extendingDuration = 0.3f; // �LѺ���ж��Εr�g
    public float delayingDuration = 0.3f;  // �ܥ�����x�������һ���r�g

    private bool curSate = false;        // �F�ڤ�״�B
    private bool lastState = false;      // ǰ�ؤ�״�B

    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();
    public void Tick(bool input)
    {
        extTimer.Tick();    // �LѺ�������ީ`�θ���
        delayTimer.Tick();  // ���C�����ީ`�θ���

        curSate = input;    // �F�ڤΥܥ���״�B��ȡ��

        IsPressing = curSate;   // Ѻ����Ƥ��뤫�ɤ������ж�

        OnPressed = false;  // �ܥ���Ѻ���줿�����ж�����ڻ�
        OnReleased = false; // �ܥ����x���줿�����ж�����ڻ�
        IsExteding = false; // �LѺ��״�B���ɤ������ж�����ڻ�
        IsDelaying = false; // �ܥ����x����Ƥ���һ���r�g�ڤ��ɤ������ж�����ڻ�


        // �F�ڤ�״�B��ǰ�ؤ�״�B���`������
        if (curSate != lastState)
        {
            if (curSate == true)    // �F�ڤ��ܥ���Ѻ���줿״�B
            {
                OnPressed = true;   // �ܥ���Ѻ���줿״�B�򥻥å�
                StartTimer(delayTimer, delayingDuration);    // �ܥ�����x������δ��C�����ީ`���_ʼ
            }
            else    // �F�ڤ��ܥ����x���줿״�B
            {
                OnReleased = true;  // �ܥ����x���줿״�B�򥻥å�
                StartTimer(extTimer, extendingDuration);    // �LѺ�������ީ`���_ʼ
            }
        }
        lastState = curSate;

        // �����ީ`���g���ФǤ�����ϡ�IsExteding��true���O������
        if (extTimer.state == MyTimer.STATE.RUN)
        {
            IsExteding = true;
        }

        // �����ީ`���g���ФǤ�����ϡ�IsDelaying��true���O������
        if (delayTimer.state == MyTimer.STATE.RUN)
        {
            IsDelaying = true;
        }
    }

    // �����ީ`���_ʼ����᥽�å�
    private void StartTimer(MyTimer timer, float duration)
    {
        // �����ީ`�γ־A�r�g���O������
        timer.duration = duration;
        // �����ީ`���_ʼ����
        timer.Go();
    }
}
