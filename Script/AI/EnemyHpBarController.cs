using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBarController : MonoBehaviour
{
    public Slider HpSlider; // HP���饤���`
    public RectTransform rectTransform; // �쥯�ȥȥ�󥹥ե��`��
    public AIStateManager asm; // AI���Ʃ`�ȥޥͩ`����`

    public Transform target; // ���`���å�
    public Vector2 offestPos; // ���ե��åȥݥ������
    public float Hp; // HP
    public float HpMax; // ���HP
    public float scale = 1f; // �����`��

    public Canvas canvas; // �����Х�

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
    void Update() // ���åץǩ`�ȄI��
    {
        if (target == null)
            return;

        Vector3 tarPos = target.transform.position; // ���`���åȤ�λ�ä�ȡ��
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
            canvas.enabled = false; // �����Х���o����
        }

    }

    public static bool IsGameObjectOnScreen(GameObject gameObject) // ���֥������Ȥ�������`���Ϥˤ��뤫�ɤ������ж�
    {
        Camera camera = Camera.main; 
        Vector3 viewportPoint = camera.WorldToViewportPoint(gameObject.transform.position); 
        return (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 && viewportPoint.z > 0);
    }
}
