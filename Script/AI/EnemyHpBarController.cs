using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBarController : MonoBehaviour
{
    public Slider HpSlider; // HPスライダー
    public RectTransform rectTransform; // レクトトランスフォーム
    public AIStateManager asm; // AIステートマネージャー

    public Transform target; // ターゲット
    public Vector2 offestPos; // オフセットポジション
    public float Hp; // HP
    public float HpMax; // 最大HP
    public float scale = 1f; // スケール

    public Canvas canvas; // キャンバス

    private void Awake() 
    {
        canvas = gameObject.GetComponentInChildren<Canvas>(); 
        rectTransform = HpSlider.GetComponent<RectTransform>(); 
    }
    void Start() 
    {
        Init();
    }

    void Init() 
    {
        Hp = asm.HP; 
        HpMax = asm.HPMax; 
        HpSlider.value = Hp / HpMax; 
    }

    // Update is called once per frame
    void Update() // アップデート処理
    {
        if (target == null)
            return;

        Vector3 tarPos = target.transform.position; // ターゲットの位置を取得
        float size = Vector3.Distance(tarPos, Camera.main.transform.position); 
        if (IsGameObjectOnScreen(gameObject) && size < 20f) 
        {
            Init(); 
            canvas.enabled = true; 

            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, tarPos); 
            scale = Mathf.Clamp((size - 20f) * -0.1f, 0f, 1f); 
            rectTransform.position = pos + offestPos; 
            rectTransform.localScale = new Vector3(scale, scale, scale); 
        }
        else
        {
            canvas.enabled = false; // キャンバスを無効化
        }

    }

    public static bool IsGameObjectOnScreen(GameObject gameObject) // オブジェクトがスクリーン上にあるかどうかを判定
    {
        Camera camera = Camera.main; 
        Vector3 viewportPoint = camera.WorldToViewportPoint(gameObject.transform.position); 
        return (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 && viewportPoint.z > 0);
    }
}
