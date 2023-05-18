using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("===== Output singnals =====")]
    public float Dup;           // ǰ�������
    public float Dright;        // ���Ҥ�����
    public float Dmag;          // �����Ώ���
    public Vector3 Dvec;        // �����Υ٥��ȥ�
    public float Jup;           // ���祤���ƥ��å�����������
    public float Jright;        // ���祤���ƥ��å�����������

    public bool run;            // �ߤ�
    public bool roll;           // ��`��
    public bool defense;        // ����
    public bool jump;           // ������
    protected bool lastjump;    // ֱǰ�Υ���������
    //public bool attack;
    public bool lb;             // ��Х�ѩ`
    public bool rb;             // �ҥХ�ѩ`
    public bool lt;             // ��ȥꥬ�`
    public bool rt;             // �ҥȥꥬ�`

    public bool lockon;         // ��å�����
    protected bool lastattack;  // ֱǰ�ι�������
    protected bool isHolding = false; // �ܥ���Ѻ����Ƥ��뤫�ɤ���

    [Header("===== Orthers =====")]
    public bool inputEnabled = true;    // �������Є����ɤ���

    protected float targetDup;          // Ŀ��ǰ������
    protected float targetDright;       // Ŀ����������
    protected float velocityDup;        // ǰ���������ٶ�
    protected float velocityDright;     // �����������ٶ�
    protected float holdTime;           // �ܥ���Ѻ����Ƥ���r�g
    protected Vector2 SquareToCircle(Vector2 input) // �Ľ������ˤ�������ˤˉ�Q����
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) * 0.5f);
        return output;
    }

    // ǰ�����Ҥ���������٥��ȥ�������Ώ�����Ӌ�㤹��
    protected void UpdateDmagDvec(float Dup2, float Dright2)
    {
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
    }
}

