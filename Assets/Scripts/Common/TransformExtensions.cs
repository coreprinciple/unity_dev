using UnityEngine;

public static class TransformExtensions
{
    public static void SetLocalIdentity(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void SetPositionX(this Transform transform, float x)
    {
        Vector3 position = transform.position;
        position.x = x;
        transform.position = position;
    }

    public static void SetPositionY(this Transform transform, float y)
    {
        Vector3 position = transform.position;
        position.y = y;
        transform.position = position;
    }

    public static void SetPositionZ(this Transform transform, float z)
    {
        Vector3 position = transform.position;
        position.z = z;
        transform.position = position;
    }

    public static void SetLocalPositionX(this Transform transform, float x)
    {
        Vector3 localPosition = transform.localPosition;
        localPosition.x = x;
        transform.localPosition = localPosition;
    }

    public static void SetLocalPositionY(this Transform transform, float y)
    {
        Vector3 localPosition = transform.localPosition;
        localPosition.y = y;
        transform.localPosition = localPosition;
    }

    public static void SetLocalPositionZ(this Transform transform, float z)
    {
        Vector3 localPosition = transform.localPosition;
        localPosition.z = z;
        transform.localPosition = localPosition;
    }

    public static void LookAt(this Transform transform, Vector3 position)
    {
        Vector3 dir = Vector3.Normalize(position - transform.position);
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        transform.Rotate(Vector3.up * angle);
    }

    public static void LookToward(this Transform transform, Vector3 direction)
    {
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        transform.Rotate(Vector3.up * angle);
    }
}


