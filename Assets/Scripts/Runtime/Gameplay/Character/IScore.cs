using System;
using Gameplay.Rewards;

public interface IScore
{
    void ResetScore();
    void AddScore(int _score);
    void RemoveScore(int _score);
    int CurrentScore { get; }
    event Action onScoreChanged;
}