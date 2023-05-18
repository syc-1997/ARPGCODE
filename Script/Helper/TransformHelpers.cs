using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelpers
{
    // 拡張メソッド「DeepFind」の定義
    public static Transform DeepFind(this Transform parent, string targetName)
    {
        Transform tempTrans = null;
        // 子オブジェクトを順番に検索
        foreach (Transform child in parent)
        {
            if (child.name == targetName)
            {
                // 目的の名前と一致するオブジェクトが見つかった場合、そのオブジェクトを返す
                return child;
            }
            else
            {
                // 目的の名前と一致しなかった場合、子オブジェクトの子孫を再帰的に検索
                tempTrans = DeepFind(child, targetName);
                if (tempTrans != null)
                {
                    // 目的の名前と一致するオブジェクトが見つかった場合、そのオブジェクトを返す
                    return tempTrans;
                }
            }
        }
        // 目的の名前と一致するオブジェクトが見つからなかった場合、nullを返す
        return tempTrans;
    }
}
