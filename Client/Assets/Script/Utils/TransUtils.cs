using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public sealed class TransUtils
{
    public static Transform InstantiateTransform(Transform prefab, Transform parent, string layer)
    {
        return InstantiateTransform(parent, parent);
    }
    public static Transform InstantiateTransform(Transform prefab, Transform parent)
    {
        if (prefab != null)
        {
            return InstantiateTransform(prefab, parent, prefab.localScale);
        }
        return null;
    }
    public static Transform InstantiateTransform(Transform prefab, Transform parent, Vector3 scale)
    {
        if (prefab != null)
        {
            Transform clone = UnityEngine.Object.Instantiate(prefab) as Transform;
            clone.SetParent(parent);
            clone.localPosition = Vector3.zero;
            clone.localRotation = Quaternion.identity;
            clone.localScale = scale;
            return clone;
        }
        return null;
    }

    public static void EnableCollider(Transform trans,bool isEnable)
    {
        if (trans != null)
        {
            BoxCollider2D boxCollider2D = trans.GetComponent<BoxCollider2D>();
            if (boxCollider2D != null)
            {
                boxCollider2D.enabled = isEnable;
            }
            Collider collider = trans.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = isEnable;
            }
        }
    }

}
