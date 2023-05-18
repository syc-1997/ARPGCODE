using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton 
{
    public bool IsPressing = false; // 押され続けているか
    public bool OnPressed = false; // ボタンが押された瞬間
    public bool OnReleased = false; // ボタンが離された瞬間
    public bool IsExteding = false; // 長押し状態かどうか
    public bool IsDelaying = false; // ボタンが離されてから一定時間内かどうか

    public float extendingDuration = 0.3f; // 長押し判定の時間
    public float delayingDuration = 0.3f;  // ボタンを離した後の一定時間

    private bool curSate = false;        // 現在の状態
    private bool lastState = false;      // 前回の状態

    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();
    public void Tick(bool input)
    {
        extTimer.Tick();    // 長押しタイマーの更新
        delayTimer.Tick();  // 待機タイマーの更新

        curSate = input;    // 現在のボタン状態を取得

        IsPressing = curSate;   // 押されているかどうかを判定

        OnPressed = false;  // ボタンが押されたかの判定を初期化
        OnReleased = false; // ボタンが離されたかの判定を初期化
        IsExteding = false; // 長押し状態かどうかの判定を初期化
        IsDelaying = false; // ボタンが離されてから一定時間内かどうかの判定を初期化


        // 現在の状態と前回の状態が違う場合
        if (curSate != lastState)
        {
            if (curSate == true)    // 現在がボタンが押された状態
            {
                OnPressed = true;   // ボタンが押された状態をセット
                StartTimer(delayTimer, delayingDuration);    // ボタンを離した後の待機タイマーを開始
            }
            else    // 現在がボタンが離された状態
            {
                OnReleased = true;  // ボタンが離された状態をセット
                StartTimer(extTimer, extendingDuration);    // 長押しタイマーを開始
            }
        }
        lastState = curSate;

        // タイマーが実行中である場合、IsExtedingにtrueを設定する
        if (extTimer.state == MyTimer.STATE.RUN)
        {
            IsExteding = true;
        }

        // タイマーが実行中である場合、IsDelayingにtrueを設定する
        if (delayTimer.state == MyTimer.STATE.RUN)
        {
            IsDelaying = true;
        }
    }

    // タイマーを開始するメソッド
    private void StartTimer(MyTimer timer, float duration)
    {
        // タイマーの持続時間を設定する
        timer.duration = duration;
        // タイマーを開始する
        timer.Go();
    }
}
