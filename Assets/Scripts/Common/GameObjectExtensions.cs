using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayerRecursively(this GameObject go, int layer)
    {
        go.layer = layer;
        Transform transform = go.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            childTransform.gameObject.SetLayerRecursively(layer);
        }
    }

    public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
}
