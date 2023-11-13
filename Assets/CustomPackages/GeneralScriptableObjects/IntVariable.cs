using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/Variables/Int", order = 0)]
    public class IntVariable : ScriptableObject
    {
        public int Value;

        public UnityAction onValueChanged;

        public void SetValue(int value)
        {
            Value = value;
            onValueChanged?.Invoke();
        }

        public void Increment()
        {
            Value++;
            onValueChanged?.Invoke();
        }

        public void SetValue(IntVariable value)
        {
            Value = value.Value;
            onValueChanged?.Invoke();
        }

        public void ApplyChange(int amount)
        {
            Value += amount;
            onValueChanged?.Invoke();
        }

        public void ApplyChange(IntVariable amount)
        {
            Value += amount.Value;
            onValueChanged?.Invoke();
        }
    }
}