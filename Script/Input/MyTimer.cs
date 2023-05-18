using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    public enum STATE
    {
        IDLE,      // アイドル状態（何もしていない）
        RUN,       // 実行状態（カウントダウン中）
        FINISHED   // 終了状態（カウントダウンが終わった）
    }
    public STATE state;         // MyTimerの現在の状態を表す列挙型

    public float duration = 1.0f; // タイマーの持続時間
    private float elapsedTime = 0; // 経過時間
    public void Tick()
    {
        switch (state)
        {
            case (STATE.IDLE):  // アイドル状態では何もしない
                break;
            case (STATE.RUN):   // 実行状態では時間をカウントダウンする
                elapsedTime += Time.deltaTime; // 経過時間を更新する
                if (elapsedTime >= duration)   // 持続時間に達したら、状態をFINISHEDに変更する
                {
                    state = STATE.FINISHED;
                }
                break;
            case (STATE.FINISHED): // 終了状態では何もしない
                break;
            default: // 状態が不正な場合はエラーを表示する
                Debug.Log("MyTimer error");
                break;
        }
    }

    public void Go()
    {
        elapsedTime = 0;  // 経過時間を初期化する
        state = STATE.RUN; // 状態をRUNに変更する
    }
}