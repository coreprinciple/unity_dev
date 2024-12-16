using UnityEngine;

namespace Common
{
    public class MonoUpdateManager : MonoSingleton<MonoUpdateManager>
    {
        private readonly UnorderedArrayContainer<IMonoUpdate> _updates = new UnorderedArrayContainer<IMonoUpdate>(10);
        private readonly UnorderedArrayContainer<IMonoLateUpdate> _lateUpdates = new UnorderedArrayContainer<IMonoLateUpdate>(10);

#if ENABLE_FIXED_UPDATE
        private readonly UnorderedArrayContainer<IMonoFixedUpdate> _fixedUpdates = new UnorderedArrayContainer<IMonoFixedUpdate>(10);

        public void AddFixedUpdate(IMonoFixedUpdate update)
        {
            if (_fixedUpdates.Contains(update) == false)
                _fixedUpdates.Add(update);
        }

        public void RemoveFixedUpdate(IMonoFixedUpdate update)
        {
            _fixedUpdates.Remove(update);
        }
#endif

        public void AddUpdate(IMonoUpdate update)
        {
            if (_updates.Contains(update) == false)
                _updates.Add(update);
        }

        public void RemoveUpdate(IMonoUpdate update)
        {
            if (_updates.Contains(update))
                _updates.Remove(update);
        }

        public void AddLateUpdate(IMonoLateUpdate update)
        {
            if (_lateUpdates.Contains(update) == false)
                _lateUpdates.Add(update);
        }

        public void RemoveLateUpadte(IMonoLateUpdate update)
        {
            if (_lateUpdates.Contains(update))
                _lateUpdates.Remove(update);
        }

        void Update()
        {
            for (int i = 0; i < _updates.Count; i++)
                _updates[i].DoUpdate(Time.deltaTime);
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _lateUpdates.Count; i++)
                _lateUpdates[i].DoLateUpdate(Time.deltaTime);
        }

#if ENABLE_FIXED_UPDATE
        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdates.count; i++)
                _fixedUpdates[i].DoUpdate(Time.fixedDeltaTime);
        }
#endif

        public void Clear()
        {
            _updates.Dispose();
            _lateUpdates.Dispose();
#if ENABLE_FIXED_UPDATE
            _fixedUpdates.Dispose();
#endif
        }
    }
}

