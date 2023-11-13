namespace Gameplay.Character
{
    public interface ITimer
    {
        void StartTimer();
        void StopTimer();
        void ResetTimer();
        float TimerValue { get; }
    }
}