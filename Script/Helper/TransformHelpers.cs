using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelpers
{
    // �����᥽�åɡ�DeepFind���ζ��x
    public static Transform DeepFind(this Transform parent, string targetName)
    {
        Transform tempTrans = null;
        // �ӥ��֥������Ȥ�혷��˗���
        foreach (Transform child in parent)
        {
            if (child.name == targetName)
            {
                // Ŀ�Ĥ���ǰ��һ�¤��륪�֥������Ȥ�Ҋ�Ĥ��ä����ϡ����Υ��֥������Ȥ򷵤�
                return child;
            }
            else
            {
                // Ŀ�Ĥ���ǰ��һ�¤��ʤ��ä����ϡ��ӥ��֥������Ȥ��ӌO���َ��Ĥ˗���
                tempTrans = DeepFind(child, targetName);
                if (tempTrans != null)
                {
                    // Ŀ�Ĥ���ǰ��һ�¤��륪�֥������Ȥ�Ҋ�Ĥ��ä����ϡ����Υ��֥������Ȥ򷵤�
                    return tempTrans;
                }
            }
        }
        // Ŀ�Ĥ���ǰ��һ�¤��륪�֥������Ȥ�Ҋ�Ĥ���ʤ��ä����ϡ�null�򷵤�
        return tempTrans;
    }
}
