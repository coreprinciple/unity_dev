namespace Common
{
    public interface IMonoUpdate
    {
        void DoUpdate(float delta);
    }

    public interface IMonoLateUpdate
    {
        void DoLateUpdate(float delta);
    }

    public interface IMonoFixedUpdate
    {
        void DoFixedUpdate(float delta);
    }
}

