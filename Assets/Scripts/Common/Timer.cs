using System;

namespace Common
{
    public interface ITimer
    {
        void OnTimerStart();
        void OnTimerStop();
    }

    public abstract class Timer
    {
        public float time { get; set; }
        public bool isRunning { get; protected set; }
        public float progress => time / _initTime;

        protected readonly UnorderedArrayContainer<ITimer> _timerListeners = new UnorderedArrayContainer<ITimer>(5);
        protected float _initTime;

        public Action OnTimerStart => delegate { };
        public Action OnTimerStop => delegate { };

        protected Timer(float value)
        {
            _initTime = value;
            isRunning = false;
        }

        public void AddListener(ITimer listener)
        {
            if (_timerListeners.Contains(listener) == false)
                _timerListeners.Add(listener);
        }

        public void RemoveListener(ITimer listener)
        {
            if (_timerListeners.Contains(listener))
                _timerListeners.Remove(listener);
        }

        public void Start()
        {
            time = _initTime;

            if (isRunning)
                return;

            isRunning = true;

            foreach (var listener in _timerListeners)
                listener.OnTimerStart();
        }

        public void Stop()
        {
            if (isRunning == false)
                return;

            isRunning = false;

            foreach (var listener in _timerListeners)
                listener.OnTimerStop();
        }

        public void Resume() => isRunning = true;
        public void Pause() => isRunning = false;

        public abstract void Tick(float delta);
    }

    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }

        public bool isFinished => time <= 0.0f;

        public void SetValue(int value) => _initTime = value;
        public void Reset() => time = _initTime;

        public void Reset(float time)
        {
            _initTime = time;
            Reset();
        }

        public override void Tick(float delta)
        {
            if (isRunning && time > 0)
                time -= delta;

            if (isRunning && time <= 0)
                Stop();
        }
    }

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer(float value) : base(value) { }

        public void Reset() => time = 0;

        public float GetTimer() => time;

        public override void Tick(float delta)
        {
            if (isRunning)
                time += delta;
        }
    }
}

