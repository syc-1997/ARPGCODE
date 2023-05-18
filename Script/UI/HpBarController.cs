using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    public Image Bar; // HP�Щ`�α�������
    public Image BarLine; // HP�Щ`�ξ��羀������ȣ�
    public Image BarLineRight; // HP�Щ`�ξ��羀�����҂ȣ�
    public Image hpUI; // HP UI
    public float Hp; // �F�ڤ�HP
    public float Maxhp; // ���HP
    public float width; // HP�Щ`�κ��
    public StateManager Sm; // ״�B�ޥͩ`����`

    private void Awake()
    {
        Hp = Sm.HP;                 // �F�ڤ�HP��״�B�ޥͩ`����`����ȡ��
        Maxhp = Sm.HPMax;           // ���HP��״�B�ޥͩ`����`����ȡ��
    }

    private void Start()
    {
        width = Maxhp * 15;         // HP�Щ`�κ����Ӌ��
        BarState();                 // HP�Щ`�γ����O��
    }

    void Update()
    {
        BarFiller();                // HP�Щ`�α�ʾ����
    }

    // HP�Щ`�γ����O��
    private void BarState()
    {
        Bar.rectTransform.sizeDelta = new Vector2(width, 35f);  // HP�Щ`�α�������Υ��������O��
        BarLine.rectTransform.sizeDelta = new Vector2(width, 35f); // HP�Щ`�ξ��羀������ȣ��Υ��������O��
        BarLineRight.rectTransform.anchoredPosition = new Vector2(-450f + BarLine.rectTransform.sizeDelta.x, -50f); // HP�Щ`�ξ��羀�����҂ȣ���λ�ä��O��
    }

    // HP�Щ`�α�ʾ����
    private void BarFiller()
    {
        Bar.fillAmount = Sm.HP / Sm.HPMax; // HP�Щ`������F�ڤ�HP�˺Ϥ碌�Ɖ��
    }
}
