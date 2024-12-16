public class Singleton<T> where T : new()
{
    protected static T sInstance;

    public static bool IsInstanceExists() { return (sInstance != null); }

    public static T Instance()
    {
        if (sInstance == null)
            sInstance = new T();
        return sInstance;
    }

    protected virtual void OnCreate() {}

    public virtual void Release()
    {
        if (sInstance != null)
            sInstance = default(T);
    }
}
