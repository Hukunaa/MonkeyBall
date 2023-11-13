using UnityEngine;

namespace Gameplay.Character.AI
{
    public interface IAIState
    {
        public void Enter();

        public void Update();

        public void Exit();
    }
}