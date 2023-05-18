using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    public Image Bar; // HPバーの背景画像
    public Image BarLine; // HPバーの境界線画像（左側）
    public Image BarLineRight; // HPバーの境界線画像（右側）
    public Image hpUI; // HP UI
    public float Hp; // 現在のHP
    public float Maxhp; // 最大HP
    public float width; // HPバーの横幅
    public StateManager Sm; // 状態マネージャー

    private void Awake()
    {
        Hp = Sm.HP;                 // 現在のHPを状態マネージャーから取得
        Maxhp = Sm.HPMax;           // 最大HPを状態マネージャーから取得
    }

    private void Start()
    {
        width = Maxhp * 15;         // HPバーの横幅を計算
        BarState();                 // HPバーの初期設定
    }

    void Update()
    {
        BarFiller();                // HPバーの表示更新
    }

    // HPバーの初期設定
    private void BarState()
    {
        Bar.rectTransform.sizeDelta = new Vector2(width, 35f);  // HPバーの背景画像のサイズを設定
        BarLine.rectTransform.sizeDelta = new Vector2(width, 35f); // HPバーの境界線画像（左側）のサイズを設定
        BarLineRight.rectTransform.anchoredPosition = new Vector2(-450f + BarLine.rectTransform.sizeDelta.x, -50f); // HPバーの境界線画像（右側）の位置を設定
    }

    // HPバーの表示更新
    private void BarFiller()
    {
        Bar.fillAmount = Sm.HP / Sm.HPMax; // HPバーの量を現在のHPに合わせて変更
    }
}
