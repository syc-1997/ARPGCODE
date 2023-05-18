using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ActorManagerクラス定義
public class ActorManger : MonoBehaviour
{
    public ActorController ac; // アクターコントローラ
    public BattleManager bm; // バトルマネージャ
    public WeaponManger wm; // ウェポンマネージャ
    public StateManager sm; // ステートマネージャ


    void Awake()
    {
        ac = GetComponent<ActorController>(); // ActorControllerコンポーネントの取得

        GameObject model = ac.model; // モデルオブジェクトの取得
        GameObject obj = GetComponent<GameObject>(); // GameObjectの取得

        GameObject sensor = transform.Find("sensor").gameObject; // センサーオブジェクトの取得
        bm = Bind<BattleManager>(sensor); 
        wm = Bind<WeaponManger>(model); 
        sm = Bind<StateManager>(gameObject); 
    }

    // ジェネリックメソッドで、引数に指定されたGameObjectに対して指定されたタイプのコンポーネントをバインド
    private T Bind<T>(GameObject go) where T : IActorMangerInterface
    {
        T temInstance;
        temInstance = go.GetComponent<T>(); // コンポーネントの取得
        if (temInstance == null) // コンポーネントが存在しない場合
        {
            temInstance = go.AddComponent<T>(); // コンポーネントの追加
        }
        temInstance.am = this; // ActorManagerの設定
        return temInstance; // インスタンスを返す
    }



    // ダメージ
    public void TryDoDamage(AIWeaponController targetWc)
    {
        if (sm.HP > 0) // HPが0より大きい場合
        {
            sm.AddHp(-1 * targetWc.GetATK()); // HPを減少
        }
    }

    // ヒットメソッド
    public void Hit()
    {
        ac.IssueTrigger("hit"); // "hit"トリガーの発行
    }

    // 死亡メソッド
    public void Die()
    {
        ac.IssueTrigger("die"); // "die"トリガーの発行
        ac.pi.inputEnabled = false; // 入力を無効化

        // 一定時間後にゲームオブジェクトを破棄
        Destroy(gameObject, 2f);
    }
}
